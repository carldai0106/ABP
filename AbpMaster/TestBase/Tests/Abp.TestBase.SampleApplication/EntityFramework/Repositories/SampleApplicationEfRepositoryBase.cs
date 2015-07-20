using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Abp.TestBase.SampleApplication.EntityFramework.Repositories
{
    public class SampleApplicationEfRepositoryBase<TEntity, TTenantId, TUserId> : SampleApplicationEfRepositoryBase<TEntity, int, TTenantId, TUserId>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TTenantId : struct
        where TUserId : struct
    {
        public SampleApplicationEfRepositoryBase(IDbContextProvider<SampleApplicationDbContext<TTenantId, TUserId>, TTenantId, TUserId> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }

    public class SampleApplicationEfRepositoryBase<TEntity, TPrimaryKey, TTenantId, TUserId> : EfRepositoryBase<SampleApplicationDbContext<TTenantId, TUserId>, TEntity, TPrimaryKey, TTenantId, TUserId>
        where TEntity : class, IEntity<TPrimaryKey>
        where TTenantId : struct
        where TUserId : struct
    {
        public SampleApplicationEfRepositoryBase(IDbContextProvider<SampleApplicationDbContext<TTenantId, TUserId>, TTenantId, TUserId> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}