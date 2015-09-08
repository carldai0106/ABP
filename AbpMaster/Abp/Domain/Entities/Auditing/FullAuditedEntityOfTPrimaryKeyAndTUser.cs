using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     Implements <see cref="IFullAudited{TUser}" /> to be a base class for full-audited entities.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    /// <typeparam name="TUserId">Type of the Creator or Modifier Deleter UserId</typeparam>
    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey, TUserId, TUser> : AuditedEntity<TPrimaryKey, TUserId, TUser>,
        IFullAudited<TUserId, TUser>
        where TUserId : struct
        where TUser : IEntity<TUserId>
    {
        /// <summary>
        ///     Is this entity Deleted?
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        ///     Reference to the deleter user of this entity.
        /// </summary>
        [ForeignKey("DeleterUserId")]
        public virtual TUser DeleterUser { get; set; }

        /// <summary>
        ///     Which user deleted this entity?
        /// </summary>
        public virtual TUserId? DeleterUserId { get; set; }

        /// <summary>
        ///     Deletion time of this entity.
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }
    }
}