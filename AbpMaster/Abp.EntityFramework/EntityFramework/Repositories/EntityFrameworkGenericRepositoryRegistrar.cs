using System;
using System.Collections.Generic;
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
                    ) && !x.FullName.Contains("Abp.Domain.Entities")
                );

            if (entities.IsNullOrEmpty())
            {
                throw new AbpException("No class found derived from IEntity or IEntity<>.");
            }

            var dbContextTypes =
               typeFinder.Find(type =>
                   type.IsPublic &&
                   !type.IsAbstract &&
                   type.IsClass && type.IsInheritsOrImplements(typeof(AbpDbContext<,>)));
            if (dbContextTypes.IsNullOrEmpty())
            {
                throw new AbpException("No class found derived from AbpDbContext<,>.");
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
            var autoRepositoryAttr = dbContextType.GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>();

            if (autoRepositoryAttr == null)
            {
                autoRepositoryAttr = AutoRepositoryTypesAttribute.Default;
            }

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
                            var genericRepositoryType = autoRepositoryAttr.RepositoryInterface.MakeGenericType(entityType);
                            if (!iocManager.IsRegistered(genericRepositoryType))
                            {
                                var implType = GetMakedGenericType<TTenantId, TUserId>(false, iocManager, autoRepositoryAttr,
                                    dbContextType, entityType, primaryKeyType);

                                iocManager.Register(
                                    genericRepositoryType,
                                    implType,
                                    DependencyLifeStyle.Transient
                                    );
                            }
                        }

                        var genericRepositoryTypeWithPrimaryKey = autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey.MakeGenericType(entityType, primaryKeyType);
                        if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                        {
                            var implType = GetMakedGenericType<TTenantId, TUserId>(true, iocManager, autoRepositoryAttr,
                                   dbContextType, entityType, primaryKeyType);

                            iocManager.Register(
                                genericRepositoryTypeWithPrimaryKey,
                                implType,
                                DependencyLifeStyle.Transient
                                );
                        }
                    }
                }
            }
        }

        public static void RegisterForDbContext<TTenantId, TUserId>(Type dbContextType, IIocManager iocManager)
            where TTenantId : struct
            where TUserId : struct
        {
            var autoRepositoryAttr = dbContextType.GetSingleAttributeOrNull<AutoRepositoryTypesAttribute>();

            if (autoRepositoryAttr == null)
            {
                autoRepositoryAttr = AutoRepositoryTypesAttribute.Default;
            }

            var entities = dbContextType.GetEntityTypes();
            foreach (var entityType in entities)
            {
                foreach (var interfaceType in entityType.GetInterfaces())
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
                            var genericRepositoryType = autoRepositoryAttr.RepositoryInterface.MakeGenericType(entityType);
                            if (!iocManager.IsRegistered(genericRepositoryType))
                            {
                                var implType = GetMakedGenericType<TTenantId, TUserId>(false, iocManager, autoRepositoryAttr,
                                    dbContextType, entityType, primaryKeyType);

                                iocManager.Register(
                                    genericRepositoryType,
                                    implType,
                                    DependencyLifeStyle.Transient
                                    );
                            }
                        }

                        var genericRepositoryTypeWithPrimaryKey = autoRepositoryAttr.RepositoryInterfaceWithPrimaryKey.MakeGenericType(entityType, primaryKeyType);
                        if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                        {
                            var implType = GetMakedGenericType<TTenantId, TUserId>(true, iocManager, autoRepositoryAttr,
                                   dbContextType, entityType, primaryKeyType);

                            iocManager.Register(
                                genericRepositoryTypeWithPrimaryKey,
                                implType,
                                DependencyLifeStyle.Transient
                                );
                        }
                    }
                }
            }
        }

        private static Type GetMakedGenericType<TTenantId, TUserId>(bool hasPrimaryKey, IIocManager iocManager, 
            AutoRepositoryTypesAttribute autoRepositoryAttr, 
            Type dbContextType, Type entityType, Type primaryKeyType)
        {
            var genericArgs = hasPrimaryKey
                ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.GetGenericArguments()
                : autoRepositoryAttr.RepositoryImplementation.GetGenericArguments();

            var types = new List<Type>();
            foreach (var ga in genericArgs)
            {
                switch (ga.Name)
                {
                    case "TDbContext":
                        types.Add(dbContextType);
                        break;
                    case "TEntity":
                        types.Add(entityType);
                        break;
                    case "TPrimaryKey":
                        types.Add(primaryKeyType);
                        break;
                    case "TTenantId":
                        types.Add(typeof(TTenantId));
                        break;
                    case "TUserId":
                        types.Add(typeof(TUserId));
                        break;
                }
            }

            var implType = hasPrimaryKey
                ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(types.ToArray())
                : autoRepositoryAttr.RepositoryImplementation.MakeGenericType(types.ToArray());

            if (string.IsNullOrEmpty(implType.FullName))
            {
                var makedDbContextType = dbContextType.MakeGenericType(typeof(TTenantId), typeof(TUserId));

                if (!iocManager.IsRegistered(makedDbContextType))
                {
                    iocManager.Register(makedDbContextType, DependencyLifeStyle.Transient);
                }

                if (types[0] == dbContextType)
                {
                    types[0] = makedDbContextType;
                }
                else
                {
                    throw new InvalidOperationException("Type of TDbContext can not be substituted.");
                }

                implType = hasPrimaryKey
                    ? autoRepositoryAttr.RepositoryImplementationWithPrimaryKey.MakeGenericType(types.ToArray())
                    : autoRepositoryAttr.RepositoryImplementation.MakeGenericType(types.ToArray());
            }

            return implType;
        }
        
    }
}