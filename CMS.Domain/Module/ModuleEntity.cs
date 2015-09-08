using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.ActionModule;
using CMS.Domain.Tenant;

namespace CMS.Domain.Module
{
    [Table("Modules")]
    public class ModuleEntity : FullAuditedAndTenantEntity, IPassivable
    {
        public const int MaxModuleCodeLength = 128;
        public const int MaxDisplayNameLength = 256;
        public const int MaxClassNameLength = 128;
        public const int MaxUrlLength = 256;
        public const int MaxDescriptionLength = 512;
        public virtual Guid? ParentId { get; set; }

        [Required]
        [StringLength(MaxModuleCodeLength)]
        public virtual string ModuleCode { get; set; }

        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        public virtual bool DisplayAsMenu { get; set; }
        public virtual bool DisplayAsFrontMenu { get; set; }

        [StringLength(MaxClassNameLength)]
        public virtual string ClassName { get; set; }

        [StringLength(MaxUrlLength)]
        public virtual string AppUrl { get; set; }

        [StringLength(MaxUrlLength)]
        public virtual string FrontUrl { get; set; }

        public virtual int Order { get; set; }

        [StringLength(MaxDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        [ForeignKey("TenantId")]
        public virtual TenantEntity Tenant { get; set; }

        [ForeignKey("ParentId")]
        public virtual ModuleEntity Parent { get; set; }

        public virtual ICollection<ModuleEntity> Chileren { get; set; }

        [ForeignKey("ModuleId")]
        public virtual ICollection<ActionModuleEntity> ActionModules { get; set; }
    }
}