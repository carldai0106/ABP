using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.TestBase.Runtime.Session
{
    public class TestAbpSession<TTenantId, TUserId> : IAbpSession<TTenantId, TUserId>, ISingletonDependency
        where TTenantId : struct
        where TUserId : struct
    {
        public TUserId? UserId { get; set; }

        public TTenantId? TenantId
        {
            get
            {
                if (!_multiTenancy.IsEnabled)
                {
                    //modify by carl
                    return default(TTenantId);
                }

                return _tenantId;
            }
            set
            {
                if (!_multiTenancy.IsEnabled && !value.Equals(default(TTenantId)))
                {
                    throw new AbpException("Can not set TenantId since multi-tenancy is not enabled. Use IMultiTenancyConfig.IsEnabled to enable it.");
                }

                _tenantId = value;
            }
        }

        public MultiTenancySides MultiTenancySide { get { return GetCurrentMultiTenancySide(); } }

        private readonly IMultiTenancyConfig _multiTenancy;
        private TTenantId? _tenantId;

        public TestAbpSession(IMultiTenancyConfig multiTenancy)
        {
            _multiTenancy = multiTenancy;
        }

        private MultiTenancySides GetCurrentMultiTenancySide()
        {
            return _multiTenancy.IsEnabled && !TenantId.HasValue
                ? MultiTenancySides.Host
                : MultiTenancySides.Tenant;
        }
    }
}