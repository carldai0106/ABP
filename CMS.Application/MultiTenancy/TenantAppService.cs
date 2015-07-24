using System;

using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;
using Abp;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CMS.Application.MultiTenancy.Dto;
using CMS.Domain;
using CMS.Application.IdentityFramework;
using CMS.Application.Localization;
using CMS.Domain.Tenant;
using Microsoft.AspNet.Identity;


namespace CMS.Application.MultiTenancy
{
    public class TenantAppService : ApplicationService<Guid, Guid>, ITenantAppService
    {
        private readonly ICmsRepository<TenantEntity, Guid> _repository;
        public TenantAppService(ICmsRepository<TenantEntity, Guid> repository)
        {
            _repository = repository;
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }

        public async Task CreateTenant(CreateTenantDto input)
        {
            var tenant = new TenantEntity(input.TenancyName, input.DisplayName)
            {
                IsActive = input.IsActive
            };

            if (await _repository.FirstOrDefaultAsync(x => x.TenancyName == tenant.TenancyName) != null)
            {
               var rs = IdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
                rs.CheckErrors();
            }

            await _repository.InsertAsync(tenant);
            

            await CurrentUnitOfWork.SaveChangesAsync(); //To get new tenant's id.

            //We are working entities of new tenant, so changing tenant filter
            CurrentUnitOfWork.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenant.Id);
        }

        public async Task<TenantEditDto> GetTenant(EntityRequestInput<Guid> input)
        {
            return (await _repository.FirstOrDefaultAsync(input.Id)).MapTo<TenantEditDto>();
        }

        public async Task UpdateTenant(TenantEditDto input)
        {
            if (await _repository.FirstOrDefaultAsync(t => t.TenancyName == input.TenancyName && t.Id != input.Id) != null)
            {
                var rs = IdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), input.TenancyName));
                rs.CheckErrors();
            }

            var tenant = await _repository.FirstOrDefaultAsync(input.Id);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with id: " + input.Id);
            }

            input.MapTo(tenant);

            await _repository.UpdateAsync(tenant);
        }

        public async Task DeleteTenant(EntityRequestInput<Guid> input)
        {
            var tenant = await _repository.FirstOrDefaultAsync(input.Id);
            if (tenant == null)
            {
                throw new AbpException("There is no tenant with id: " + input.Id);
            }

            await _repository.DeleteAsync(tenant);
        }

        public async Task<TenantEditDto> GetTenant(string tenancyName)
        {
            return (await _repository.FirstOrDefaultAsync(x => x.TenancyName == tenancyName)).MapTo<TenantEditDto>();
        }
    }
}
