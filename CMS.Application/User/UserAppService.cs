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
using Abp.Extensions;
using Abp.Linq.Extensions;
using CMS.Application.IdentityFramework;
using CMS.Application.Localization;
using CMS.Application.User.Dto;
using CMS.Domain;
using CMS.Domain.User;
using Microsoft.AspNet.Identity;

namespace CMS.Application.User
{
    public class UserAppService : ApplicationService<Guid, Guid>, IUserAppService
    {
        private readonly ICmsRepository<UserEntity, Guid> _repository;
        public UserAppService(ICmsRepository<UserEntity, Guid> repository)
        {
            _repository = repository;
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
                var rs = IdentityResult.Failed(string.Format(L("Identity.DuplicateName"), user.UserName));
                rs.CheckErrors();
            }

            if (await _repository.FirstOrDefaultAsync(x => x.Email == user.Email) != null)
            {
                var rs = IdentityResult.Failed(string.Format(L("Identity.DuplicateEmail"), user.Email));
                rs.CheckErrors();
            }

            if (AbpSession.TenantId.HasValue && AbpSession.TenantId != default(Guid))
            {
                user.TenantId = AbpSession.TenantId.Value;
            }

            await _repository.InsertAsync(user);

            await CurrentUnitOfWork.SaveChangesAsync(); //To get new user's Id.

            
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
            if ( usr != null && usr.Id != input.Id)
            {
                var rs = IdentityResult.Failed(string.Format(L("Identity.DuplicateName"), user.UserName));
                rs.CheckErrors();
            }

            usr = await _repository.FirstOrDefaultAsync(x => x.Email == user.Email);
            if ( usr != null && usr.Id != input.Id)
            {
                var rs = IdentityResult.Failed(string.Format(L("Identity.DuplicateEmail"), user.Email));
                rs.CheckErrors();
            }

            var oldUserName = user.UserName;
            if (oldUserName == "admin" && input.UserName != "admin")
            {
                var rs = IdentityResult.Failed(string.Format(L("CanNotRenameAdminUser"), "admin"));
                rs.CheckErrors();
            }

            await _repository.UpdateAsync(user);
        }

        public async Task DeleteUser(IdInput<Guid> input)
        {
            var usr = await _repository.FirstOrDefaultAsync(x => x.Id == input.Id);
            if (usr.UserName == "admin")
            {
                IdentityResult.Failed(string.Format(L("CanNotDeleteAdminUser"), "admin")).CheckErrors();
            }

            await _repository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultOutput<UserEditDto>> GetUsers(GetUsersInput input)
        {
            var query =  _repository.GetAll().WhereIf(!input.Filter.IsNullOrWhiteSpace(),
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

        public int Add(int a, int b)
        {
            var us = IocManager.Instance.Resolve<UserService>();
            return us.Add(a, b);
        }
    }
}
