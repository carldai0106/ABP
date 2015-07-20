using System;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFramework.Extensions;
using Abp.Reflection;
using Abp.Reflection.Extensions;

namespace Abp.EntityFramework.Repositories
{
    internal static class EntityFrameworkGenericRepositoryRegistrar
    {
        //modify by carl
        public static void RegisterGenericRepositories<TTenantId, TUserId>(ITypeFinder typeFinder, IIocManager iocManager)
            where TTenantId : struct
            where TUserId : struct
        {
            var entities = typeFinder.Find(
                x => (
                    typeof(IEntity).IsAssignableFrom(x) ||
                    x.IsInheritsOrImplements(typeof(IEntity<>))
                    )
                );

            if (entities.IsNullOrEmpty())
            {
                throw new AbpException("No class found derived from Entity or Entity<>.");
            }

            var dbContextTypes =
               typeFinder.Find(type =>
                   type.IsPublic &&
                   !type.IsAbstract &&
                   type.IsClass && type.IsInheritsOrImplements(typeof(AbpDbContext<,>)));
            if (dbContextTypes.IsNullOrEmpty())
            {
                throw new AbpException("No class found derived from AbpDbContext.");
            }

            foreach (var dbContextType in dbContextTypes)
            {
                RegisterGenericRepositories<TTenantId, TUserId>(dbContextType, entities, iocManager);
            }
        }


        public static void RegisterGenericRepositories<TTenantId, TUserId>(Type dbContextType, Type[] entities,
            IIocManager iocManager)
            where TTenantId : struct
            where TUserId : struct
        {
            foreach (var entityType in entities)
            {
                var interfaces = entityType.GetInterfaces();
                foreach (var interfaceType in interfaces)
                {
                    if (interfaceType.IsGenericType &&
                        interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                    {
                        var primaryKeyType = interfaceType.GenericTypeArguments[0];
                        if (primaryKeyType.Name == "TPrimaryKey" || string.IsNullOrEmpty(primaryKeyType.FullName))
                        {
                            continue;
                        }

                        if (primaryKeyType == typeof(int))
                        {
                            var genericRepositoryType = typeof(IRepository<>).MakeGenericType(entityType);
                            if (!iocManager.IsRegistered(genericRepositoryType))
                            {
                                var makedGenericType = typeof(EfRepositoryBase<,,,>).MakeGenericType(dbContextType, entityType, typeof(TTenantId), typeof(TUserId));
                                if (string.IsNullOrEmpty(makedGenericType.FullName))
                                {
                                    dbContextType = dbContextType.MakeGenericType(typeof(TTenantId), typeof(TUserId));
                                    if (!iocManager.IsRegistered(dbContextType))
                                    {
                                        iocManager.Register(dbContextType, DependencyLifeStyle.Transient);
                                    }
                                    makedGenericType = typeof(EfRepositoryBase<,,,>).MakeGenericType(dbContextType, entityType, typeof(TTenantId), typeof(TUserId));
                                }

                                iocManager.Register(
                                    genericRepositoryType,
                                    makedGenericType,
                                    DependencyLifeStyle.Transient
                                    );
                            }
                        }

                        var genericRepositoryTypeWithPrimaryKey = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                        if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                        {
                            var makedGenericType = typeof(EfRepositoryBase<,,,,>).MakeGenericType(dbContextType, entityType, primaryKeyType, typeof(TTenantId), typeof(TUserId));
                            if (string.IsNullOrEmpty(makedGenericType.FullName))
                            {
                                dbContextType = dbContextType.MakeGenericType(typeof(TTenantId), typeof(TUserId));
                                if (!iocManager.IsRegistered(dbContextType))
                                {
                                    iocManager.Register(dbContextType, DependencyLifeStyle.Transient);
                                }
                                makedGenericType = typeof(EfRepositoryBase<,,,,>).MakeGenericType(dbContextType, entityType, primaryKeyType, typeof(TTenantId), typeof(TUserId));
                            }

                            iocManager.Register(
                                genericRepositoryTypeWithPrimaryKey,
                                makedGenericType,
                                DependencyLifeStyle.Transient
                                );
                        }
                    }
                }
            }
        }
       

        //modify by carl
        public static void RegisterForDbContext<TTenantId, TUserId>(Type dbContextType, IIocManager iocManager)
            where TTenantId : struct
            where TUserId : struct
        {

            var entities = dbContextType.GetEntityTypes();
            foreach (var entityType in entities)
            {
                var interfaces = entityType.GetInterfaces();
                foreach (var interfaceType in interfaces)
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEntity<>))
                    {
                        var primaryKeyType = interfaceType.GenericTypeArguments[0];
                        if (primaryKeyType.Name == "TPrimaryKey" || string.IsNullOrEmpty(primaryKeyType.FullName))
                        {
                            continue;
                        }

                        if (primaryKeyType == typeof(int))
                        {
                            var genericRepositoryType = typeof(IRepository<>).MakeGenericType(entityType);
                            if (!iocManager.IsRegistered(genericRepositoryType))
                            {
                                var makedGenericType = typeof(EfRepositoryBase<,,,>).MakeGenericType(dbContextType, entityType, typeof(TTenantId), typeof(TUserId));
                                if (string.IsNullOrEmpty(makedGenericType.FullName))
                                {
                                    dbContextType = dbContextType.MakeGenericType(typeof(TTenantId), typeof(TUserId));
                                    if (!iocManager.IsRegistered(dbContextType))
                                    {
                                        iocManager.Register(dbContextType, DependencyLifeStyle.Transient);
                                    }
                                    makedGenericType = typeof(EfRepositoryBase<,,,>).MakeGenericType(dbContextType, entityType, typeof(TTenantId), typeof(TUserId));
                                }

                                iocManager.Register(
                                    genericRepositoryType,
                                    makedGenericType,
                                    DependencyLifeStyle.Transient
                                    );
                            }
                        }

                        var genericRepositoryTypeWithPrimaryKey = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                        if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                        {
                            var makedGenericType = typeof(EfRepositoryBase<,,,,>).MakeGenericType(dbContextType, entityType, primaryKeyType, typeof(TTenantId), typeof(TUserId));
                            if (string.IsNullOrEmpty(makedGenericType.FullName))
                            {
                                dbContextType = dbContextType.MakeGenericType(typeof(TTenantId), typeof(TUserId));
                                if (!iocManager.IsRegistered(dbContextType))
                                {
                                    iocManager.Register(dbContextType, DependencyLifeStyle.Transient);
                                }
                                makedGenericType = typeof(EfRepositoryBase<,,,,>).MakeGenericType(dbContextType, entityType, primaryKeyType, typeof(TTenantId), typeof(TUserId));
                            }

                            iocManager.Register(
                                genericRepositoryTypeWithPrimaryKey,
                                makedGenericType,
                                DependencyLifeStyle.Transient
                                );
                        }
                    }
                }
            }
        }
    }
}