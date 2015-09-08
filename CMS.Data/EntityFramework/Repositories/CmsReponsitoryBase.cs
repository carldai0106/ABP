using System.Data.Entity;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace CMS.Data.EntityFramework.Repositories
{
    public abstract class CmsReponsitoryBase<TDbContext, TEntity, TPrimaryKey, TTenantId, TUserId> :
        EfRepositoryBase<TDbContext, TEntity, TPrimaryKey, TTenantId, TUserId>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
        where TTenantId : struct
        where TUserId : struct
    {
        protected CmsReponsitoryBase(IDbContextProvider<TDbContext, TTenantId, TUserId> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}