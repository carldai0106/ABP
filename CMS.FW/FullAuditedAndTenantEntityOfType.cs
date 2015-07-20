using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace CMS.FW
{
    public abstract class FullAuditedAndTenantEntity<TPrimaryKey, TUserId, TTenantId> : 
        FullAuditedEntity<TPrimaryKey, TUserId>, IMustHaveTenant<TTenantId>
        where TUserId :struct
        where TTenantId : struct
    {
        public TTenantId TenantId { get; set; }
    }
}
