using System.Data.Entity;

namespace Abp.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <typeparam name="TTenantId"></typeparam>
    /// <typeparam name="TUserId"></typeparam>
    public sealed class SimpleDbContextProvider<TDbContext, TTenantId, TUserId> : IDbContextProvider<TDbContext, TTenantId, TUserId>
        where TDbContext : DbContext
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// 
        /// </summary>
        public TDbContext DbContext { get; private set; }

        private readonly TDbContext _dbContext;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public SimpleDbContextProvider(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //public TDbContext GetDbContext<TTenantId, TUserId>()
        //    where TTenantId : struct
        //    where TUserId : struct
        //{
        //    return _dbContext;
        //}
    }
}