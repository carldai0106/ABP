using System.Threading.Tasks;

namespace Abp.Web.Navigation
{
    /// <summary>
    /// Used to generate navigation scripts.
    /// </summary>
    public interface INavigationScriptManager<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Used to generate navigation scripts.
        /// </summary>
        /// <returns></returns>
        Task<string> GetScriptAsync();
    }
}
