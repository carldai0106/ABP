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
using Shouldly;
using Xunit;

namespace CMS.Test
{
    public class ActionAppService_Tests : TestBase<Guid, Guid>
    {
        private readonly IActionAppService _actionAppService;
        public ActionAppService_Tests()
        {
            _actionAppService = Resolve<IActionAppService>();
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
        }

        [Fact]
        public async Task Create_Get_Update_Delete_Actions()
        {
            var actionTypes = new[]
            {
                "Create", "Update", "Delete", "Display", "Preview", "Search", "Export", "Test"
            };

            foreach (var item in actionTypes)
            {
                var dto = new ActionCreateDto
                {
                    ActionCode = "CMS." + item,
                    DisplayName = item,
                    Description = item,
                    IsActive = true
                };

                await _actionAppService.Create(dto);
            }

            var action = await _actionAppService.GetAction("CMS.Test");
            action.ShouldNotBe(null);
            action.ActionCode.ShouldBe("CMS.Test");

            action.Description = "Test, just a test!";
            await _actionAppService.Update(action);

            action = await _actionAppService.GetAction("CMS.Test");
            action.ShouldNotBe(null);
            action.Description.ShouldBe("Test, just a test!");

            await _actionAppService.Delete(new Abp.Application.Services.Dto.IdInput<Guid>{ Id = action.Id });

            action = await _actionAppService.GetAction("CMS.Test");
            action.ShouldBe(null);
        }
    }
}
