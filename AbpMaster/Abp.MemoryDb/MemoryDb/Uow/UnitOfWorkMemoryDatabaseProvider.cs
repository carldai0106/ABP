using Abp.Dependency;
using Abp.Domain.Uow;

namespace Abp.MemoryDb.Uow
{
    /// <summary>
    /// Implements <see cref="IMemoryDatabaseProvider{TTenantId, TUserId}"/> that gets database from active unit of work.
    /// </summary>
    public class UnitOfWorkMemoryDatabaseProvider<TTenantId, TUserId> : IMemoryDatabaseProvider<TTenantId, TUserId>, ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        public MemoryDatabase Database { get { return ((MemoryDbUnitOfWork<TTenantId, TUserId>)_currentUnitOfWork.Current).Database; } }

        private readonly ICurrentUnitOfWorkProvider<TTenantId, TUserId> _currentUnitOfWork;

        public UnitOfWorkMemoryDatabaseProvider(ICurrentUnitOfWorkProvider<TTenantId, TUserId> currentUnitOfWork)
        {
            _currentUnitOfWork = currentUnitOfWork;
        }
    }
}