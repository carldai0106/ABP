using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.Modules;
using CMS.Domain.User;

namespace CMS.Domain
{
    public class DomainModule : AbpModule
    {
        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            if (!Abp.Dependency.IocManager.Instance.IsRegistered<UserService>())
            {
                Abp.Dependency.IocManager.Instance.Register<UserService>();
            }
        }
    }
}
