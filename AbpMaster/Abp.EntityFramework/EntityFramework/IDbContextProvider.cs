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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="TTenantId"></typeparam>
        ///// <typeparam name="TUserId"></typeparam>
        ///// <returns></returns>
        //TDbContext GetDbContext<TTenantId, TUserId>()
        //    where TTenantId : struct
        //    where TUserId : struct;
    }
}