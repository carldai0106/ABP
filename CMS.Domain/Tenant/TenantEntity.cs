using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.User;

namespace CMS.Domain.Tenant
{
    public class TenantEntity : FullAuditedEntity<Guid, Guid>, IPassivable
    {
        public const int MaxTenancyNameLength = 128;
        
        public const int MaxDisplayNameLength = 256; 

        [Required]
        [StringLength(MaxTenancyNameLength)]
        public string TenancyName { get; private set; }

        [Required]
        [StringLength(MaxDisplayNameLength)]
        public string DisplayName { get; private set; }

        public bool IsActive { get; set; }

        public ICollection<UserEntity> Users { get; private set; }

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
    }
}
