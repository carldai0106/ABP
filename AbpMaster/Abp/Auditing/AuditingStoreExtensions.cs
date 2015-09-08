using Abp.Threading;

namespace Abp.Auditing
{
    /// <summary>
    ///     Extension methods for <see cref="IAuditingStore" />.
    /// </summary>
    public static class AuditingStoreExtensions
    {
        /// <summary>
        ///     Should save audits to a persistent store.
        /// </summary>
        /// <param name="auditingStore">Auditing store</param>
        /// <param name="auditInfo">Audit informations</param>
        public static void Save<TTenantId, TUserId>(this IAuditingStore<TTenantId, TUserId> auditingStore,
            AuditInfo<TTenantId, TUserId> auditInfo)
            where TTenantId : struct
            where TUserId : struct
        {
            AsyncHelper.RunSync(() => auditingStore.SaveAsync(auditInfo));
        }
    }
}