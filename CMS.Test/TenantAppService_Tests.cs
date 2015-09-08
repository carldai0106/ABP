using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using CMS.Application.MultiTenancy;
using CMS.Application.MultiTenancy.Dto;
using Shouldly;
using Xunit;

namespace CMS.Test
{
    public class TenantAppService_Tests : TestBase<Guid, Guid>
    {
        private readonly ITenantAppService _tenantAppService;

        public TenantAppService_Tests()
        {
            _tenantAppService = Resolve<ITenantAppService>();
        }

        [Fact]
        public async Task Create_Get_Update_Delete_Tenant_Test()
        {
            var dto = new CreateTenantDto
            {
                DisplayName = "Tenant_TEST",
                IsActive = true,
                TenancyName = "Tenant_TEST"
            };

            await _tenantAppService.CreateTenant(dto);

            var dtoEdit = await _tenantAppService.GetTenant(dto.TenancyName);
            dtoEdit.ShouldNotBe(null);
            dtoEdit.TenancyName.ShouldBe("Tenant_TEST");

            dtoEdit.TenancyName = "Tenant_TEST_UPDATE";

            await _tenantAppService.UpdateTenant(dtoEdit);

            var dtoGet = await _tenantAppService.GetTenant("Tenant_TEST_UPDATE");
            dtoGet.ShouldNotBe(null);
            dtoEdit.TenancyName.ShouldBe("Tenant_TEST_UPDATE");

            await _tenantAppService.DeleteTenant(new EntityRequestInput<Guid>(dtoGet.Id));

            var tenant = await _tenantAppService.GetTenant(new EntityRequestInput<Guid>(dtoGet.Id));
            tenant.ShouldBe(null);
        }
    }
}