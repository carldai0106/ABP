using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.Role;

namespace CMS.Domain.UserRole
{
    [Table("UserRoles")]
    public class UserRoleEntity : AuditedEntity<Guid, Guid>
    {
        public virtual Guid UserId { get; set; }
        public virtual Guid RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }
        public virtual bool Status { get; set; }
    }
}