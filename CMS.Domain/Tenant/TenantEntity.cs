using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.User;

namespace CMS.Domain.Tenant
{
    [Table("Tenants")]
    [Serializable]
    public class TenantEntity : FullAuditedEntity<Guid, Guid>, IPassivable
    {
        public const int MaxTenancyNameLength = 128;
        public const int MaxDisplayNameLength = 256;

        public TenantEntity()
        {
            IsActive = true;
        }

        public TenantEntity(string tenancyName, string displayName)
            : this()
        {
            Id = Guid.NewGuid();
            TenancyName = tenancyName;
            DisplayName = displayName;
        }

        [Required]
        [StringLength(MaxTenancyNameLength)]
        public virtual string TenancyName { get; set; }

        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}