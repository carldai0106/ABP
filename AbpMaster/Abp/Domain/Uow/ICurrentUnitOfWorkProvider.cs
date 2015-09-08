namespace Abp.Domain.Uow
{
    /// <summary>
    ///     Used to get/set current <see cref="IUnitOfWork{TTenantId, TUserId}" />.
    /// </summary>
    public interface ICurrentUnitOfWorkProvider<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        ///     Gets/sets current <see cref="IUnitOfWork{TTenantId, TUserId}" />.
        /// </summary>
        IUnitOfWork<TTenantId, TUserId> Current { get; set; }
    }
}