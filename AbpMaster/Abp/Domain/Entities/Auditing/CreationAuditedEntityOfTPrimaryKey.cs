using System;
using Abp.Timing;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    /// This class can be used to simplify implementing <see cref="ICreationAudited{TUserId}"/>.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUserId"></typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey, TUserId> : Entity<TPrimaryKey>, ICreationAudited<TUserId> where TUserId : struct
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        public virtual DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator of this entity.
        /// </summary>
        public virtual TUserId? CreatorUserId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreationAuditedEntity()
        {
            CreationTime = Clock.Now;
        }
    }
}