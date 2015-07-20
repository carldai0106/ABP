using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace CMS.Data.EntityFramework.Repositories
{
    public class Reponsitory<TDbContext, TEntity, TPrimaryKey, TTenantId, TUserId> : EfRepositoryBase<TDbContext, TEntity, TPrimaryKey, TTenantId, TUserId>
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
        where TTenantId : struct
        where TUserId : struct
    {
        protected Reponsitory(IDbContextProvider<TDbContext, TTenantId, TUserId> dbContextProvider)
            : base(dbContextProvider)
        {

        }
    }
}
