namespace Abp.Domain.Uow
{
    /// <summary>
    ///     Defines a unit of work.
    ///     This interface is internally used by ABP.
    ///     Use <see cref="IUnitOfWorkManager{TTenantId, TUserId}.Begin()" /> to start a new unit of work.
    /// </summary>
    public interface IUnitOfWork<TTenantId, TUserId> : IActiveUnitOfWork, IUnitOfWorkCompleteHandle
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        ///     Unique id of this UOW.
        /// </summary>
        string Id { get; }

        /// <summary>
        ///     Reference to the outer UOW if exists.
        /// </summary>
        IUnitOfWork<TTenantId, TUserId> Outer { get; set; }

        /// <summary>
        ///     Begins the unit of work with given options.
        /// </summary>
        /// <param name="options">Unit of work options</param>
        void Begin(UnitOfWorkOptions options);
    }
}