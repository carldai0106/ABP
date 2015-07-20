using System;
using Abp.Domain.Entities.Auditing;
using Abp.Timing;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    /// This class can be inherited for simple Dto objects those are used for entities implement <see cref="ICreationAudited{TUserId}"/> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    /// <typeparam name="TUserId">Type of the Creator UserId</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntityDto<TPrimaryKey, TUserId> : EntityDto<TPrimaryKey>, ICreationAudited<TUserId>
        where TUserId :struct
    {
        /// <summary>
        /// Creation date of this entity.
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator user's id for this entity.
        /// </summary>
        public TUserId? CreatorUserId { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected CreationAuditedEntityDto()
        {
            CreationTime = Clock.Now;
        }
    }
}