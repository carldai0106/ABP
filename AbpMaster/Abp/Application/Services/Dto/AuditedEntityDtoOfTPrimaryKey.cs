using System;
using Abp.Domain.Entities.Auditing;

namespace Abp.Application.Services.Dto
{
    /// <summary>
    ///     This class can be inherited for simple Dto objects those are used for entities implement
    ///     <see cref="IAudited{TUserId}" /> interface.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of primary key</typeparam>
    /// <typeparam name="TUserId"></typeparam>
    [Serializable]
    public abstract class AuditedEntityDto<TPrimaryKey, TUserId> : CreationAuditedEntityDto<TPrimaryKey, TUserId>,
        IAudited<TUserId>
        where TUserId : struct
    {
        /// <summary>
        ///     Last modification date of this entity.
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        /// <summary>
        ///     Last modifier user of this entity.
        /// </summary>
        public TUserId? LastModifierUserId { get; set; }
    }
}