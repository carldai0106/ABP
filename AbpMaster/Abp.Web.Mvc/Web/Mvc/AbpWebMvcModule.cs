using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Abp.Modules;
using Abp.Web.Mvc.Controllers;
using Abp.Web.Mvc.Localized;

namespace Abp.Web.Mvc
{
    /// <summary>
    /// This module is used to build ASP.NET MVC web sites using Abp.
    /// </summary>
    [DependsOn(typeof(AbpWebModule))]
    public class AbpWebMvcModule : AbpModule
    {
        /// <inheritdoc/>
        public override void PreInitialize<TTenantId, TUserId>()
        {
            IocManager.AddConventionalRegistrar(new ControllerConventionalRegistrar());

            if (!IocManager.IsRegistered(typeof(Translation)))
            {
                IocManager.Register(typeof(Translation));
            }

            //找出缺省的客户端数据验证类型
            var clientDataTypeValidator = ModelValidatorProviders.Providers.OfType<ClientDataTypeModelValidatorProvider>().FirstOrDefault();
            if (null != clientDataTypeValidator)
            {
                //如果有匹配删除该类型
                ModelValidatorProviders.Providers.Remove(clientDataTypeValidator);
            }
            //添加自定义的验证类型
            ModelValidatorProviders.Providers.Add(new FilterableClientDataTypeModelValidatorProvider());

            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LocalizedRequired), typeof(RequiredAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LocalizedStringLength), typeof(StringLengthAttributeAdapter));
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LocalizedRegularExpression), typeof(RegularExpressionAttributeAdapter));
        }

        /// <inheritdoc/>
        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(IocManager.IocContainer.Kernel));
            GlobalFilters.Filters.Add(IocManager.Resolve<AbpHandleErrorAttribute>());
        }
    }
}
