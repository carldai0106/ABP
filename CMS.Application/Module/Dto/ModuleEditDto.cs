using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.Module;

namespace CMS.Application.Module.Dto
{
    [AutoMap(typeof (ModuleEntity))]
    public class ModuleEditDto : EntityDto<Guid>, IDoubleWayDto
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
        public virtual ICollection<ActionModuleDto> ActionModules { get; set; }
    }
}