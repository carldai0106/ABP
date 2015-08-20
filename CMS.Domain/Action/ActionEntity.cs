using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using CMS.Domain.ActionModule;
using CMS.Domain.Tenant;
using CMS.FW.Domain;

namespace CMS.Domain.Action
{
    [Table("Actions")]
    public class ActionEntity : FullAuditedAndTenantEntity, IPassivable
    {
        public const int MaxActionCodeLength = 128;
        public const int MaxDisplayNameLength = 256;
        public const int MaxDescriptionLength = 512;

        [Required]
        [StringLength(MaxActionCodeLength)]
        public virtual string ActionCode { get; set; }
        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }
        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("TenantId")]
        public virtual TenantEntity Tenant { get; set; }
        [ForeignKey("ActionId")]
        public virtual ICollection<ActionModuleEntity> ActionModuels { get; set; }
    }
}
