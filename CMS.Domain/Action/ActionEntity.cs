using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using CMS.Domain.ActionModule;

namespace CMS.Domain.Action
{
    [Table("Actions")]
    public class ActionEntity : FullAuditedEntity<Guid, Guid>, IPassivable
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

        [ForeignKey("ActionId")]
        public virtual ICollection<ActionModuleEntity> ActionModuels { get; set; }
    }
}