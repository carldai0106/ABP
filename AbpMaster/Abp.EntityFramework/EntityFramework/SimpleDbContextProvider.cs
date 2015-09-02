using System.Data.Entity;

namespace Abp.EntityFramework
{
    public sealed class SimpleDbContextProvider<TDbContext, TTenantId, TUserId> : IDbContextProvider<TDbContext, TTenantId, TUserId>
        where TDbContext : DbContext
        where TTenantId : struct
        where TUserId : struct
    {
      
        public TDbContext DbContext { get; private set; }
      
        public SimpleDbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
        }       
    }
}