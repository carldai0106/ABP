using Abp.Dependency;
using Abp.Domain.Uow;
using MongoDB.Driver;

namespace Abp.MongoDb.Uow
{
    /// <summary>
    /// Implements <see cref="IMongoDatabaseProvider"/> that gets database from active unit of work.
    /// </summary>
    public class UnitOfWorkMongoDatabaseProvider<TTenantId, TUserId> : IMongoDatabaseProvider, ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        public MongoDatabase Database { get { return ((MongoDbUnitOfWork<TTenantId, TUserId>)_currentUnitOfWork.Current).Database; } }

        private readonly ICurrentUnitOfWorkProvider<TTenantId, TUserId> _currentUnitOfWork;

        public UnitOfWorkMongoDatabaseProvider(ICurrentUnitOfWorkProvider<TTenantId, TUserId> currentUnitOfWork)
        {
            _currentUnitOfWork = currentUnitOfWork;
        }
    }
}