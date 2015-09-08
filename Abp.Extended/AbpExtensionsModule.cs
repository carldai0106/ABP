using System.Reflection;
using Abp.Modules;

namespace Abp
{
    public class AbpExtensionsModule : AbpModule
    {
        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}