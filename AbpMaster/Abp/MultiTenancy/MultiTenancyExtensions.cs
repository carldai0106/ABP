using Abp.Domain.Entities;

namespace Abp.MultiTenancy
{
    /// <summary>
    /// Extension methods for multi-tenancy.
    /// </summary>
    public static class MultiTenancyExtensions
    {
        /// <summary>
        /// Gets multi-tenancy side (<see cref="MultiTenancySides"/>) of an object that implements <see cref="IMayHaveTenant"/>.
        /// </summary>
        /// <param name="obj">The object</param>
        public static MultiTenancySides GetMultiTenancySide<TTenantId>(this IMayHaveTenant<TTenantId> obj)
            where TTenantId : struct
        {
            return obj.TenantId.HasValue
                ? MultiTenancySides.Tenant
                : MultiTenancySides.Host;
        }
    }
}