namespace Abp.Runtime.Session
{
    /// <summary>
    ///     Extension methods for <see cref="IAbpSession{TTenantId, TUserId}" />.
    /// </summary>
    public static class AbpSessionExtensions
    {
        /// <summary>
        ///     Gets current User's Id.
        ///     Throws <see cref="AbpException" /> if <see cref="IAbpSession{TTenantId, TUserId}.UserId" /> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current User's Id.</returns>
        public static TUserId GetUserId<TTenantId, TUserId>(this IAbpSession<TTenantId, TUserId> session)
            where TTenantId : struct
            where TUserId : struct
        {
            if (!session.UserId.HasValue)
            {
                throw new AbpException("Session.UserId is null! Probably, user is not logged in.");
            }

            return session.UserId.Value;
        }

        /// <summary>
        ///     Gets current Tenant's Id.
        ///     Throws <see cref="AbpException" /> if <see cref="IAbpSession{TTenantId, TUserId}.TenantId" /> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current Tenant's Id.</returns>
        /// <exception cref="AbpException"></exception>
        public static TTenantId GetTenantId<TTenantId, TUserId>(this IAbpSession<TTenantId, TUserId> session)
            where TTenantId : struct
            where TUserId : struct
        {
            if (!session.TenantId.HasValue)
            {
                throw new AbpException(
                    "Session.TenantId is null! Possible problems: User is not logged in, current user in not tenant user or this is not a multi-tenant application.");
            }

            return session.TenantId.Value;
        }
    }
}