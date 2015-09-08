using System;
using System.Linq;
using Abp.Collections;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.TestBase;
using CMS.Application;
using CMS.Data.EntityFramework;
using EntityFramework.DynamicFilters;

namespace CMS.Test
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
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
            LoginAsTenant("Default", "admin");
        }

        protected override void AddModules(ITypeList<AbpModule> modules)
        {
            base.AddModules(modules);

            //Adding testing modules. Depended modules of these modules are automatically added.
            modules.Add<CmsAppModule>();
        }

        protected void LoginAsTenant(string tenancyName, string userName)
        {
            var tenant = UsingDbContext(context => context.Tenants.FirstOrDefault(t => t.TenancyName == tenancyName));
            if (tenant == null)
            {
                throw new Exception("There is no tenant: " + tenancyName);
            }

            TTenantId? tenantId = (TTenantId) Convert.ChangeType(tenant.Id, typeof (TTenantId));

            AbpSession.TenantId = tenantId;

            var guid = (Guid) Convert.ChangeType(tenantId, typeof (Guid));

            var user =
                UsingDbContext(
                    context => context.Users.FirstOrDefault(u => u.TenantId == guid && u.UserName == userName));
            if (user == null)
            {
                throw new Exception("There is no user: " + userName + " for tenant: " + tenancyName);
            }

            AbpSession.UserId = (TUserId) Convert.ChangeType(user.Id, typeof (TUserId));
        }

        protected T UsingDbContext<T>(Func<CmsDbContext, T> func)
        {
            T result;

            using (var context = LocalIocManager.Resolve<CmsDbContext>())
            {
                //context.Database.Initialize(false);
                context.DisableAllFilters();
                result = func(context);
                context.SaveChanges();
            }

            return result;
        }
    }
}