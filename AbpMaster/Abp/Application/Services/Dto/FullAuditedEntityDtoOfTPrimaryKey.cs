using System;
using Abp.Domain.Entities.Auditing;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    ///     This class can be inherited for simple Dto objects those are used for entities implement
    ///     <see cref="IFullAudited{TUserId}" /> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    /// <typeparam name="TUserId"></typeparam>
    [Serializable]
    public abstract class FullAuditedEntityDto<TPrimaryKey, TUserId> : AuditedEntityDto<TPrimaryKey, TUserId>,
        IFullAudited<TUserId>
        where TUserId : struct
    {
        /// <summary>
        ///     Is this entity deleted?
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     Deleter user's Id, if this entity is deleted,
        /// </summary>
        public TUserId? DeleterUserId { get; set; }

        /// <summary>
        ///     Deletion time, if this entity is deleted,
        /// </summary>
        public DateTime? DeletionTime { get; set; }
    }
}