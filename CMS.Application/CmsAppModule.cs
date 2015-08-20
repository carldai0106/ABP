using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Localization.Sources;
using Abp.Localization.Sources.Json;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.Mvc.Localized;
using CMS.Application.Authorization;
using CMS.Application.Localization;
using CMS.Data;
using CMS.Domain;

namespace CMS.Application
{
    [DependsOn(typeof(AbpWebMvcModule), typeof(CmsDomainModule), typeof(AbpAutoMapperModule), typeof(CmsDataModule), typeof(AbpExtensionsModule))]
    public class CmsAppModule : AbpModule
    {
        public override void PreInitialize<TTenantId, TUserId>()
        {
            IocManager.Register<IPermissionChecker<Guid, Guid>, PermissionChecker>();

            base.PreInitialize<TTenantId, TUserId>();
            Configuration.Localization.Languages.Add(new Abp.Localization.LanguageInfo("en-US", "English", "famfamfam-flag-gb", true));
            Configuration.Localization.Languages.Add(new Abp.Localization.LanguageInfo("zh-CN", "中文", "famfamfam-flag-cn"));
            
            Configuration.Localization.Sources.Add(
             new DictionaryBasedLocalizationSource(
                 "cms-en-US",
                 new JsonEmbeddedFileLocalizationDictionaryProvider(
                     Assembly.GetExecutingAssembly(), "CMS.Application.Localization.Resources"
                     )));

            Configuration.MultiTenancy.IsEnabled = true;
        }

        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.Resolve<Translation>().LocalizationSourceName =
                   CmsConsts.LocalizationSourceName;

            //This code is used to register classes to dependency injection system for this assembly using conventions.
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            CustomDtoMapper.CreateMappings();
        }
    }
}
