using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     This interface is implemented by entities which wanted to store deletion information (who and when deleted).
    /// </summary>
    public interface IDeletionAudited<TUserId> : ISoftDelete where TUserId : struct
    {
        /// <summary>
        ///     Which user deleted this entity?
        /// </summary>
        TUserId? DeleterUserId { get; set; }

        /// <summary>
        ///     Deletion time of this entity.
        /// </summary>
        DateTime? DeletionTime { get; set; }
    }
}