namespace Abp.Domain.Entities
{
    /// <summary>
    ///     Implement this interface for an entity which must have TenantId.
    /// </summary>
    public interface IMustHaveTenant : IMustHaveTenant<int>
    {
    }
}