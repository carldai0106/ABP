namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     Adds navigation properties to <see cref="IAudited{TUserId}" /> interface for user.
    /// </summary>
    /// <typeparam name="TUser">Type of the user</typeparam>
    /// <typeparam name="TUserId">Type of the Creator or Modifier UserId</typeparam>
    public interface IAudited<TUserId, TUser> : IAudited<TUserId>, ICreationAudited<TUserId, TUser>,
        IModificationAudited<TUserId, TUser>
        where TUserId : struct
        where TUser : IEntity<TUserId>
    {
    }
}