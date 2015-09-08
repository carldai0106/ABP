using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.Authorization
{
    /// <summary>
    ///     Permission manager.
    /// </summary>
    internal class PermissionManager<TTenantId, TUserId> : PermissionDefinitionContextBase,
        IPermissionManager<TTenantId, TUserId>, ISingletonDependency
        where TTenantId : struct
        where TUserId : struct
    {
        private readonly IAuthorizationConfiguration _authorizationConfiguration;
        private readonly IIocManager _iocManager;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public PermissionManager(IIocManager iocManager, IAuthorizationConfiguration authorizationConfiguration)
        {
            _iocManager = iocManager;
            _authorizationConfiguration = authorizationConfiguration;

            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
        }

        public IAbpSession<TTenantId, TUserId> AbpSession { get; set; }

        public Permission GetPermission(string name)
        {
            var permission = Permissions.GetOrDefault(name);
            if (permission == null)
            {
                throw new AbpException("There is no permission with name: " + name);
            }

            return permission;
        }

        public IReadOnlyList<Permission> GetAllPermissions(bool tenancyFilter = true)
        {
            return Permissions.Values
                .WhereIf(tenancyFilter, p => p.MultiTenancySides.HasFlag(AbpSession.MultiTenancySide))
                .ToImmutableList();
        }

        public IReadOnlyList<Permission> GetAllPermissions(MultiTenancySides multiTenancySides)
        {
            return Permissions.Values
                .Where(p => p.MultiTenancySides.HasFlag(multiTenancySides))
                .ToImmutableList();
        }

        public void Initialize()
        {
            foreach (var providerType in _authorizationConfiguration.Providers)
            {
                CreateAuthorizationProvider(providerType).SetPermissions(this);
            }

            Permissions.AddAllPermissions();
        }

        private AuthorizationProvider CreateAuthorizationProvider(Type providerType)
        {
            if (!_iocManager.IsRegistered(providerType))
            {
                _iocManager.Register(providerType);
            }

            return (AuthorizationProvider) _iocManager.Resolve(providerType);
        }
    }
}