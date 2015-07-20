using Abp.MultiTenancy;

namespace Abp.Runtime.Session
{
    /// <summary>
    /// Defines some session information that can be useful for applications.
    /// </summary>
    public interface IAbpSession : IAbpSession<int, long>
    {
        
    }
}
