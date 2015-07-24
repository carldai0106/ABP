using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
