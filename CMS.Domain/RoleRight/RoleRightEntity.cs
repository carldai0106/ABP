using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.ActionModule;

namespace CMS.Domain.RoleRight
{
    [Table("RoleRights")]
    public class RoleRightEntity : AuditedEntity<Guid, Guid>
    {
        public virtual Guid RoleId { get; set; }
        public virtual Guid ActionModuleId { get; set; }
        public virtual bool Status { get; set; }
        public virtual ActionModuleEntity ActionModule { get; set; }
    }
}