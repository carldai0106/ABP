using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.Role;
using CMS.Domain.User;

namespace CMS.Domain.UserRole
{
    [Table("UserRoles")]
    public class UserRoleEntity : AuditedEntity<Guid, Guid>
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public RoleEntity Role { get; set; }
        public virtual bool Status { get; set; }
    }
}
