using System;

namespace Abp.Domain.Entities.Auditing
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