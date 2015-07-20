using System.Threading.Tasks;
using Castle.Core.Logging;

namespace Abp.Auditing
{
    /// <summary>
    /// Implements <see cref="IAuditingStore{TTenantId, TUserId}"/> to simply write audits to logs.
    /// </summary>
    public class SimpleLogAuditingStore<TTenantId, TUserId> : IAuditingStore<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static SimpleLogAuditingStore<TTenantId, TUserId> Instance { get { return SingletonInstance; } }
        private static readonly SimpleLogAuditingStore<TTenantId, TUserId> SingletonInstance = new SimpleLogAuditingStore<TTenantId, TUserId>();

        public ILogger Logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SimpleLogAuditingStore()
        {
            Logger = NullLogger.Instance;
        }

        public Task SaveAsync(AuditInfo<TTenantId, TUserId> auditInfo)
        {
            Logger.Info(auditInfo.ToString());
            return Task.FromResult(0);
        }
    }
}