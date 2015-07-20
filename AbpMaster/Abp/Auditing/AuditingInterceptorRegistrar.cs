using System;
using System.Linq;
using Abp.Dependency;
using Castle.Core;
using Castle.MicroKernel;

namespace Abp.Auditing
{
    internal class AuditingInterceptorRegistrar
        //<TTenantId, TUserId>
        //where TTenantId : struct
        //where TUserId : struct
    {
        private readonly IAuditingConfiguration _auditingConfiguration;
        private readonly IIocManager _iocManager;

        public AuditingInterceptorRegistrar(IAuditingConfiguration auditingConfiguration, IIocManager iocManager)
        {
            _auditingConfiguration = auditingConfiguration;
            _iocManager = iocManager;
        }

        public void Initialize<TTenantId, TUserId>()
            where TTenantId : struct
            where TUserId : struct
        {
            if (!_auditingConfiguration.IsEnabled)
            {
                return;
            }

            //_iocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;
            _iocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered<TTenantId, TUserId>;
        }
       

        private void Kernel_ComponentRegistered<TTenantId, TUserId>(string key, IHandler handler)
            where TTenantId : struct
            where TUserId : struct
        {
            if (ShouldIntercept(handler.ComponentModel.Implementation))
            {
                //todo : modify by carl
                //handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AuditingInterceptor<,>)));
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AuditingInterceptor<TTenantId,TUserId>)));
            }
        }

        private bool ShouldIntercept(Type type)
        {
            if (_auditingConfiguration.Selectors.Any(selector => selector.Predicate(type)))
            {
                return true;
            }

            if (type.IsDefined(typeof(AuditedAttribute), true)) //TODO: true or false?
            {
                return true;
            }

            if (type.GetMethods().Any(m => m.IsDefined(typeof(AuditedAttribute), true))) //TODO: true or false?
            {
                return true;
            }

            return false;
        }
    }
}