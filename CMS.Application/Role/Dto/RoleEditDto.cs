using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.Role;

namespace CMS.Application.Role.Dto
{
    [AutoMap(typeof (RoleEntity))]
    public class RoleEditDto : EntityDto<Guid>, IDoubleWayDto
    {
        public const int MaxRoleCodeLength = 128;
        public const int MaxDisplayNameLength = 256;
        public const int MaxDescriptionLength = 512;

        [Required]
        [StringLength(MaxRoleCodeLength)]
        public virtual string RoleCode { get; set; }

        [Required]
        [StringLength(MaxDisplayNameLength)]
        public virtual string DisplayName { get; set; }

        [StringLength(MaxDescriptionLength)]
        public string Description { get; set; }

        public virtual bool IsActive { get; set; }
        public virtual int Order { get; set; }
        public virtual ICollection<RoleRightDto> RoleRights { get; set; }
    }
}