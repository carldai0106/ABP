﻿using System.Reflection;
using Abp.EntityFramework;
using Abp.Modules;
using CMS.Domain;

namespace CMS.Data
{
    [DependsOn(typeof(AbpEntityFrameworkModule), typeof(DomainModule))]
    public class CmsDataModule : AbpModule
    {
        public override void PreInitialize<TTenantId, TUserId>()
        {
            //web.config (or app.config for non-web projects) file should containt a connection string named "Default".
            Configuration.DefaultNameOrConnectionString = "AbpCmsConn";
        }

        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
