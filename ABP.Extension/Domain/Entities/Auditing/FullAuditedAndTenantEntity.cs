using System;

namespace Abp.Domain.Entities.Auditing
{
    [Serializable]
    public abstract class FullAuditedAndTenantEntity : FullAuditedEntity<Guid, Guid>, IMustHaveTenant<Guid>
    {
        public Guid TenantId { get; set; }
    }
}