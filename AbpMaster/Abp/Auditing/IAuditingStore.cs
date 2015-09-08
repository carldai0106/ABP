using System.Threading.Tasks;

namespace Abp.Auditing
{
    /// <summary>
    ///     This interface should be implemented by vendors to
    ///     make auditing working.
    ///     Default implementation is <see cref="SimpleLogAuditingStore{TTenantId, TUserId}" />.
    /// </summary>
    public interface IAuditingStore<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        ///     Should save audits to a persistent store.
        /// </summary>
        /// <param name="auditInfo">Audit informations</param>
        Task SaveAsync(AuditInfo<TTenantId, TUserId> auditInfo);
    }
}