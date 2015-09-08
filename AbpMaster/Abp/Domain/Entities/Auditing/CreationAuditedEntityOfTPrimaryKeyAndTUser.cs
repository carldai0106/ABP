using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Abp.Domain.Entities.Auditing
{
    /// <summary>
    ///     This class can be used to simplify implementing <see cref="ICreationAudited{TUser}" />.
    /// </summary>
    /// <typeparam name="TPrimaryKey">Type of the primary key of the entity</typeparam>
    /// <typeparam name="TUser">Type of the user</typeparam>
    /// <typeparam name="TUserId">Type of the Creator UserId</typeparam>
    [Serializable]
    public abstract class CreationAuditedEntity<TPrimaryKey, TUserId, TUser> :
        CreationAuditedEntity<TPrimaryKey, TUserId>,
        ICreationAudited<TUserId, TUser>
        where TUserId : struct
        where TUser : IEntity<TUserId>
    {
        /// <summary>
        ///     Reference to the creator user of this entity.
        /// </summary>
        [ForeignKey("CreatorUserId")]
        public virtual TUser CreatorUser { get; set; }
    }
}