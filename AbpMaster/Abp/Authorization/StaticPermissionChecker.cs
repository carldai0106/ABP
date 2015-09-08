using System;
using Abp.Dependency;

namespace Abp.Authorization
{
    internal static class StaticPermissionChecker<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        private static readonly Lazy<IPermissionChecker<TTenantId, TUserId>> LazyInstance;

        static StaticPermissionChecker()
        {
            if (IocManager.Instance.IsRegistered<IPermissionChecker<TTenantId, TUserId>>())
            {
                LazyInstance = new Lazy<IPermissionChecker<TTenantId, TUserId>>(
                    () => IocManager.Instance.Resolve<IPermissionChecker<TTenantId, TUserId>>()
                    );
            }
            else
            {
                LazyInstance = new Lazy<IPermissionChecker<TTenantId, TUserId>>(
                    () => NullPermissionChecker<TTenantId, TUserId>.Instance
                    );
            }
        }

        public static IPermissionChecker<TTenantId, TUserId> Instance
        {
            get { return LazyInstance.Value; }
        }
    }
}