using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration.Startup;
using CMS.Application.Action;
using CMS.Application.Module;
using CMS.Application.Module.Dto;
using CMS.Application.Role;
using CMS.Application.Role.Dto;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace CMS.Test
{
    public class RoleAppService_Tests : TestBase<Guid, Guid>
    {
        private readonly IRoleAppService _roleAppService;
        private readonly IModuleAppService _moduleAppService;
        private readonly IActionAppService _actionAppService;
        private readonly ITestOutputHelper _output;

        public RoleAppService_Tests(ITestOutputHelper output)
        {
            _roleAppService = Resolve<IRoleAppService>();
            _moduleAppService = Resolve<IModuleAppService>();
            _actionAppService = Resolve<IActionAppService>();
            _output = output;
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
        }

       

        [Fact]
        public async Task Create_Update_Get_Delete_Role_Test()
        {
            var list = new List<RoleCreateDto>
            {
                new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Administrator",
                    IsActive = true,
                    RoleCode = "Administrator"
                },
                new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Manager",
                    IsActive = true,
                    RoleCode = "Manager"
                },new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Accountant",
                    IsActive = true,
                    RoleCode = "Accountant"
                },new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Group Leader",
                    IsActive = true,
                    RoleCode = "GroupLeader"
                },new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Staff",
                    IsActive = true,
                    RoleCode = "Staff"
                },new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Test",
                    IsActive = true,
                    RoleCode = "Test"
                }
            };

            foreach (var item in list)
            {
                await _roleAppService.Create(item);
            }

            var roleTest = await _roleAppService.GetRole("Test");
            roleTest.ShouldNotBe(null);
            roleTest.RoleCode.ShouldBe("Test");

            roleTest.RoleCode = "TestRole";

            await _roleAppService.Update(roleTest);

            var rs = await _roleAppService.GetRole("TestRole");
            rs.ShouldNotBe(null);
            rs.RoleCode.ShouldBe("TestRole");

            await _roleAppService.Delete(new Abp.Application.Services.Dto.IdInput<Guid> {Id = rs.Id});

            rs = await _roleAppService.GetRole(new Abp.Application.Services.Dto.IdInput<Guid> { Id = rs.Id});
            rs.ShouldBe(null);
        }

        [Fact]
        public async Task Create_RoleRight_Test()
        {
            var roles = await _roleAppService.GetRoles(new GetRolesInput());
            var modules = await _moduleAppService.GetModules(new GetModulesInput());

            var role = roles.Items.FirstOrDefault(x => x.RoleCode == "Administrator");
            var items = modules.Items;

            var list = new List<RoleRightDto>();
            foreach (var m in items)
            {
                foreach (var am in m.ActionModules)
                {
                    list.Add(new RoleRightDto
                    {
                        ActionModuleId = am.Id.Value,
                        RoleId = role.Id,
                        Status = true
                    });
                }
            }

            await _roleAppService.CreateOrUpdate(list);
        }

        [Fact]
        public async Task Update_RoleRight_Test()
        {
            var role = await _roleAppService.GetRole("Administrator");
            var modules = await _moduleAppService.GetModules(new GetModulesInput());
            var module = modules.Items.FirstOrDefault(x => x.ModuleCode == "CMS.Admin.Users");

            var action = await _actionAppService.GetAction("CMS.Delete");

            Guid actionid = default(Guid);
            foreach (var am in module.ActionModules)
            {
                foreach (var r2 in role.RoleRights)
                {
                    if (am.Id == r2.ActionModuleId && am.ActionId == action.Id)
                    {
                        actionid = r2.ActionModuleId;
                        r2.Status = false;
                    }
                }
            }

            await _roleAppService.CreateOrUpdate(role.RoleRights);

            role = await _roleAppService.GetRole("Administrator");

            role.RoleRights.FirstOrDefault(x => x.ActionModuleId == actionid).Status.ShouldBe(false);

            var str = JsonConvert.SerializeObject(role, Formatting.Indented);
            _output.WriteLine(str);
        }

        [Fact]
        public async Task Get_RoleRight_Test()
        {
            var role = await _roleAppService.GetRole("Administrator");
            var str = JsonConvert.SerializeObject(role, Formatting.Indented);
            _output.WriteLine(str);
        }
    }
}
