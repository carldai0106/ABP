using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace CMS.FW.Domain
{
    [Serializable]
    public abstract class FullAuditedAndTenantEntity<TPrimaryKey, TUserId, TTenantId> :
        FullAuditedEntity<TPrimaryKey, TUserId>, IMustHaveTenant<TTenantId>
        where TUserId : struct
        where TTenantId : struct
    {
        public TTenantId TenantId { get; set; }
    }
}