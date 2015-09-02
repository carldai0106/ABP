using System.Threading.Tasks;
using Abp.Tests.Application.Navigation;
using Abp.Tests.Configuration;
using Abp.Web.Navigation;
using Shouldly;
using Xunit;

namespace Abp.Web.Tests.Navigation
{
    public class NavigationScript_Tests
    {
        [Fact]
        public async Task Should_Get_Script()
        {
            var testCase = new NavigationTestCase<int, long>();
            var scriptManager = new NavigationScriptManager<int, long>(testCase.UserNavigationManager)
            {
                AbpSession = new MyChangableSession<int, long> { UserId = 1 }
            };

            var script = await scriptManager.GetScriptAsync();
            script.ShouldNotBeNullOrEmpty();
        }
    }
}
