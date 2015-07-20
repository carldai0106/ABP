using Abp.MultiTenancy;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Defines some session information that can be useful for applications.
    /// </summary>
    public interface IAbpSession<TTenantId, TTUserId>
        where TTenantId : struct
        where TTUserId : struct
    {
        /// <summary>
        /// Gets current UserId or null.
        /// </summary>
        TTUserId? UserId { get; }

        /// <summary>
        /// Gets current TenantId or null.
        /// </summary>
        TTenantId? TenantId { get; }

        /// <summary>
        /// Gets current multi-tenancy side.
        /// </summary>
        MultiTenancySides MultiTenancySide { get; }
    }
}
