namespace Abp.Domain.Entities
{
    /// <summary>
    /// Implement this interface for an entity which may optionally have TenantId.
    /// </summary>
    public interface IMayHaveTenant<TTenantId> where TTenantId: struct
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        TTenantId? TenantId { get; set; }
    }
}