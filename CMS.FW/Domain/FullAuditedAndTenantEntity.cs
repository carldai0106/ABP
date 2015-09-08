using System;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace CMS.FW.Domain
{
    [Serializable]
    public abstract class FullAuditedAndTenantEntity : FullAuditedEntity<Guid, Guid>, IMustHaveTenant<Guid>
    {
        public Guid TenantId { get; set; }
    }
}