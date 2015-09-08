using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dependency;

namespace Abp.Application.Navigation
{
    internal class UserNavigationManager<TTenantId, TUserId> : IUserNavigationManager<TTenantId, TUserId>,
        ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        private readonly INavigationManager _navigationManager;

        public UserNavigationManager(INavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
            PermissionChecker = NullPermissionChecker<TTenantId, TUserId>.Instance;
        }

        public IPermissionChecker<TTenantId, TUserId> PermissionChecker { get; set; }

        public async Task<UserMenu> GetMenuAsync(string menuName, TUserId? userId)
        {
            var menuDefinition = _navigationManager.Menus.GetOrDefault(menuName);
            if (menuDefinition == null)
            {
                throw new AbpException("There is no menu with given name: " + menuName);
            }

            var userMenu = new UserMenu(menuDefinition);
            await FillUserMenuItems(userId, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }

        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync(TUserId? userId)
        {
            var userMenus = new List<UserMenu>();

            foreach (var menu in _navigationManager.Menus.Values)
            {
                userMenus.Add(await GetMenuAsync(menu.Name, userId));
            }

            return userMenus;
        }

        private async Task<int> FillUserMenuItems(TUserId? userId, IList<MenuItemDefinition> menuItemDefinitions,
            IList<UserMenuItem> userMenuItems)
        {
            var addedMenuItemCount = 0;

            foreach (var menuItemDefinition in menuItemDefinitions)
            {
                if (menuItemDefinition.RequiresAuthentication && !userId.HasValue)
                {
                    continue;
                }

                var requiredPermissionName = menuItemDefinition.RequiredPermissionName;
                //var rs = await PermissionChecker.IsGrantedAsync(userId.Value, requiredPermissionName);

                if (!string.IsNullOrEmpty(requiredPermissionName) &&
                    (!userId.HasValue ||
                     !(await PermissionChecker.IsGrantedAsync(userId.Value, menuItemDefinition.RequiredPermissionName))))
                {
                    continue;
                }

                var userMenuItem = new UserMenuItem(menuItemDefinition);
                if (menuItemDefinition.IsLeaf ||
                    (await FillUserMenuItems(userId, menuItemDefinition.Items, userMenuItem.Items)) > 0)
                {
                    userMenuItems.Add(userMenuItem);
                    ++addedMenuItemCount;
                }
            }

            return addedMenuItemCount;
        }
    }
}