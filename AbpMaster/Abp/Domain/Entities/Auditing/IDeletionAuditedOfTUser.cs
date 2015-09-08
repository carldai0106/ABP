namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     Adds navigation properties to <see cref="IDeletionAudited{TUserId}" /> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    /// <typeparam name="TUserId">Type of the DeleterUserId</typeparam>
    public interface IDeletionAudited<TUserId, TUser> : IDeletionAudited<TUserId> where TUserId : struct
        where TUser : IEntity<TUserId>
    {
        /// <summary>
        ///     Reference to the deleter user of this entity.
        /// </summary>
        TUser DeleterUser { get; set; }
    }
}