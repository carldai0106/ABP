using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using CMS.Application.Action;
using CMS.Application.Action.Dto;
using CMS.Application.MultiTenancy;
using Xunit;

namespace CMS.Test
{
    public class ActionAppService_Tests : TestBase<Guid, Guid>
    {
        private readonly IActionAppService _actionAppService;
        private readonly ITenantAppService _tenantAppService;
        public ActionAppService_Tests()
        {
            _actionAppService = Resolve<IActionAppService>();
            _tenantAppService = Resolve<ITenantAppService>();
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
        }

        [Fact]
        public async Task Create_Actions()
        {
            var tenant = await _tenantAppService.GetTenant("TKM");
            AbpSession.TenantId = tenant.Id;

            var actionTypes = new[]
            {
                "Create", "Update", "Delete", "Display", "Preview", "Search", "Export"
            };

            foreach (var item in actionTypes)
            {
                var dto = new ActionCreateDto();
                dto.ActionCode = tenant.TenancyName + "." + item;
                dto.DisplayName = item;
                dto.Description = item;
                dto.IsActive = true;

                await _actionAppService.Create(dto);
            }
        }

        [Fact]
        public async Task Get_Actions()
        {
            var list = await _actionAppService.GetActions(new GetActionsInput
            {
                Sorting = "ActionCode"
            });

            foreach (var item in list.Items)
            {
                Debug.WriteLine(item.ActionCode);
            }
        }
    }
}
