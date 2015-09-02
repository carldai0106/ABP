using System;
using System.Collections.Generic;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Reflection;
using Abp.Reflection.Extensions;

namespace Abp.NHibernate.Repositories
{
    internal static class NhGenericRepositoryRegistrar
    {
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

            RegisterGenericRepositories<TTenantId, TUserId>(entities, iocManager);
        }


        public static void RegisterGenericRepositories<TTenantId, TUserId>(Type[] entities,
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
                                iocManager.Register(
                                    genericRepositoryType,
                                    typeof(NhRepositoryBase<,,>).MakeGenericType(entityType, typeof(TTenantId), typeof(TUserId)),
                                    DependencyLifeStyle.Transient
                                    );
                            }
                        }

                        var genericRepositoryTypeWithPrimaryKey = typeof(IRepository<,>).MakeGenericType(entityType, primaryKeyType);
                        if (!iocManager.IsRegistered(genericRepositoryTypeWithPrimaryKey))
                        {
                            iocManager.Register(
                                genericRepositoryTypeWithPrimaryKey,
                                typeof(NhRepositoryBase<,,,>).MakeGenericType(entityType, primaryKeyType, typeof(TTenantId), typeof(TUserId)),
                                DependencyLifeStyle.Transient
                                );
                        }
                    }
                }
            }
        }
    }
}