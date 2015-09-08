using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.NHibernate.Filters;
using Abp.NHibernate.Interceptors;
using Abp.NHibernate.Repositories;
using Abp.Reflection;
using NHibernate;


namespace Abp.NHibernate
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in NHibernate.
    /// </summary>
    public class AbpNHibernateModule : AbpModule
    {
        /// <summary>
        /// NHibernate session factory object.
        /// </summary>
        private ISessionFactory _sessionFactory;
        private readonly ITypeFinder _typeFinder;

        public AbpNHibernateModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        /// <inheritdoc/>
        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.Register<AbpNHibernateInterceptor<TTenantId, TUserId>>(DependencyLifeStyle.Transient);

            _sessionFactory = Configuration.Modules.AbpNHibernate().FluentConfiguration
                .Mappings(m => m.FluentMappings.Add(typeof(MayHaveTenantFilter)))
                .Mappings(m => m.FluentMappings.Add(typeof(MustHaveTenantFilter)))
                .ExposeConfiguration(config => config.SetInterceptor(IocManager.Resolve<AbpNHibernateInterceptor<TTenantId, TUserId>>()))
                .BuildSessionFactory();

            IocManager.IocContainer.Install(new NhRepositoryInstaller(_sessionFactory));
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            NhGenericRepositoryRegistrar.RegisterGenericRepositories<TTenantId, TUserId>(_typeFinder, IocManager);
        }

        /// <inheritdoc/>
        public override void Shutdown()
        {
            _sessionFactory.Dispose();
        }
    }
}
