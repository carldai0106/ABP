using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Configuration.Startup;
using CMS.Application.Action;
using CMS.Application.Module;
using CMS.Application.Module.Dto;
using CMS.Application.Role;
using CMS.Application.Role.Dto;
using CMS.Application.User;
using CMS.Application.User.Dto;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions;

namespace CMS.Test
{
    [TestCaseOrderer("Xunit.Extensions.PriorityOrderer", "Xunit.Extensions")]
    public class RoleAppService_Tests : TestBase<Guid, Guid>
    {
        private readonly IActionAppService _actionAppService;
        private readonly IModuleAppService _moduleAppService;
        private readonly ITestOutputHelper _output;
        private readonly IRoleAppService _roleAppService;
        private readonly IUserAppService _userAppService;

        public RoleAppService_Tests(ITestOutputHelper output)
        {
            _roleAppService = Resolve<IRoleAppService>();
            _moduleAppService = Resolve<IModuleAppService>();
            _actionAppService = Resolve<IActionAppService>();
            _userAppService = Resolve<IUserAppService>();
            _output = output;
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
        }

        [Fact, TestPriority(0)]
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
                },
                new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Accountant",
                    IsActive = true,
                    RoleCode = "Accountant"
                },
                new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Group Leader",
                    IsActive = true,
                    RoleCode = "GroupLeader"
                },
                new RoleCreateDto
                {
                    Description = "",
                    DisplayName = "Staff",
                    IsActive = true,
                    RoleCode = "Staff"
                },
                new RoleCreateDto
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

            await _roleAppService.Delete(new IdInput<Guid> {Id = rs.Id});

            rs = await _roleAppService.GetRole(new IdInput<Guid> {Id = rs.Id});
            rs.ShouldBe(null);
        }

        [Fact, TestPriority(1)]
        public async Task Create_UserRole_Test()
        {
            var pageResult = await _roleAppService.GetRoles(new GetRolesInput());
            var str = JsonConvert.SerializeObject(pageResult.Items, Formatting.Indented);
            _output.WriteLine(str);

            var userAdmin = await _userAppService.GetUser("admin");
            var userPerry = await _userAppService.GetUser("Perry");

            str = JsonConvert.SerializeObject(userAdmin, Formatting.Indented);
            _output.WriteLine(str);

            var users = await _userAppService.GetUsers(new GetUsersInput());

            str = JsonConvert.SerializeObject(users.Items, Formatting.Indented);
            _output.WriteLine(str);

            var list = pageResult.Items.Where(x => x.RoleCode == "GroupLeader" || x.RoleCode == "Manager");

            var usrRoles = new List<UserRoleDto>();
            foreach (var item in list)
            {
                usrRoles.Add(new UserRoleDto
                {
                    RoleId = item.Id,
                    Status = true,
                    UserId = userPerry.Id
                });
            }

            foreach (var item in pageResult.Items)
            {
                usrRoles.Add(new UserRoleDto
                {
                    RoleId = item.Id,
                    Status = true,
                    UserId = userAdmin.Id
                });
            }

            await _userAppService.CreateOrUpdate(usrRoles);
        }

        [Fact, TestPriority(2)]
        public async Task Update_UserRole_Test()
        {
            var pageResult = await _roleAppService.GetRoles(new GetRolesInput());

            var userAdmin = await _userAppService.GetUser("admin");
            foreach (var item in userAdmin.UserRoles)
            {
                foreach (var rs in pageResult.Items)
                {
                    if (item.RoleId == rs.Id && (rs.RoleCode == "Staff" || rs.RoleCode == "Accountant"))
                    {
                        item.Status = false;
                    }
                }
            }

            await _userAppService.CreateOrUpdate(userAdmin.UserRoles);
        }

        [Fact, TestPriority(3)]
        public async Task Get_UserRole_Test()
        {
            var userAdmin = await _userAppService.GetUser("admin");
            var userPerry = await _userAppService.GetUser("Perry");

            var str = JsonConvert.SerializeObject(userAdmin, Formatting.Indented);

            _output.WriteLine(str);
            _output.WriteLine(Environment.NewLine + "-----------------------------------------------------" +
                              Environment.NewLine);

            str = JsonConvert.SerializeObject(userPerry, Formatting.Indented);
            _output.WriteLine(str);

            var rs = await _userAppService.GetPermission(new NullableIdInput<Guid>
            {
                Id = userAdmin.Id
            }, "CMS.Admin.RoleRights", "");

            _output.WriteLine(str);
            _output.WriteLine(Environment.NewLine + "-----------------------------------------------------" +
                              Environment.NewLine);

            str = JsonConvert.SerializeObject(rs, Formatting.Indented);
            _output.WriteLine(str);
        }

        [Fact, TestPriority(4)]
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

            role = roles.Items.FirstOrDefault(x => x.RoleCode == "GroupLeader");

            list = new List<RoleRightDto>();
            foreach (var m in items.Where(x => x.ModuleCode == "CMS.Admin.Setup" || x.ModuleCode == "CMS.Admin.Modules")
                )
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

        [Fact, TestPriority(5)]
        public async Task Update_RoleRight_Test()
        {
            var role = await _roleAppService.GetRole("Administrator");
            var modules = await _moduleAppService.GetModules(new GetModulesInput());
            var module = modules.Items.FirstOrDefault(x => x.ModuleCode == "CMS.Admin.Users");

            var action = await _actionAppService.GetAction("CMS.Delete");

            var actionid = default(Guid);
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

        [Fact, TestPriority(6)]
        public async Task Get_RoleRight_Test()
        {
            var role = await _roleAppService.GetRole("Administrator");
            var str = JsonConvert.SerializeObject(role, Formatting.Indented);
            _output.WriteLine(str);
        }
    }
}