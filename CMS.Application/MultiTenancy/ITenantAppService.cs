using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CMS.Application.MultiTenancy.Dto;
using CMS.Domain.Tenant;

namespace CMS.Application.MultiTenancy
{
    public interface ITenantAppService : IApplicationService
    {
        Task CreateTenant(CreateTenantDto input);
        Task<TenantEditDto> GetTenant(EntityRequestInput<Guid> input);
        Task UpdateTenant(TenantEditDto input);
        Task DeleteTenant(EntityRequestInput<Guid> input);
        Task<TenantEditDto> GetTenant(string tenancyName);
    }
}
