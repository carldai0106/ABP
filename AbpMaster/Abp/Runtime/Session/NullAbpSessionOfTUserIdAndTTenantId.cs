using Abp.MultiTenancy;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Implements null object pattern for <see cref="IAbpSession"/>.
    /// </summary>
    public class NullAbpSession<TTenantId, TUserId> : IAbpSession<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullAbpSession<TTenantId, TUserId> Instance { get { return SingletonInstance; } }
        private static readonly NullAbpSession<TTenantId, TUserId> SingletonInstance = new NullAbpSession<TTenantId, TUserId>();

        /// <inheritdoc/>
        public TUserId? UserId
        {
            get
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public TTenantId? TenantId { get { return null; } }

        public MultiTenancySides MultiTenancySide { get { return MultiTenancySides.Tenant; } }

        private NullAbpSession()
        {

        }
    }
}