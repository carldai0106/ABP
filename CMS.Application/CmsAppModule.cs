using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Localization.Sources;
using Abp.Localization.Sources.Json;
using Abp.Localization.Sources.Xml;
using Abp.Modules;
using CMS.Data;
using CMS.Domain;

namespace CMS.Application
{
    [DependsOn(typeof(CmsDomainModule), typeof(AbpAutoMapperModule), typeof(CmsDataModule))]
    public class CmsAppModule : AbpModule
    {
        public override void PreInitialize<TTenantId, TUserId>()
        {
            base.PreInitialize<TTenantId, TUserId>();
            Configuration.Localization.Languages.Add(new Abp.Localization.LanguageInfo("en-US", "English", null, true));
            Configuration.Localization.Languages.Add(new Abp.Localization.LanguageInfo("zh-CN", "中文"));
            
            Configuration.Localization.Sources.Add(
             new DictionaryBasedLocalizationSource(
                 "cms-en-US",
                 new JsonEmbeddedFileLocalizationDictionaryProvider(
                     Assembly.GetExecutingAssembly(), "CMS.Application.Localization.Resources"
                     )));

            //Configuration.MultiTenancy.IsEnabled = true;
        }

        public override void Initialize<TTenantId, TUserId>()
        {
            //This code is used to register classes to dependency injection system for this assembly using conventions.
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            CustomDtoMapper.CreateMappings();
        }
    }
}
