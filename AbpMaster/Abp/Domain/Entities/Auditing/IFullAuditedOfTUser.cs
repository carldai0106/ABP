namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     Adds navigation properties to <see cref="IFullAudited{TUserId}" /> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    /// <typeparam name="TUserId">Type of </typeparam>
    public interface IFullAudited<TUserId, TUser> : IFullAudited<TUserId>, IDeletionAudited<TUserId, TUser>
        where TUserId : struct
        where TUser : IEntity<TUserId>
    {
    }
}