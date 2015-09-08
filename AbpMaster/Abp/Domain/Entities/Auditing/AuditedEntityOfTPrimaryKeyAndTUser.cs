using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     This class can be used to simplify implementing <see cref="IAudited{TUser}" />.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    /// <typeparam name="TUserId">Type of the Creator or Modifier UserId</typeparam>
    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey, TUserId, TUser> : AuditedEntity<TPrimaryKey, TUserId>,
        IAudited<TUserId, TUser>
        where TUserId : struct
        where TUser : IEntity<TUserId>
    {
        /// <summary>
        ///     Reference to the creator user of this entity.
        /// </summary>
        [ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }

        /// <summary>
        ///     Reference to the last modifier user of this entity.
        /// </summary>
        [ForeignKey("LastModifierUserId")]
        public virtual TUser LastModifierUser { get; set; }
    }
}