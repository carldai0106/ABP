using System;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     This interface is implemented by entities that is wanted to store modification information (who and when modified
    ///     lastly).
    ///     Properties are automatically set when updating the <see cref="IEntity" />.
    /// </summary>
    public interface IModificationAudited<TUserId> where TUserId : struct
    {
        /// <summary>
        ///     The last time of modification.
        /// </summary>
        DateTime? LastModificationTime { get; set; }

        /// <summary>
        ///     Last modifier user for this entity.
        /// </summary>
        TUserId? LastModifierUserId { get; set; }
    }
}