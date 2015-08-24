using System.Reflection;
using Abp.Collections.Extensions;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Modules;
using Abp.NHibernate.Filter;
using Abp.NHibernate.Interceptors;
using Abp.NHibernate.Repositories;
using Abp.Reflection;
using Abp.Reflection.Extensions;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;
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
        }

        /// <inheritdoc/>
        public override void Shutdown()
        {
            _sessionFactory.Dispose();
        }

        ///// <summary>
        ///// NHibernate session factory object.
        ///// </summary>
        //private ISessionFactory _sessionFactory;

        //private readonly ITypeFinder _typeFinder;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="typeFinder"></param>
        //public AbpNHibernateModule(ITypeFinder typeFinder)
        //{
        //    _typeFinder = typeFinder;
        //}

        ///// <inheritdoc/>
        //public override void Initialize<TTenantId, TUserId>()
        //{
        //    IocManager.Register<AbpNHibernateInterceptor<TTenantId, TUserId>>(DependencyLifeStyle.Transient);

        //    _sessionFactory = Configuration.Modules.AbpNHibernate().FluentConfiguration
        //        .ExposeConfiguration(config => config.SetInterceptor(IocManager.Resolve<AbpNHibernateInterceptor<TTenantId, TUserId>>()))
        //        .BuildSessionFactory();

        //    IocManager.IocContainer.Install(new NhRepositoryInstaller(_sessionFactory));

        //    //modify by carl
        //    RegisterGenericRepositories<TTenantId, TUserId>();

        //    IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        //}

        ////modify by carl
        //private void RegisterGenericRepositories<TTenantId, TUserId>()
        //    where TTenantId : struct
        //    where TUserId : struct
        //{
        //    var entities = _typeFinder.Find(
        //         x => (
        //             typeof(IEntity).IsAssignableFrom(x) ||
        //             x.IsInheritsOrImplements(typeof(IEntity<>))
        //             )
        //         );

        //    if (entities.IsNullOrEmpty())
        //    {
        //        throw new AbpException("No class found derived from Entity or Entity<>.");
        //    }

        //    foreach (var entityType in entities)
        //    {
        //        var interfaces = entityType.GetInterfaces();
        //        foreach (var interfaceType in interfaces)
        //        {
        //            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
        //            {
        //                var primaryKeyType = interfaceType.GenericTypeArguments[0];
        //                if (primaryKeyType.Name == "TPrimaryKey" || string.IsNullOrEmpty(primaryKeyType.FullName))
        //                {
        //                    continue;
        //                }

        //                if (primaryKeyType == typeof(int))
        //                {
        //                    var genericRepositoryType = typeof(IRepository<>).MakeGenericType(entityType);
        //                    if (!IocManager.IsRegistered(genericRepositoryType))
        //                    {
        //                        IocManager.Register(
        //                            genericRepositoryType,
        //                            typeof(NhRepositoryBase<,,>).MakeGenericType(entityType, typeof(TTenantId),
        //                                typeof(TUserId)),
        //                            DependencyLifeStyle.Transient
        //                            );
        //                    }
        //                }

        //                var genericRepositoryTypeWithPrimaryKey = typeof(IRepository<,>).MakeGenericType(entityType,
        //                    primaryKeyType);
        //                if (!IocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
        //                {
        //                    IocManager.Register(
        //                        genericRepositoryTypeWithPrimaryKey,
        //                        typeof(NhRepositoryBase<,,,>).MakeGenericType(entityType, primaryKeyType,
        //                            typeof(TTenantId), typeof(TUserId)),
        //                        DependencyLifeStyle.Transient
        //                        );
        //                }
        //            }
        //        }
        //    }
        //}

        ///// <inheritdoc/>
        //public override void Shutdown()
        //{
        //    _sessionFactory.Dispose();
        //}
    }
}
