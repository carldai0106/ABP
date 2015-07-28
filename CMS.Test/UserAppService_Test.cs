using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using CMS.Application.MultiTenancy;
using CMS.Application.Role;
using CMS.Application.Role.Dto;
using CMS.Application.User;
using CMS.Application.User.Dto;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace CMS.Test
{
    public class UserAppService_Test : TestBase<Guid, Guid>
    {
        private readonly IUserAppService _userAppService;
        private readonly IRoleAppService _roleAppService;
        readonly ITestOutputHelper _output;

        public UserAppService_Test(ITestOutputHelper output)
        {
            _userAppService = Resolve<IUserAppService>();
            _roleAppService = Resolve<IRoleAppService>();
            this._output = output;

            Resolve<IMultiTenancyConfig>().IsEnabled = true;
        }

        

        [Fact]
        public async Task Create_Get_Update_Delete_User_Test()
        {
            var list = new List<CreateUserDto>
            {
                new CreateUserDto
                {
                    Email = "dc01061@126.com",
                    IsActive = true,
                    FirstName = "Perry",
                    LastName = "Xu",
                    Password = "123456",
                    UserName = "Perry"
                },
                new CreateUserDto
                {
                    Email = "dc01062@126.com",
                    IsActive = true,
                    FirstName = "T1",
                    LastName = "D1",
                    Password = "123456",
                    UserName = "Test"
                }
            };

            foreach (var item in list)
            {
                await _userAppService.CreateUser(item);
            }

            var user = await _userAppService.GetUser("Test");
            user.ShouldNotBe(null);

            var ue =  user.MapTo<UserEditDto>();
            ue.Email = "dc01062@126.com";
            ue.Password = "123321";
            ue.FirstName = "vivien";
            ue.LastName = "zhu";

            await _userAppService.UpdateUser(ue);

            user = await _userAppService.GetUser("Test");
            user.ShouldNotBe(null);
            user.FirstName.ShouldBe("vivien");
            user.LastName.ShouldBe("zhu");

            await _userAppService.DeleteUser(new Abp.Application.Services.Dto.IdInput<Guid> {Id = user.Id});

            user = await _userAppService.GetUser("Test");
            user.ShouldBe(null);
        }

        [Fact]
        public async Task Create_UserRole_Test()
        {
            var pageResult = await _roleAppService.GetRoles(new GetRolesInput());
            string str = JsonConvert.SerializeObject(pageResult.Items, Formatting.Indented);
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

        [Fact]
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

        [Fact]
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
        }
    }
}
