﻿using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Navigation;
using Shouldly;
using Xunit;

namespace Abp.Tests.Application.Navigation
{
    public class Menu_Tests : TestBaseWithLocalIocManager
    {
        [Fact]
        public async Task Test_Menu_System()
        {
            var testCase = new NavigationTestCase<int, long>();

            //Check created menu definitions
            var mainMenuDefinition = testCase.NavigationManager.MainMenu;
            mainMenuDefinition.Items.Count.ShouldBe(1);

            var adminMenuItemDefinition = mainMenuDefinition.GetItemByNameOrNull("Abp.Zero.Administration");
            adminMenuItemDefinition.ShouldNotBe(null);
            adminMenuItemDefinition.Items.Count.ShouldBe(3);
            
            //Check user menus
            var userMenu = await testCase.UserNavigationManager.GetMenuAsync(mainMenuDefinition.Name, 1);
            userMenu.Items.Count.ShouldBe(1);

            var userAdminMenu = userMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration");
            userAdminMenu.ShouldNotBe(null);

            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.User").ShouldNotBe(null);
            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.Role").ShouldBe(null);
            userAdminMenu.Items.FirstOrDefault(i => i.Name == "Abp.Zero.Administration.Setting").ShouldNotBe(null);
        }
    }
}
