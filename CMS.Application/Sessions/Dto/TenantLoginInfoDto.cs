using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CMS.Domain.Tenant;

namespace CMS.Application.Sessions.Dto
{
    [AutoMapFrom(typeof(TenantEntity))]
    public class TenantLoginInfoDto : EntityDto<Guid>
    {
        public string TenancyName { get; set; }

        public string DisplayName { get; set; }
    }
}
