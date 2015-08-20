using Abp.Localization;

namespace Abp.Web.Api.Tests
{
    public static class AbpWebApiTests<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        private static AbpBootstrapper<TTenantId, TUserId> _bootstrapper;

        public static void Initialize()
        {
            if (_bootstrapper != null)
            {
                return;
            }

            _bootstrapper = new AbpBootstrapper<TTenantId, TUserId>();
            _bootstrapper.Initialize();
        }
    }
}