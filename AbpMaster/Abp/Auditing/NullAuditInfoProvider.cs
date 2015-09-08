namespace Abp.Auditing
{
    /// <summary>
    ///     Null implementation of <see cref="IAuditInfoProvider" />.
    /// </summary>
    internal class NullAuditInfoProvider : IAuditInfoProvider
    {
        public void Fill<TTenantId, TUserId>(AuditInfo<TTenantId, TUserId> auditInfo)
            where TTenantId : struct
            where TUserId : struct
        {
        }
    }
}