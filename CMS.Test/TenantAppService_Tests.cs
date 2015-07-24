using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async Task Create_Tenant_Test()
        {
            
            var dto = new CreateTenantDto
            {
                DisplayName = "TKM_TEST",
                IsActive = true,
                TenancyName = "TKM_TEST"
            };

            await _tenantAppService.CreateTenant(dto);

            var rs = await _tenantAppService.GetTenant(dto.TenancyName);
            rs.ShouldNotBe(null);
        }

        [Fact]
        public async Task Create_Update_And_Delete_Tenant_Test()
        {
            var dto = new CreateTenantDto
            {
                DisplayName = "BCS_TEST",
                IsActive = true,
                TenancyName = "BCS_TEST"
            };

            await _tenantAppService.CreateTenant(dto);

            var dtoEdit = await _tenantAppService.GetTenant(dto.TenancyName);
            dtoEdit.ShouldNotBe(null);
            dtoEdit.TenancyName.ShouldBe("BCS_TEST");

            dtoEdit.TenancyName = "BCS_TEST_UPDATE";

            await _tenantAppService.UpdateTenant(dtoEdit);

            var dtoGet = await _tenantAppService.GetTenant("BCS_TEST_UPDATE");
            dtoGet.ShouldNotBe(null);
            dtoEdit.TenancyName.ShouldBe("BCS_TEST_UPDATE");

            var rs = await _tenantAppService.GetTenant(new Abp.Application.Services.Dto.EntityRequestInput<Guid>(dtoGet.Id));
            rs.ShouldNotBe(null);

            await _tenantAppService.DeleteTenant(new Abp.Application.Services.Dto.EntityRequestInput<Guid>(dtoGet.Id));

            var tenant = await _tenantAppService.GetTenant(new Abp.Application.Services.Dto.EntityRequestInput<Guid>(dtoGet.Id));
            tenant.ShouldBe(null);
        }
    }
}
