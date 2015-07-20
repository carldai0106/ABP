using System.Threading.Tasks;

namespace Abp.Web.Authorization
{
    /// <summary>
    /// This class is used to build and cache authorization script.
    /// </summary>
    public interface IAuthorizationScriptManager<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Gets Javascript that contains all authorization information.
        /// </summary>
        Task<string> GetScriptAsync();
    }
}
