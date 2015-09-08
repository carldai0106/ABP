using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.RoleRight;

namespace CMS.Application.Role.Dto
{
    [AutoMap(typeof (RoleRightEntity))]
    public class RoleRightDto : EntityDto<Guid?>, IDoubleWayDto
    {
        [Required]
        public virtual Guid RoleId { get; set; }

        [Required]
        public virtual Guid ActionModuleId { get; set; }

        public virtual bool Status { get; set; }
    }
}