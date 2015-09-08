using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Configuration.Startup;
using CMS.Application.Action;
using CMS.Application.Action.Dto;
using CMS.Application.Module;
using CMS.Application.Module.Dto;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace CMS.Test
{
    public class ModuleAppService_Tests : TestBase<Guid, Guid>
    {
        private readonly IActionAppService _actionAppService;
        private readonly IModuleAppService _moduleAppService;
        private readonly ITestOutputHelper output;

        public ModuleAppService_Tests(ITestOutputHelper output)
        {
            _moduleAppService = Resolve<IModuleAppService>();
            _actionAppService = Resolve<IActionAppService>();
            this.output = output;

            Resolve<IMultiTenancyConfig>().IsEnabled = true;
        }

        [Fact]
        public async Task Create_Update_Delete_Module_Test()
        {
            var list = new List<ModuleCreateDto>
            {
                new ModuleCreateDto
                {
                    AppUrl = "~/Admin/Dashboard/Index",
                    ClassName = "fa fa-dashboard",
                    Description = "Dashboard",
                    DisplayAsFrontMenu = false,
                    DisplayAsMenu = true,
                    DisplayName = "Dashboard",
                    FrontUrl = "#",
                    IsActive = true,
                    ModuleCode = "CMS.Admin.Dashboard",
                    Order = 1,
                    ParentId = null
                },
                new ModuleCreateDto
                {
                    AppUrl = "#",
                    ClassName = "fa fa-cog",
                    Description = "Setup",
                    DisplayAsFrontMenu = false,
                    DisplayAsMenu = true,
                    DisplayName = "Setup",
                    FrontUrl = "#",
                    IsActive = true,
                    ModuleCode = "CMS.Admin.Setup",
                    Order = 2,
                    ParentId = null
                }
            };

            foreach (var item in list)
            {
                await _moduleAppService.Create(item);
            }

            var setupModule = await _moduleAppService.GetModule("CMS.Admin.Setup");
            setupModule.ShouldNotBe(null);
            setupModule.ModuleCode.ShouldBe("CMS.Admin.Setup");

            var parentid = setupModule.Id;
            var setups = new List<ModuleCreateDto>
            {
                new ModuleCreateDto
                {
                    AppUrl = "~/Admin/Users/Index",
                    ClassName = "",
                    Description = "Users",
                    DisplayAsFrontMenu = false,
                    DisplayAsMenu = true,
                    DisplayName = "Users",
                    FrontUrl = "#",
                    IsActive = true,
                    ModuleCode = "CMS.Admin.Users",
                    Order = 1,
                    ParentId = parentid
                },
                new ModuleCreateDto
                {
                    AppUrl = "~/Admin/Roles/Index",
                    ClassName = "",
                    Description = "Roles",
                    DisplayAsFrontMenu = false,
                    DisplayAsMenu = true,
                    DisplayName = "Roles",
                    FrontUrl = "#",
                    IsActive = true,
                    ModuleCode = "CMS.Admin.Roles",
                    Order = 2,
                    ParentId = parentid
                },
                new ModuleCreateDto
                {
                    AppUrl = "~/Admin/Actions/Index",
                    ClassName = "",
                    Description = "Actions",
                    DisplayAsFrontMenu = false,
                    DisplayAsMenu = true,
                    DisplayName = "Actions",
                    FrontUrl = "#",
                    IsActive = true,
                    ModuleCode = "CMS.Admin.Actions",
                    Order = 3,
                    ParentId = parentid
                },
                new ModuleCreateDto
                {
                    AppUrl = "~/Admin/Modules/Index",
                    ClassName = "",
                    Description = "Modules",
                    DisplayAsFrontMenu = false,
                    DisplayAsMenu = true,
                    DisplayName = "Modules",
                    FrontUrl = "#",
                    IsActive = true,
                    ModuleCode = "CMS.Admin.Modules",
                    Order = 4,
                    ParentId = parentid
                },
                new ModuleCreateDto
                {
                    AppUrl = "~/Admin/RoleRights/Index",
                    ClassName = "",
                    Description = "Role Rights",
                    DisplayAsFrontMenu = false,
                    DisplayAsMenu = true,
                    DisplayName = "Role Rights",
                    FrontUrl = "#",
                    IsActive = true,
                    ModuleCode = "CMS.Admin.RoleRights",
                    Order = 5,
                    ParentId = parentid
                },
                new ModuleCreateDto
                {
                    AppUrl = "~/Admin/Tests/Index",
                    ClassName = "",
                    Description = "Tests",
                    DisplayAsFrontMenu = false,
                    DisplayAsMenu = true,
                    DisplayName = "Tests",
                    FrontUrl = "#",
                    IsActive = true,
                    ModuleCode = "CMS.Admin.Tests",
                    Order = 6,
                    ParentId = parentid
                }
            };

            foreach (var item in setups)
            {
                await _moduleAppService.Create(item);
            }

            var module = await _moduleAppService.GetModule("CMS.Admin.Tests");
            module.ShouldNotBe(null);
            module.ModuleCode.ShouldBe("CMS.Admin.Tests");

            module.DisplayName = "Tests : just a test";

            await _moduleAppService.Update(module);
            module = await _moduleAppService.GetModule("CMS.Admin.Tests");
            module.DisplayName.ShouldBe("Tests : just a test");

            await _moduleAppService.Delete(new IdInput<Guid> {Id = module.Id});

            var rs = await _moduleAppService.GetModule(new IdInput<Guid> {Id = module.Id});
            rs.ShouldBe(null);
        }

        [Fact]
        public async Task Create_ActionModules_Test()
        {
            var rs = await _actionAppService.GetActions(new GetActionsInput());
            var items = rs.Items;

            var list = await _moduleAppService.GetModules(new GetModulesInput());
            var modules = list.Items; //list.Items.Where(x => x.ParentId != null);

            //var actions = items.Where(
            //        x => x.ActionCode == "CMS.Create" || x.ActionCode == "CMS.Update" || x.ActionCode == "CMS.Delete");
            var actions = items;

            var am = (from m in modules
                from a in actions
                select new ActionModuleDto
                {
                    ActionId = a.Id,
                    ModuleId = m.Id,
                    Status = true
                }).ToList();

            await _moduleAppService.CreateOrUpdate(am);
        }

        [Fact]
        public async Task Update_ActionModules_Test()
        {
            var modules = await _moduleAppService.GetModules(new GetModulesInput
            {
                Filter = "CMS.Admin.Users"
            });

            foreach (var m in modules.Items)
            {
                foreach (var a in m.ActionModules)
                {
                    a.Status = false;
                }

                await _moduleAppService.CreateOrUpdate(m.ActionModules);
            }
        }

        [Fact]
        public async Task Get_Module_ActionModule_Test()
        {
            var module = await _moduleAppService.GetModule("CMS.Admin.Dashboard");
            var str = JsonConvert.SerializeObject(module, Formatting.Indented);
            output.WriteLine(str);
        }

        [Fact]
        public async Task Get_All_Modules_Test()
        {
            var list = await _moduleAppService.GetModules(new GetModulesInput());
            var str = JsonConvert.SerializeObject(list.Items, Formatting.Indented);
            output.WriteLine(str);
        }
    }
}