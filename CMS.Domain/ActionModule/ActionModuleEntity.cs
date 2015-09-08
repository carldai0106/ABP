using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.Action;
using CMS.Domain.RoleRight;

namespace CMS.Domain.ActionModule
{
    [Table("ActionModules")]
    public class ActionModuleEntity : AuditedEntity<Guid, Guid>
    {
        public virtual Guid ActionId { get; set; }
        public virtual Guid ModuleId { get; set; }
        public virtual bool Status { get; set; }
        public virtual ActionEntity Action { get; set; }

        [ForeignKey("ActionModuleId")]
        public virtual ICollection<RoleRightEntity> RoleRights { get; set; }
    }
}