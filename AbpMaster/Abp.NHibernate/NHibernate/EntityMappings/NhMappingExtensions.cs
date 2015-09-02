using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using FluentNHibernate.Mapping;

namespace Abp.NHibernate.EntityMappings
{
    /// <summary>
    /// This class is used to make mapping easier for standart columns.
    /// </summary>
    public static class NhMappingExtensions
    {
        /// <summary>
        /// Maps full audit columns (defined by <see cref="IFullAudited"/>).
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TUserId"></typeparam>
        public static void MapFullAudited<TEntity,TUserId>(this ClassMap<TEntity> mapping)
            where TEntity : IFullAudited<TUserId> where TUserId :struct
        {
            mapping.MapAudited<TEntity, TUserId>();
            mapping.MapDeletionAudited<TEntity, TUserId>();
        }

        /// <summary>
        /// Maps audit columns. See <see cref="IAudited"/>.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TUserId"></typeparam>
        public static void MapAudited<TEntity, TUserId>(this ClassMap<TEntity> mapping) 
            where TEntity :
            IAudited<TUserId> where TUserId : struct
        {
            mapping.MapCreationAudited<TEntity, TUserId>();
            mapping.MapModificationAudited<TEntity, TUserId>();
        }

        /// <summary>
        /// Maps creation audit columns. See <see cref="ICreationAudited"/>.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TUserId"></typeparam>
        public static void MapCreationAudited<TEntity, TUserId>(this ClassMap<TEntity> mapping)
            where TEntity : ICreationAudited<TUserId>
            where TUserId : struct
        {
            mapping.MapCreationTime();
            mapping.Map(x => x.CreatorUserId);
        }

        /// <summary>
        /// Maps CreationTime column. See <see cref="ICreationAudited"/>.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        public static void MapCreationTime<TEntity>(this ClassMap<TEntity> mapping)
            where TEntity : IHasCreationTime
        {
            mapping.Map(x => x.CreationTime);
        }

        /// <summary>
        /// Maps modification audit columns. See <see cref="ICreationAudited"/>.
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TUserId"></typeparam>
        public static void MapModificationAudited<TEntity, TUserId>(this ClassMap<TEntity> mapping)
            where TEntity : IModificationAudited<TUserId> where TUserId : struct
        {
            mapping.Map(x => x.LastModificationTime);
            mapping.Map(x => x.LastModifierUserId);
        }

        /// <summary>
        /// Maps deletion audit columns (defined by <see cref="IDeletionAudited"/>).
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <typeparam name="TUserId"></typeparam>
        public static void MapDeletionAudited<TEntity, TUserId>(this ClassMap<TEntity> mapping)
            where TEntity : IDeletionAudited<TUserId> where TUserId : struct
        {
            mapping.MapIsDeleted();
            mapping.Map(x => x.DeleterUserId);
            mapping.Map(x => x.DeletionTime);
        }

        /// <summary>
        /// Maps IsDeleted column (defined by <see cref="ISoftDelete"/>).
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        public static void MapIsDeleted<TEntity>(this ClassMap<TEntity> mapping) where TEntity : ISoftDelete
        {
            mapping.Map(x => x.IsDeleted);
        }
    }
}