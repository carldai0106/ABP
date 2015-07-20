using System.Reflection;
using Abp.Modules;
using Abp.Web.Api.Tests.DynamicApiController.Clients;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Clients;

namespace Abp.Web.Api.Tests
{
    [DependsOn(typeof(AbpWebApiModule))]
    public class AbpWebApiTestModule : AbpModule
    {
        public override void PreInitialize<TTenantId, TUserId>()
        {
            base.PreInitialize<TTenantId, TUserId>();
            Configuration.Localization.IsEnabled = false;
        }

        public override void Initialize<TTenantId, TUserId>()
        {
            base.Initialize<TTenantId, TUserId>();
            
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            
            DynamicApiClientBuilder
                .For<IMyAppService>("http://www.aspnetboilerplate.com/api/services/myapp/myservice")
                .Build();
        }
    }
}