namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities that is wanted to store creation information (who and when created).
    /// Creation time and creator user are automatically set when saving <see cref="Entity"/> to database.
    /// </summary>
    public interface ICreationAudited<TUserId> : IHasCreationTime where TUserId : struct
    {
        /// <summary>
        /// Id of the creator user of this entity.
        /// </summary>
        TUserId? CreatorUserId { get; set; }
    }
}