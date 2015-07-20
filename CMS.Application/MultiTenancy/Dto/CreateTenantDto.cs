using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using CMS.Domain;
using CMS.Domain.Tenant;

namespace CMS.Application.MultiTenancy.Dto
{
    public class CreateTenantDto : IInputDto, IPassivable
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
