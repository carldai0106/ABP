using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Threading;

namespace Abp.Authorization
{
    internal class AuthorizeAttributeHelper<TTenantId, TUserId> : IAuthorizeAttributeHelper<TTenantId, TUserId>,
        ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        public AuthorizeAttributeHelper()
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            PermissionChecker = NullPermissionChecker<TTenantId, TUserId>.Instance;
        }

        public IAbpSession<TTenantId, TUserId> AbpSession { get; set; }
        public IPermissionChecker<TTenantId, TUserId> PermissionChecker { get; set; }

        public async Task AuthorizeAsync(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new AbpAuthorizationException("No user logged in!");
            }

            foreach (var authorizeAttribute in authorizeAttributes)
            {
                //await PermissionChecker.AuthorizeAsync(authorizeAttribute.RequireAllPermissions, authorizeAttribute.Permissions);
                //modify by carl
                await PermissionChecker.AuthorizeAsync(authorizeAttribute);
            }
        }

        public async Task AuthorizeAsync(IAbpAuthorizeAttribute authorizeAttribute)
        {
            await AuthorizeAsync(new[] {authorizeAttribute});
        }

        public void Authorize(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes)
        {
            AsyncHelper.RunSync(() => AuthorizeAsync(authorizeAttributes));
        }

        public void Authorize(IAbpAuthorizeAttribute authorizeAttribute)
        {
            Authorize(new[] {authorizeAttribute});
        }
    }
}