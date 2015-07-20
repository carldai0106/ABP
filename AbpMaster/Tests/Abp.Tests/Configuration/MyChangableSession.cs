using Abp.MultiTenancy;
using Abp.Runtime.Session;

namespace Abp.Tests.Configuration
{
    public class MyChangableSession<TTenantId, TUserId> : IAbpSession<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct

    {
        public TUserId? UserId { get; set; }

        public TTenantId? TenantId { get; set; }

        public MultiTenancySides MultiTenancySide
        {
            get
            {
                return !TenantId.HasValue ? MultiTenancySides.Host : MultiTenancySides.Tenant;
            }
        }
    }
}