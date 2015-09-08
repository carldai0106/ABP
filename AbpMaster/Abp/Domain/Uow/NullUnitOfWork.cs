using System.Threading.Tasks;

namespace Abp.Domain.Uow
{
    /// <summary>
    ///     Null implementation of unit of work.
    ///     It's used if no component registered for <see cref="IUnitOfWork" />.
    ///     This ensures working ABP without a database.
    /// </summary>
    public sealed class NullUnitOfWork<TTenantId, TUserId> : UnitOfWorkBase<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        public NullUnitOfWork(IUnitOfWorkDefaultOptions defaultOptions)
            : base(defaultOptions)
        {
        }

        public override void SaveChanges()
        {
        }

        public override async Task SaveChangesAsync()
        {
        }

        protected override void BeginUow()
        {
        }

        protected override void CompleteUow()
        {
        }

        protected override async Task CompleteUowAsync()
        {
        }

        protected override void DisposeUow()
        {
        }
    }
}