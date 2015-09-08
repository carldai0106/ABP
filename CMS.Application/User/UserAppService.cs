using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Security;
using Abp.UI;
using AutoMapper;
using CMS.Application.IdentityFramework;
using CMS.Application.User.Dto;
using CMS.Domain;
using CMS.Domain.Action;
using CMS.Domain.Module;
using CMS.Domain.RoleRight;
using CMS.Domain.Tenant;
using CMS.Domain.User;
using CMS.Domain.UserRole;
using Microsoft.AspNet.Identity;

namespace CMS.Application.User
{
    public class UserAppService : CmsAppServiceBase, IUserAppService
    {
        private readonly ICmsRepository<ActionEntity, Guid> _actionRepository;
        private readonly ICmsRepository<ModuleEntity, Guid> _moduleRepository;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ICmsRepository<UserEntity, Guid> _repository;
        private readonly ICmsRepository<RoleRightEntity, Guid> _roleRightRepository;
        private readonly ICmsRepository<TenantEntity, Guid> _tenantRepository;
        private readonly ICmsRepository<UserRoleEntity, Guid> _userRoleRepository;

        public UserAppService(
            ICmsRepository<UserEntity, Guid> repository,
            ICmsRepository<UserRoleEntity, Guid> userRoleRepository,
            ICmsRepository<ModuleEntity, Guid> moduleRepository,
            ICmsRepository<ActionEntity, Guid> actionRepository,
            ICmsRepository<RoleRightEntity, Guid> roleRightRepository,
            ICmsRepository<TenantEntity, Guid> tenantRepository,
            IMultiTenancyConfig multiTenancyConfig
            )
        {
            _repository = repository;
            _userRoleRepository = userRoleRepository;
            _moduleRepository = moduleRepository;
            _actionRepository = actionRepository;
            _roleRightRepository = roleRightRepository;
            _multiTenancyConfig = multiTenancyConfig;
            _tenantRepository = tenantRepository;
        }

        public async Task<Guid> CreateUser(CreateUserDto input)
        {
            var user = input.MapTo<UserEntity>();
            var userId = Guid.NewGuid();
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

            return user.Id;
        }

        public async Task<UserEditDto> GetUser(string userNameOrEmail)
        {
            var user =
                await _repository.FirstOrDefaultAsync(x => x.UserName == userNameOrEmail || x.Email == userNameOrEmail);
            
            var ue = user.MapTo<UserEditDto>();
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

        public async Task<List<PermissionDto>> GetPermission(NullableIdInput<Guid> userId, string moduleCode,
            string actionCode)
        {
            var modules = _moduleRepository.GetAll();
            var actions = _actionRepository.GetAll();
            var roleRightQuery = _roleRightRepository.GetAll();
            var userRoles = _userRoleRepository.GetAll();

            var userRole_JoinRole_Right_Query = userRoles.WhereIf(userId.Id != null, x => x.UserId == userId.Id)
                .Join(roleRightQuery, x => x.RoleId, y => y.RoleId, (x, y) => new
                {
                    y.RoleId,
                    x.Role.RoleCode,
                    y.Status,
                    y.ActionModule.ModuleId,
                    y.ActionModule.ActionId,
                    ActionModuleStatus = y.ActionModule.Status
                }).Where(x => x.ActionModuleStatus);

            var userRole_JoinRole_Right_Query_Join_Module_Query = userRole_JoinRole_Right_Query.Join(modules,
                x => x.ModuleId, y => y.Id, (x, y) => new
                {
                    x.RoleId,
                    x.RoleCode,
                    ModuleId = y.Id,
                    y.ModuleCode,
                    y.DisplayName,
                    x.ActionId,
                    x.Status
                }).WhereIf(!moduleCode.IsNullOrWhiteSpace(), x => x.ModuleCode == moduleCode);

            var query = userRole_JoinRole_Right_Query_Join_Module_Query.Join(actions, x => x.ActionId, y => y.Id,
                (x, y) => new PermissionDto
                {
                    ActionId = x.ActionId,
                    ActionCode = y.ActionCode,
                    ModuleId = x.ModuleId,
                    ModuleCode = x.ModuleCode,
                    DisplayName = x.DisplayName,
                    RoleId = x.RoleId,
                    RoleCode = x.RoleCode,
                    Status = x.Status
                }).WhereIf(!actionCode.IsNullOrWhiteSpace(), x => x.ActionCode == actionCode);

            var rs = await query.ToListAsync();

            return rs;
        }

        public async Task<LoginResultDto> Login(string userNameOrEmailAddress, string plainPassword,
            string tenancyName = null)
        {
            if (userNameOrEmailAddress.IsNullOrEmpty())
            {
                throw new ArgumentNullException("userNameOrEmailAddress");
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException("plainPassword");
            }

            TenantEntity tenant = null;
            if (!_multiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (!string.IsNullOrWhiteSpace(tenancyName))
            {
                tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == tenancyName);
                if (tenant == null)
                {
                    return new LoginResultDto(LoginResultType.InvalidTenancyName);
                }

                if (!tenant.IsActive)
                {
                    return new LoginResultDto(LoginResultType.TenantIsNotActive);
                }
            }

            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MustHaveTenant))
            {
                var user = await _repository.FirstOrDefaultAsync(x =>
                    x.TenantId == tenant.Id &&
                    (x.UserName == userNameOrEmailAddress || x.Email == userNameOrEmailAddress));
                
                if (user == null)
                {
                    return new LoginResultDto(LoginResultType.InvalidUserNameOrEmailAddress);
                }

                var verificationResult = new PasswordHasher().VerifyHashedPassword(user.Password, plainPassword);

                if (verificationResult != PasswordVerificationResult.Success)
                {
                    return new LoginResultDto(LoginResultType.InvalidPassword);
                }

                if (!user.IsActive)
                {
                    return new LoginResultDto(LoginResultType.UserIsNotActive);
                }

                var info = user.MapTo<UserEditDto>();

                return new LoginResultDto(info, CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
            }
        }

        private async Task<TenantEntity> GetDefaultTenantAsync()
        {
            var tenant = await _tenantRepository.FirstOrDefaultAsync(t => t.TenancyName == "Default");
            if (tenant == null)
            {
                throw new AbpException("There should be a 'Default' tenant if multi-tenancy is disabled!");
            }

            return tenant;
        }

        private ClaimsIdentity CreateIdentity(UserEntity user, string authenticationType)
        {
            var roleIds = user.UserRoles.Select(x => x.RoleId).JoinAsString(",");
            var identity = new ClaimsIdentity(authenticationType, AbpClaimTypes.UserNameClaimType,
                AbpClaimTypes.RoleClaimType);

            identity.AddClaim(new Claim(AbpClaimTypes.UserIdClaimType, user.Id.ToString(),
                "http://www.w3.org/2001/XMLSchema#string"));
            identity.AddClaim(new Claim(AbpClaimTypes.UserNameClaimType, user.UserName,
                "http://www.w3.org/2001/XMLSchema#string"));
            identity.AddClaim(new Claim(AbpClaimTypes.RoleClaimType, roleIds, "http://www.w3.org/2001/XMLSchema#string"));
            identity.AddClaim(new Claim(AbpClaimTypes.TenantId, user.TenantId.ToString(),
                "http://www.w3.org/2001/XMLSchema#string"));

            return identity;
        }
    }
}