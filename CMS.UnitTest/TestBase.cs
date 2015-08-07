using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Abp.Collections;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using Abp.TestBase;
using CMS.Application;
using CMS.Data;
using CMS.Data.EntityFramework;
using CMS.Domain;

namespace CMS.UnitTest
{
    public class TestBase<TTenantId, TUserId> : AbpIntegratedTestBase<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        public TestBase()
        {
            //LocalIocManager.IocContainer.Register(
            //    Component.For<DbConnection>()
            //        .UsingFactoryMethod(Effort.DbConnectionFactory.CreateTransient)
            //        .LifestyleSingleton()
            //    );

            //LocalIocManager.IocContainer.Register(Component.For<CmsDbContext>());
        }

        protected override void AddModules(ITypeList<AbpModule> modules)
        {
            base.AddModules(modules);

            //Adding testing modules. Depended modules of these modules are automatically added.
            modules.Add<CmsAppModule>();
            //modules.Add<DomainModule>();
            //modules.Add<CmsDataModule>();
        }
    }
}
