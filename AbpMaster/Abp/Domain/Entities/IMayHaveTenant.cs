namespace Abp.Domain.Entities
{
    /// <summary>
    ///     Implement this interface for an entity which may optionally have TenantId.
    /// </summary>
    public interface IMayHaveTenant : IMayHaveTenant<int>
    {
    }
}