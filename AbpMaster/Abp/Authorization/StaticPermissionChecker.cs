using System;
using Abp.Dependency;

namespace Abp.Authorization
{
    internal static class StaticPermissionChecker<TUserId>
        where TUserId : struct
    {
        public static IPermissionChecker<TUserId> Instance { get { return LazyInstance.Value; } }
        private static readonly Lazy<IPermissionChecker<TUserId>> LazyInstance;

        static StaticPermissionChecker()
        {
            LazyInstance = new Lazy<IPermissionChecker<TUserId>>(
                () => IocManager.Instance.IsRegistered<IPermissionChecker<TUserId>>()
                    ? IocManager.Instance.Resolve<IPermissionChecker<TUserId>>()
                    : NullPermissionChecker<TUserId>.Instance
                );
        }
    }
}