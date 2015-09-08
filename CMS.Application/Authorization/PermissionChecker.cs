using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Runtime.Session;
using CMS.Application.User;

namespace CMS.Application.Authorization
{
    public class PermissionChecker : IPermissionChecker<Guid, Guid>, ITransientDependency
    {
        private readonly IUserAppService _userAppService;

        public PermissionChecker(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        public IAbpSession<Guid, Guid> AbpSession { get; set; }

        public Task<bool> IsGrantedAsync(string permissionName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsGrantedAsync(Guid userId, string permissionName)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsGrantedAsync(string moduleCode, string permissionName)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new AbpAuthorizationException("No user logged in!");
            }

            var list =
                await
                    _userAppService.GetPermission(new NullableIdInput<Guid> {Id = AbpSession.UserId.Value}, moduleCode,
                        permissionName);
            return list.Any(x => x.Status);
        }
    }
}