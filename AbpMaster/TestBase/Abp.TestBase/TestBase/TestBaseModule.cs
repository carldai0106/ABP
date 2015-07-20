using System.Reflection;
using Abp.Modules;

namespace Abp.TestBase
{
    [DependsOn(typeof(AbpKernelModule))]
    public class TestBaseModule : AbpModule
    {
        public override void PreInitialize<TTenantId, TUserId>()
        {
            Configuration.EventBus.UseDefaultEventBus = false;
        }

        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}