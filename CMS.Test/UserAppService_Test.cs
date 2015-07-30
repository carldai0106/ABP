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
       
        readonly ITestOutputHelper _output;

        public UserAppService_Test(ITestOutputHelper output)
        {
            _userAppService = Resolve<IUserAppService>();
         
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
        
    }
}
