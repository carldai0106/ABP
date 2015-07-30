using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using CMS.Application.IdentityFramework;
using CMS.Application.Localization;
using CMS.Application.User.Dto;
using CMS.Domain;
using CMS.Domain.Action;
using CMS.Domain.Module;
using CMS.Domain.Role;
using CMS.Domain.RoleRight;
using CMS.Domain.User;
using CMS.Domain.UserRole;
using Microsoft.AspNet.Identity;

namespace CMS.Application.User
{
    public class UserAppService : ApplicationService<Guid, Guid>, IUserAppService
    {
        private readonly ICmsRepository<UserEntity, Guid> _repository;
        private readonly ICmsRepository<UserRoleEntity, Guid> _userRoleRepository;

        private readonly ICmsRepository<ModuleEntity, Guid> _moduleRepository;
        private readonly ICmsRepository<ActionEntity, Guid> _actionRepository;
        private readonly ICmsRepository<RoleRightEntity, Guid> _roleRightRepository;


        public UserAppService(
            ICmsRepository<UserEntity, Guid> repository, 
            ICmsRepository<UserRoleEntity, Guid> userRoleRepository,
            ICmsRepository<ModuleEntity, Guid> moduleRepository,
            ICmsRepository<ActionEntity, Guid> actionRepository,
            ICmsRepository<RoleRightEntity, Guid> roleRightRepository
            )
        {
            _repository = repository;
            _userRoleRepository = userRoleRepository;
            _moduleRepository = moduleRepository;
            _actionRepository = actionRepository;
            _roleRightRepository = roleRightRepository;

            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }

        public async Task CreateUser(CreateUserDto input)
        {
            var user = input.MapTo<UserEntity>();
            user.Id = Guid.NewGuid();

            if (!input.Password.IsNullOrEmpty())
            {
                var rs = await new PasswordValidator().ValidateAsync(input.Password);
                rs.CheckErrors();
            }
            else
            {
                input.Password = UserEntity.CreateRandomPassword();
            }

            user.Password = new PasswordHasher().HashPassword(input.Password);


            if (await _repository.FirstOrDefaultAsync(x => x.UserName == user.UserName) != null)
            {
                throw new UserFriendlyException(string.Format(L("Identity.DuplicateName"), user.UserName));
            }

            if (await _repository.FirstOrDefaultAsync(x => x.Email == user.Email) != null)
            {
                throw new UserFriendlyException(string.Format(L("Identity.DuplicateEmail"), user.Email));
            }

            if (AbpSession.TenantId.HasValue && AbpSession.TenantId != default(Guid))
            {
                user.TenantId = AbpSession.TenantId.Value;
            }

            await _repository.InsertAsync(user);
        }

        public async Task<UserEditDto> GetUser(string userName)
        {
            var user = await _repository.FirstOrDefaultAsync(x => x.UserName == userName);
            var ue = user.MapTo<UserEditDto>();
            //ue.Id = user.Id;
            return ue;
        }

        public async Task<UserEditDto> GetUser(NullableIdInput<Guid> input)
        {
            if (input.Id.HasValue)
            {
                var user = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
                return user.MapTo<UserEditDto>();
            }

            return new UserEditDto();
        }

        public async Task UpdateUser(UserEditDto input)
        {
            var user = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (user == null)
            {
                throw new AbpException("There is no user with id: " + input.Id);
            }

            input.MapTo(user);

            if (!input.Password.IsNullOrEmpty())
            {
                var rs = await new PasswordValidator().ValidateAsync(input.Password);
                rs.CheckErrors();

                user.Password = new PasswordHasher().HashPassword(input.Password);
            }


            var usr = await _repository.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (usr != null && usr.Id != input.Id)
            {
                throw new UserFriendlyException(string.Format(L("Identity.DuplicateName"), user.UserName));
            }

            usr = await _repository.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (usr != null && usr.Id != input.Id)
            {
                throw new UserFriendlyException(string.Format(L("Identity.DuplicateEmail"), user.Email));
            }

            var oldUserName = user.UserName;
            if (oldUserName == "admin" && input.UserName != "admin")
            {
                throw new UserFriendlyException(string.Format(L("CanNotRenameAdminUser"), "admin"));
            }

            await _repository.UpdateAsync(user);

        }

