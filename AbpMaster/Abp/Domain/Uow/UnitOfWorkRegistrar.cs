using System.Linq;
using System.Reflection;
using Abp.Dependency;
using Castle.Core;
using Castle.MicroKernel;

namespace Abp.Domain.Uow
{
    /// <summary>
    ///     This class is used to register interceptor for needed classes for Unit Of Work mechanism.
    /// </summary>
    internal static class UnitOfWorkRegistrar
    {
        /// <summary>
        ///     Initializes the registerer.
        /// </summary>
        /// sssss
        /// <param name="iocManager">IOC manager</param>
        public static void Initialize<TTenantId, TUserId>(IIocManager iocManager)
            where TTenantId : struct
            where TUserId : struct
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += ComponentRegistered<TTenantId, TUserId>;
        }

        private static void ComponentRegistered<TTenantId, TUserId>(string key, IHandler handler)
            where TTenantId : struct
            where TUserId : struct
        {
            if (UnitOfWorkHelper.IsConventionalUowClass(handler.ComponentModel.Implementation))
            {
                //Intercept all methods of all repositories.
                handler.ComponentModel.Interceptors.Add(
                    new InterceptorReference(typeof (UnitOfWorkInterceptor<TTenantId, TUserId>)));
            }
            else if (
                handler.ComponentModel.Implementation.GetMethods(BindingFlags.Instance | BindingFlags.Public |
                                                                 BindingFlags.NonPublic)
                    .Any(UnitOfWorkHelper.HasUnitOfWorkAttribute))
            {
                //Intercept all methods of classes those have at least one method that has UnitOfWork attribute.
                //TODO: Intecept only UnitOfWork methods, not other methods!
                handler.ComponentModel.Interceptors.Add(
                    new InterceptorReference(typeof (UnitOfWorkInterceptor<TTenantId, TUserId>)));
            }
        }
    }
}