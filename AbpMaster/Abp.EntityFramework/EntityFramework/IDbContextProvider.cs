using System.Data.Entity;

namespace Abp.EntityFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    public interface IDbContextProvider<out TDbContext, TTenantId, TUserId>
        where TDbContext : DbContext
    {
        TDbContext DbContext { get; }
    }
}