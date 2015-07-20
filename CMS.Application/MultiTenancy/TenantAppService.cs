using System;

using System.Diagnostics;
using System.Linq;

using System.Threading.Tasks;

using Abp.Application.Services;
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
        private readonly IRepository<TenantEntity, Guid> _repository;
        public TenantAppService(IRepository<TenantEntity, Guid> repository)
        {
            _repository = repository;
            //LocalizationManager = NullLocalizationManager.Instance;
            //LocalizationSource
            LocalizationSourceName = CmsConsts.LocalizationSourceName;
        }

        public async Task CreateTenant(CreateTenantDto input)
        {
            var tenant = new TenantEntity(input.TenancyName, input.DisplayName)
            {
                IsActive = input.IsActive
            };
            
            var rs = await CreateAsync(tenant);
            rs.CheckErrors();

            await CurrentUnitOfWork.SaveChangesAsync(); //To get new tenant's id.

            //We are working entities of new tenant, so changing tenant filter
            CurrentUnitOfWork.SetFilterParameter(AbpDataFilters.MayHaveTenant, AbpDataFilters.Parameters.TenantId, tenant.Id);
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        private async Task<IdentityResult> CreateAsync(TenantEntity tenant)
        {
            if (await _repository.FirstOrDefaultAsync(x => x.TenancyName == tenant.TenancyName) != null)
            {
                return IdentityResult.Failed(string.Format(L("TenancyNameIsAlreadyTaken"), tenant.TenancyName));
            }
            await _repository.InsertAsync(tenant);
            return IdentityResult.Success;
        }
    }
}
