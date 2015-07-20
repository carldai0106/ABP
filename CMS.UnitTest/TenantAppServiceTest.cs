using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMS.Application.MultiTenancy;
using CMS.Application.MultiTenancy.Dto;
using CMS.Domain;
using CMS.Domain.Tenant;

namespace CMS.UnitTest
{
    [TestClass]
    public class TenantAppServiceTest : TestBase<Guid, Guid>
    {
        private readonly ITenantAppService _appService;

        public TenantAppServiceTest()
        {
            _appService = LocalIocManager.Resolve<ITenantAppService>();
        }

        [TestMethod]
        public async Task CreateTenant()
        {
            var dto = new CreateTenantDto
            {
                DisplayName = "carl dai",
                IsActive = true,
                TenancyName = "bcsint3"
            };

            await _appService.CreateTenant(dto);
        }

        [TestMethod]
        public void GetTest()
        {
            //var taskRepository = LocalIocManager.Resolve<ICmsRepository<TenantEntity,Guid>>();
            //taskRepository.Insert(new TenantEntity("1", "2"));
            //Debug.WriteLine(taskRepository.Count());
        }
    }
}
