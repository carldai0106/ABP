using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Abp.MongoDb.Repositories
{
    /// <summary>
    /// Implements IRepository for MongoDB.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TTenantId">Type of the tenant id</typeparam>
    /// <typeparam name="TUserId">Type of the user id</typeparam>
    public class MongoDbRepositoryBase<TEntity, TTenantId, TUserId> : MongoDbRepositoryBase<TEntity, int, TTenantId, TUserId>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TTenantId : struct
        where TUserId : struct
    {
        public MongoDbRepositoryBase(IMongoDatabaseProvider<TTenantId, TUserId> databaseProvider)
            : base(databaseProvider)
        {
        }
    }

    /// <summary>
    /// Implements IRepository for MongoDB.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    /// <typeparam name="TTenantId">Type of the tenant id</typeparam>
    /// <typeparam name="TUserId">Type of the user id</typeparam>
    public class MongoDbRepositoryBase<TEntity, TPrimaryKey, TTenantId, TUserId> : AbpRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
        where TTenantId : struct
        where TUserId : struct
    {
        private readonly IMongoDatabaseProvider<TTenantId, TUserId> _databaseProvider;

        protected MongoDatabase Database
        {
            get { return _databaseProvider.Database; }
        }

        protected MongoCollection<TEntity> Collection
        {
            get
            {
                return _databaseProvider.Database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
        }

        public MongoDbRepositoryBase(IMongoDatabaseProvider<TTenantId, TUserId> databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Collection.AsQueryable();
        }

        public override TEntity Get(TPrimaryKey id)
        {
            var query = MongoDB.Driver.Builders.Query<TEntity>.EQ(e => e.Id, id);
            return Collection.FindOne(query); //TODO: What if no entity with id?
        }

        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            var query = MongoDB.Driver.Builders.Query<TEntity>.EQ(e => e.Id, id);
            return Collection.FindOne(query); //TODO: What if no entity with id?
        }

        public override TEntity Insert(TEntity entity)
        {
            Collection.Insert(entity);
            return entity;
        }
        public override TEntity Update(TEntity entity)
        {
            Collection.Save(entity);
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(TPrimaryKey id)
        {
            var query = MongoDB.Driver.Builders.Query<TEntity>.EQ(e => e.Id, id);
            Collection.Remove(query);
        }
    }
}