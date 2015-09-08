using Abp.MultiTenancy;

namespace Abp.Runtime.Session
{
    /// <summary>
    ///     Implements null object pattern for <see cref="IAbpSession{TTenantId, TUserId}" />.
    /// </summary>
    public class NullAbpSession<TTenantId, TUserId> : IAbpSession<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        private static readonly NullAbpSession<TTenantId, TUserId> SingletonInstance =
            new NullAbpSession<TTenantId, TUserId>();

        private NullAbpSession()
        {
        }

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        public static NullAbpSession<TTenantId, TUserId> Instance
        {
            get { return SingletonInstance; }
        }

        /// <inheritdoc />
        public TUserId? UserId
        {
            get { return null; }
        }

        /// <inheritdoc />
        public TTenantId? TenantId
        {
            get { return null; }
        }

        public MultiTenancySides MultiTenancySide
        {
            get { return MultiTenancySides.Tenant; }
        }
    }
}