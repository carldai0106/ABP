using System.Reflection;
using Abp.Modules;

namespace CMS.Domain
{
    public class CmsDomainModule : AbpModule
    {
        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}