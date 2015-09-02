using System.Data.Entity;
using Abp.Domain.Uow;

namespace Abp.EntityFramework.Uow
{
    /// <summary>
    /// Implements <see cref="IDbContextProvider{TDbContext,TTenantId, TUserId}"/> that gets DbContext from
    /// active unit of work.
    /// </summary>
    /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
    /// <typeparam name="TTenantId"></typeparam>
    /// <typeparam name="TUserId"></typeparam>
    public class UnitOfWorkDbContextProvider<TDbContext, TTenantId, TUserId> : IDbContextProvider<TDbContext, TTenantId, TUserId>
        where TDbContext : DbContext
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Gets the DbContext.
        /// </summary>
        public TDbContext DbContext
        {
            get { return _currentUnitOfWorkProvider.Current.GetDbContext<TDbContext, TTenantId, TUserId>(); }
        }
       

        private readonly ICurrentUnitOfWorkProvider<TTenantId, TUserId> _currentUnitOfWorkProvider;

        /// <summary>
        /// Creates a new <see cref="UnitOfWorkDbContextProvider{TDbContext,TTenantId, TUserId}"/>.
        /// </summary>
        /// <param name="currentUnitOfWorkProvider"></param>
        public UnitOfWorkDbContextProvider(ICurrentUnitOfWorkProvider<TTenantId, TUserId> currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }
    }
}