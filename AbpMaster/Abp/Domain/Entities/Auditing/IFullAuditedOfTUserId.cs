namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     This interface ads <see cref="IDeletionAudited{TUserId}" /> to <see cref="IAudited{TUserId}" /> for a fully audited
    ///     entity.
    /// </summary>
    public interface IFullAudited<TUserId> : IAudited<TUserId>, IDeletionAudited<TUserId> where TUserId : struct
    {
    }
}