using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace CMS.FW
{
    public abstract class FullAuditedAndTenantEntity : FullAuditedEntity<Guid, Guid>, IMustHaveTenant<Guid>
    {
        public Guid TenantId { get; set; }
    }
}
