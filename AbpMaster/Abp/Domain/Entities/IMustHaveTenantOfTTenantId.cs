namespace Abp.Domain.Entities
{
    /// <summary>
    /// Implement this interface for an entity which must have TenantId.
    /// </summary>
    public interface IMustHaveTenant<TTenantId> where TTenantId : struct
    {
        /// <summary>
        /// TenantId of this entity.
        /// </summary>
        TTenantId TenantId { get; set; }
    }
}