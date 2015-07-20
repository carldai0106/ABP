﻿using System;
using System.Reflection;
using Abp.Application.Navigation;
using Abp.Application.Services;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Interceptors;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.Localization.Sources.Xml;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Runtime.Validation.Interception;

namespace Abp
{
    /// <summary>
    /// Kernel (core) module of the ABP system.
    /// No need to depend on this, it's automatically the first module always.
    /// </summary>
     public sealed class AbpKernelModule : AbpModule
     {
        private AuditingInterceptorRegistrar _auditingInterceptorRegistrar;

        public override void PreInitialize<TTenantId, TUserId>()
        {
            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());

            ValidationInterceptorRegistrar.Initialize(IocManager);

            //TODO: Consider to change order of Uow and Auth interceptors..?
            UnitOfWorkRegistrar.Initialize<TTenantId, TUserId>(IocManager);
            AuthorizationInterceptorRegistrar<TTenantId, TUserId>.Initialize(IocManager);

            _auditingInterceptorRegistrar = new AuditingInterceptorRegistrar(IocManager.Resolve<IAuditingConfiguration>(), IocManager);
            _auditingInterceptorRegistrar.Initialize<TTenantId, TUserId>();

            Configuration.Auditing.Selectors.Add(
                new NamedTypeSelector(
                    "Abp.ApplicationServices",
                    type => typeof (IApplicationService).IsAssignableFrom(type)
                    )
                );

            Configuration.Localization.Sources.Add(
                new DictionaryBasedLocalizationSource(
                    AbpConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        Assembly.GetExecutingAssembly(), "Abp.Localization.Sources.AbpXmlSource"
                        )));

            Configuration.Settings.Providers.Add<EmailSettingProvider>();

            Configuration.UnitOfWork.RegisterFilter(AbpDataFilters.SoftDelete, true);
            Configuration.UnitOfWork.RegisterFilter(AbpDataFilters.MustHaveTenant, true);
            Configuration.UnitOfWork.RegisterFilter(AbpDataFilters.MayHaveTenant, true);
        }

        public override void Initialize<TTenantId, TUserId>()
        {
            base.Initialize<TTenantId, TUserId>();

            IocManager.IocContainer.Install(new EventBusInstaller(IocManager));

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly(),
                new ConventionalRegistrationConfig
                {
                    InstallInstallers = false
                });
        }

        public override void PostInitialize<TTenantId, TUserId>()
        {
            RegisterMissingComponents<TTenantId, TUserId>();
           
            //modify by carl
            IocManager.RegisterIfNot<IUnitOfWork<TTenantId, TUserId>, NullUnitOfWork<TTenantId, TUserId>>(DependencyLifeStyle.Transient);
            IocManager.Resolve<PermissionManager<TTenantId, TUserId>>().Initialize();
            //end

            IocManager.Resolve<LocalizationManager>().Initialize();
            IocManager.Resolve<NavigationManager>().Initialize();
            IocManager.Resolve<SettingDefinitionManager>().Initialize();
        }

        private void RegisterMissingComponents<TTenantId, TUserId>()
            where TTenantId : struct
            where TUserId : struct
        {
            IocManager.RegisterIfNot<IAuditInfoProvider, NullAuditInfoProvider>(DependencyLifeStyle.Transient);
            IocManager.RegisterIfNot<IAuditingStore<TTenantId, TUserId>, SimpleLogAuditingStore<TTenantId, TUserId>>(DependencyLifeStyle.Transient);
        }
    }
}