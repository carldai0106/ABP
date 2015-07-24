using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using CMS.Application.MultiTenancy;
using CMS.Application.User;
using CMS.Application.User.Dto;

using Shouldly;
using Xunit;

namespace CMS.Test
{
    public class UserAppService_Test : TestBase<Guid, Guid>
    {
        private readonly IUserAppService _userAppService;
        private readonly ITenantAppService _tenantAppService;

        public UserAppService_Test()
        {
            _userAppService = Resolve<IUserAppService>();
            _tenantAppService = Resolve<ITenantAppService>();
            Resolve<IMultiTenancyConfig>().IsEnabled = true;
        }

        [Fact]
        public async Task Create_Update_Delete_User_Test()
        {
            var tenant = await _tenantAppService.GetTenant("bcsint");
            AbpSession.TenantId = tenant.Id;

            var cu = new CreateUserDto
            {
                Email = "dc01061@126.com",
                IsActive = true,
                FirstName = "T1",
                LastName = "D1",
                Password = "123456",
                UserName = "Test"
            };

            await _userAppService.CreateUser(cu);

            var user = await _userAppService.GetUser("Test");
            user.ShouldNotBe(null);

            var ue =  user.MapTo<UserEditDto>();
            ue.Email = "dc01061@126.com";
            ue.UserName = "Test_Update";
            ue.Password = "123321";

            await _userAppService.UpdateUser(ue);

        }

        [Fact]
        public async Task Get_User_Test()
        {
            var tenant = await _tenantAppService.GetTenant("bcsint");
            AbpSession.TenantId = tenant.Id;

            var input = new GetUsersInput();
            input.Filter = "admin";
            input.MaxResultCount = 10;
            input.SkipCount = 0;

            var rs = await _userAppService.GetUsers(input);
            rs.Items.Count.ShouldBe(1);
            rs.TotalCount.ShouldBe(1);
        }
    }
}
