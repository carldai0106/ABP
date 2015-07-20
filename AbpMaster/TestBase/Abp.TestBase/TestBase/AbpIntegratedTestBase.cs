﻿using System;
using Abp.Collections;
using Abp.Dependency;
using Abp.Modules;
using Abp.Runtime.Session;
using Abp.TestBase.Modules;
using Abp.TestBase.Runtime.Session;

namespace Abp.TestBase
{
    /// <summary>
    /// This is the base class for all tests integrated to ABP.
    /// </summary>
    public abstract class AbpIntegratedTestBase<TTenantId, TUserId> : IDisposable
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// A reference to the <see cref="IIocManager"/> used for this test.
        /// </summary>
        protected IIocManager LocalIocManager { get; private set; }

        /// <summary>
        /// Gets Session object. Can be used to change current user and tenant in tests.
        /// </summary>
        protected TestAbpSession<TTenantId, TUserId> AbpSession { get; private set; }

        private readonly AbpBootstrapper _bootstrapper;

        protected AbpIntegratedTestBase()
        {
            LocalIocManager = new IocManager();

            LocalIocManager.Register<IModuleFinder, TestModuleFinder>();
            LocalIocManager.Register<IAbpSession<TTenantId, TUserId>, TestAbpSession<TTenantId, TUserId>>();

            AddModules(LocalIocManager.Resolve<TestModuleFinder>().Modules);

            PreInitialize();

            _bootstrapper = new AbpBootstrapper(LocalIocManager);
            _bootstrapper.Initialize<TTenantId, TUserId>();

            AbpSession = LocalIocManager.Resolve<TestAbpSession<TTenantId, TUserId>>();
        }

        protected virtual void AddModules(ITypeList<AbpModule> modules)
        {
            modules.Add<TestBaseModule>();
        }

        /// <summary>
        /// This method can be overrided to replace some services with fakes.
        /// </summary>
        protected virtual void PreInitialize()
        {

        }

        public virtual void Dispose()
        {
            _bootstrapper.Dispose();
            LocalIocManager.Dispose();
        }

        /// <summary>
        /// A shortcut to resolve an object from <see cref="LocalIocManager"/>.
        /// Also registers <see cref="T"/> as transient if it's not registered before.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <returns>The object instance</returns>
        protected T Resolve<T>()
        {
            EnsureClassRegistered(typeof(T));
            return LocalIocManager.Resolve<T>();
        }

        /// <summary>
        /// A shortcut to resolve an object from <see cref="LocalIocManager"/>.
        /// Also registers <see cref="T"/> as transient if it's not registered before.
        /// </summary>
        /// <typeparam name="T">Type of the object to get</typeparam>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>The object instance</returns>
        protected T Resolve<T>(object argumentsAsAnonymousType)
        {
            EnsureClassRegistered(typeof(T));
            return LocalIocManager.Resolve<T>(argumentsAsAnonymousType);
        }

        /// <summary>
        /// A shortcut to resolve an object from <see cref="LocalIocManager"/>.
        /// Also registers <see cref="type"/> as transient if it's not registered before.
        /// </summary>
        /// <param name="type">Type of the object to get</param>
        /// <returns>The object instance</returns>
        protected object Resolve(Type type)
        {
            EnsureClassRegistered(type);
            return LocalIocManager.Resolve(type);
        }

        /// <summary>
        /// A shortcut to resolve an object from <see cref="LocalIocManager"/>.
        /// Also registers <see cref="type"/> as transient if it's not registered before.
        /// </summary>
        /// <param name="type">Type of the object to get</param>
        /// <param name="argumentsAsAnonymousType">Constructor arguments</param>
        /// <returns>The object instance</returns>
        protected object Resolve(Type type, object argumentsAsAnonymousType)
        {
            EnsureClassRegistered(type);
            return LocalIocManager.Resolve(type, argumentsAsAnonymousType);
        }

        /// <summary>
        /// Registers given type if it's not registered before.
        /// </summary>
        /// <param name="type">Type to check and register</param>
        /// <param name="lifeStyle">Lifestyle</param>
        protected void EnsureClassRegistered(Type type, DependencyLifeStyle lifeStyle = DependencyLifeStyle.Transient)
        {
            if (!LocalIocManager.IsRegistered(type))
            {
                if (!type.IsClass || type.IsAbstract)
                {
                    throw new AbpException("Can not register " + type.Name + ". It should be a non-abstract class. If not, it should be registered before.");
                }

                LocalIocManager.Register(type, lifeStyle);
            }
        }
    }
}
