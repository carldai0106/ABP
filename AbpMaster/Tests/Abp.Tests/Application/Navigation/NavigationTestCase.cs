using System.Threading.Tasks;
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Localization;
using NSubstitute;

namespace Abp.Tests.Application.Navigation
{
    internal class NavigationTestCase<TUserId>
        where TUserId : struct
    {
        public NavigationManager NavigationManager { get; private set; }

        public IUserNavigationManager<TUserId> UserNavigationManager { get; private set; }

        private readonly IIocManager _iocManager;

        private static TUserId UserId { get; set; }

        public NavigationTestCase(TUserId userId)
            : this(new IocManager(), userId)
        {
           
        }

        public NavigationTestCase(IIocManager iocManager, TUserId userId)
        {
            _iocManager = iocManager;
            UserId = userId;
            Initialize();
        }

        private void Initialize()
        {
            //Navigation providers should be registered
            _iocManager.Register<MyNavigationProvider1>();
            _iocManager.Register<MyNavigationProvider2>();

            //Preparing navigation configuration
            var configuration = new NavigationConfiguration();
            configuration.Providers.Add<MyNavigationProvider1>();
            configuration.Providers.Add<MyNavigationProvider2>();

            //Initializing navigation manager
            NavigationManager = new NavigationManager(_iocManager, configuration);
            NavigationManager.Initialize();
            
            //Create user navigation manager to test
            UserNavigationManager = new UserNavigationManager<TUserId>(NavigationManager)
            {
                PermissionChecker = CreateMockPermissionChecker(UserId)
            };
        }

        private static IPermissionChecker<TUserId> CreateMockPermissionChecker(TUserId userId)
        {
            var permissionChecker = Substitute.For<IPermissionChecker<TUserId>>();
            permissionChecker.IsGrantedAsync(userId, "Abp.Zero.UserManagement").Returns(Task.FromResult(true));
            permissionChecker.IsGrantedAsync(userId, "Abp.Zero.RoleManagement").Returns(Task.FromResult(false));
            return permissionChecker;
        }

        public class MyNavigationProvider1 : NavigationProvider
        {
            public override void SetNavigation(INavigationProviderContext context)
            {
                context.Manager.MainMenu.AddItem(
                    new MenuItemDefinition(
                        "Abp.Zero.Administration",
                        new FixedLocalizableString("Administration"),
                        "fa fa-asterisk",
                        requiresAuthentication: true
                        ).AddItem(
                            new MenuItemDefinition(
                                "Abp.Zero.Administration.User",
                                new FixedLocalizableString("User management"),
                                "fa fa-users",
                                "#/admin/users",
                                requiredPermissionName: "Abp.Zero.UserManagement",
                                customData: "A simple test data"
                                )
                        ).AddItem(
                            new MenuItemDefinition(
                                "Abp.Zero.Administration.Role",
                                new FixedLocalizableString("Role management"),
                                "fa fa-star-o",
                                "#/admin/roles",
                                requiredPermissionName: "Abp.Zero.RoleManagement"
                                )
                        )
                    );
            }
        }

        public class MyNavigationProvider2 : NavigationProvider
        {
            public override void SetNavigation(INavigationProviderContext context)
            {
                var adminMenu = context.Manager.MainMenu.GetItemByName("Abp.Zero.Administration");
                adminMenu.AddItem(
                    new MenuItemDefinition(
                        "Abp.Zero.Administration.Setting",
                        new FixedLocalizableString("Setting management"),
                        icon: "fa fa-cog",
                        url: "#/admin/settings",
                        customData: new MyCustomDataClass { Data1 = 42, Data2 = "FortyTwo" }
                        )
                    );
            }
        }

        public class MyCustomDataClass
        {
            public int Data1 { get; set; }

            public string Data2 { get; set; }
        }
    }
}