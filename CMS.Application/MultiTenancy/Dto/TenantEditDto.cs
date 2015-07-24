using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.Tenant;

namespace CMS.Application.MultiTenancy.Dto
{
    [AutoMap(typeof(TenantEntity))]
    public class TenantEditDto : EntityDto<Guid>, IDoubleWayDto
    {
        [Required]
        [StringLength(TenantEntity.MaxTenancyNameLength)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(TenantEntity.MaxDisplayNameLength)]
        public string DisplayName { get; set; }

        public bool IsActive { get; set; }
    }
}
