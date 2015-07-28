namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// Adds navigation properties to <see cref="IModificationAudited{TUserId}"/> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    /// <typeparam name="TUserId">Type of the LastModifierUserId</typeparam>
    public interface IModificationAudited<TUserId, TUser> : IModificationAudited<TUserId>
        where TUserId : struct
        where TUser : IEntity<TUserId>
    {
        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// </summary>
        TUser LastModifierUser { get; set; }
    }
}