        public async Task DeleteUser(IdInput<Guid> input)
        {
            var usr = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (usr.UserName == "admin")
            {
                throw new UserFriendlyException(string.Format(L("CanNotDeleteAdminUser"), "admin"));
            }

            await _repository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<UserEditDto>> GetUsers(GetUsersInput input)
        {
            var query = _repository.GetAll().WhereIf(!input.Filter.IsNullOrWhiteSpace(),
                u => u.FirstName.Contains(input.Filter) ||
                     u.LastName.Contains(input.Filter)
                     || u.UserName.Contains(input.Filter)
                     || u.Email.Contains(input.Filter)
                );


            var count = await query.CountAsync();
            var users = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            var userListDtos = users.MapTo<List<UserEditDto>>();

            return new PagedResultOutput<UserEditDto>(count, userListDtos);
        }

        public async Task CreateOrUpdate(IEnumerable<UserRoleDto> inputs)
        {
            foreach (var item in inputs)
            {
                var rs = item.MapTo<UserRoleEntity>();
                if (!item.Id.HasValue)
                {
                    if (
                        await
                            _userRoleRepository.FirstOrDefaultAsync(
                                x => x.UserId == item.UserId && x.RoleId == item.RoleId) == null)
                    {
                        rs.Id = Guid.NewGuid();
                        await _userRoleRepository.InsertAsync(rs);
                    }
                }
                else
                {
                    await _userRoleRepository.UpdateAsync(rs);
                }
            }
        }

        public async Task<List<PermissionDto>> GetPermission(NullableIdInput<Guid> userId, string moduleCode, string actionCode)
        {
            var modules = _moduleRepository.GetAll();
            var actions = _actionRepository.GetAll();
            var roleRightQuery = _roleRightRepository.GetAll();
            var userRoles = _userRoleRepository.GetAll();

            var userRole_JoinRole_Right_Query = userRoles.WhereIf(userId.Id != null, x => x.UserId == userId.Id).Join(roleRightQuery, x => x.RoleId, y => y.RoleId, (x, y) => new
            {
                RoleId = y.RoleId,
                RoleCode = x.Role.RoleCode,
                Status = y.Status,
                ModuleId = y.ActionModule.ModuleId,
                ActionId = y.ActionModule.ActionId,
                ActionModuleStatus = y.ActionModule.Status
            }).Where(x => x.ActionModuleStatus);

            var userRole_JoinRole_Right_Query_Join_Module_Query = userRole_JoinRole_Right_Query.Join(modules, x => x.ModuleId, y => y.Id, (x, y) => new
            {
                RoleId = x.RoleId,
                RoleCode = x.RoleCode,
                ModuleId = y.Id,
                ModuleCode = y.ModuleCode,
                DisplayName = y.DisplayName,
                ActionId = x.ActionId,
                Status = x.Status,
            }).WhereIf(!moduleCode.IsNullOrWhiteSpace(), x => x.ModuleCode == moduleCode);

            var query = userRole_JoinRole_Right_Query_Join_Module_Query.Join(actions, x => x.ActionId, y => y.Id, (x, y) => new PermissionDto
            {
                ActionId = x.ActionId,
                ActionCode = y.ActionCode,
                ModuleId = x.ModuleId,
                ModuleCode = x.ModuleCode,
                DisplayName = x.DisplayName,
                RoleId = x.RoleId,
                RoleCode = x.RoleCode,
                Status = x.Status
            }).WhereIf(!actionCode.IsNullOrWhiteSpace(), x=> x.ActionCode == actionCode);

            var rs = await query.ToListAsync();

            return rs;
        }
    }
}
