using System.Reflection;
using Abp.Collections.Extensions;
using Abp.EntityFramework.Dependency;
using Abp.EntityFramework.Repositories;
using Abp.EntityFramework.Uow;
using Abp.Modules;
using Abp.Reflection;
using Abp.Reflection.Extensions;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;

namespace Abp.EntityFramework
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in EntityFramework.
    /// </summary>
    public class AbpEntityFrameworkModule : AbpModule
    {
        public ILogger Logger { get; set; }

        private readonly ITypeFinder _typeFinder;

        public AbpEntityFrameworkModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
            Logger = NullLogger.Instance;
        }

        public override void PreInitialize<TTenantId, TUserId>()
        {
            IocManager.AddConventionalRegistrar(new EntityFrameworkConventionalRegisterer<TTenantId, TUserId>());
        }

        public override void Initialize<TTenantId, TUserId>()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component.For(typeof(IDbContextProvider<,,>))
                    .ImplementedBy(typeof(UnitOfWorkDbContextProvider<,,>))
                    .LifestyleTransient()
                );

            RegisterGenericRepositories<TTenantId, TUserId>();
            //EntityFrameworkGenericRepositoryRegistrar.RegisterGenericRepositories<TTenantId, TUserId>(_typeFinder, IocManager);
        }

        private void RegisterGenericRepositories<TTenantId, TUserId>()
            where TTenantId : struct
            where TUserId : struct
        {
            var dbContextTypes =
                _typeFinder.Find(type =>
                    type.IsPublic &&
                    !type.IsAbstract &&
                    type.IsClass &&
                    type.IsInheritsOrImplements(typeof(AbpDbContext<,>)));

            if (dbContextTypes.IsNullOrEmpty())
            {
                Logger.Warn("No class found derived from AbpDbContext.");
                return;
            }

            foreach (var dbContextType in dbContextTypes)
            {
                EntityFrameworkGenericRepositoryRegistrar.RegisterForDbContext<TTenantId, TUserId>(dbContextType, IocManager);
            }
        }
    }
}
