using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Threading;

namespace Abp.Authorization
{
    internal class AuthorizeAttributeHelper<TTenantId, TUserId> : IAuthorizeAttributeHelper<TTenantId, TUserId>, ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        public IAbpSession<TTenantId, TUserId> AbpSession { get; set; }

        public IPermissionChecker<TUserId> PermissionChecker { get; set; }

        public AuthorizeAttributeHelper()
        {
            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            PermissionChecker = NullPermissionChecker<TUserId>.Instance;
        }

        public async Task AuthorizeAsync(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes)
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new AbpAuthorizationException("No user logged in!");
            }

            foreach (var authorizeAttribute in authorizeAttributes)
            {
                await PermissionChecker.AuthorizeAsync(authorizeAttribute.RequireAllPermissions, authorizeAttribute.Permissions);
            }
        }

        public async Task AuthorizeAsync(IAbpAuthorizeAttribute authorizeAttribute)
        {
            await AuthorizeAsync(new[] { authorizeAttribute });
        }

        public void Authorize(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes)
        {
            AsyncHelper.RunSync(() => AuthorizeAsync(authorizeAttributes));
        }

        public void Authorize(IAbpAuthorizeAttribute authorizeAttribute)
        {
            Authorize(new[] { authorizeAttribute });
        }
    }
}