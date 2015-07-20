using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="IAudited{TUserId}"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserId"></typeparam>
    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey, TUserId> : CreationAuditedEntity<TPrimaryKey, TUserId>, IAudited<TUserId> where TUserId : struct
    {
        /// <summary>
        /// Last modification date of this entity.
        /// </summary>
        public virtual DateTime? LastModificationTime { get; set; }

        /// <summary>
        /// Last modifier user of this entity.
        /// </summary>
        public virtual TUserId? LastModifierUserId { get; set; }
    }
}