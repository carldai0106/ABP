using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// Implements <see cref="IFullAudited{TUserId}"/> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserId">Type of the Creator or Modifier User Id</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey, TUserId> : AuditedEntity<TPrimaryKey, TUserId>, IFullAudited<TUserId>
        where TUserId : struct
    {
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual bool IsDeleted { get; set; }
        
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        public virtual TUserId? DeleterUserId { get; set; }
        
        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
}
