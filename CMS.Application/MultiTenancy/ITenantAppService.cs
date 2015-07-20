using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using CMS.Application.MultiTenancy.Dto;

namespace CMS.Application.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        Task CreateTenant(CreateTenantDto input);
    }
}
