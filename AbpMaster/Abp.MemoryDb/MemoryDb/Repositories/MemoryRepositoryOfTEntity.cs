using Abp.Domain.Entities;
using Abp.Domain.Repositories;

namespace Abp.MemoryDb.Repositories
{
    public class MemoryRepository<TEntity, TTenantId, TUserId> : MemoryRepository<TEntity, int, TTenantId, TUserId>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
        where TTenantId : struct
        where TUserId : struct
    {
        public MemoryRepository(IMemoryDatabaseProvider<TTenantId, TUserId> databaseProvider)
            : base(databaseProvider)
        {
        }
    }
}
