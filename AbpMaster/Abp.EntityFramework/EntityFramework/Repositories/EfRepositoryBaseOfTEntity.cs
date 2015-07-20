using System.Data.Entity;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.EntityFramework.Repositories
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TTenantId"></typeparam>
    /// <typeparam name="TUserId"></typeparam>
    public class EfRepositoryBase<TDbContext, TEntity, TTenantId, TUserId> : EfRepositoryBase<TDbContext, TEntity, int, TTenantId, TUserId>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TDbContext : DbContext
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public EfRepositoryBase(IDbContextProvider<TDbContext, TTenantId, TUserId> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}