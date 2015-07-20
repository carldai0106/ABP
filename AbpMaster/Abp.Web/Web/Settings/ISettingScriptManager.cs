using System.Threading.Tasks;

namespace Abp.Web.Settings
{
    /// <summary>
    /// Define interface to get setting scripts
    /// </summary>
    public interface ISettingScriptManager<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Gets Javascript that contains setting values.
        /// </summary>
        Task<string> GetScriptAsync();
    }
}