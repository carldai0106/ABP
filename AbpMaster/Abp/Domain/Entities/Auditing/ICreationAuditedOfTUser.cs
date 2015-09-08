namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     Adds navigation properties to <see cref="ICreationAudited{TCreatorUserId}" /> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    /// <typeparam name="TUserId">Type of the CreatorUserId</typeparam>
    public interface ICreationAudited<TUserId, TUser> : ICreationAudited<TUserId>
        where TUserId : struct
        where TUser : IEntity<TUserId>
    {
        /// <summary>
        ///     Reference to the creator user of this entity.
        /// </summary>
        TUser CreatorUser { get; set; }
    }
}