using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using CMS.Application.User;
using CMS.Application.User.Dto;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace CMS.Test
{
    public class UserAppService_Test : TestBase<Guid, Guid>
    {
        private readonly ITestOutputHelper _output;
        private readonly IUserAppService _userAppService;

        public UserAppService_Test(ITestOutputHelper output)
        {
            _userAppService = Resolve<IUserAppService>();

            _output = output;

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

            var ue = user.MapTo<UserEditDto>();
            ue.Email = "dc01062@126.com";
            ue.Password = "123321";
            ue.ConfimPassword = "123321";
            ue.FirstName = "vivien";
            ue.LastName = "zhu";

            await _userAppService.UpdateUser(ue);

            user = await _userAppService.GetUser("Test");
            user.ShouldNotBe(null);
            user.FirstName.ShouldBe("vivien");
            user.LastName.ShouldBe("zhu");

            await _userAppService.DeleteUser(new IdInput<Guid> {Id = user.Id});

            user = await _userAppService.GetUser("Test");
            user.ShouldBe(null);
        }

        [Fact]
        public async Task Get_User_Change_Password_Test()
        {
            var user = await _userAppService.GetUser("admin");
            user.ShouldNotBe(null);

            var ue = user.MapTo<UserEditDto>();

            ue.Password = "123456";
            ue.ConfimPassword = "123456";

            await _userAppService.UpdateUser(ue);
        }
    }
}