using System.Threading.Tasks;
using Abp.Runtime.Session;

namespace Abp.Authorization
{
    /// <summary>
    /// This class is used to permissions for users.
    /// </summary>
    public interface IPermissionChecker<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// 
        /// </summary>
        IAbpSession<TTenantId, TUserId> AbpSession { get; set; }

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        Task<bool> IsGrantedAsync(string permissionName);

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="userId">Id of user to check</param>
        /// <param name="permissionName">Name of the permission</param>
        Task<bool> IsGrantedAsync(TUserId userId, string permissionName);

        /// <summary>
        /// Checks if a user is granted for a module code and permission.
        /// </summary>
        /// <param name="moduleCode">Code of the menu or module</param>
        /// <param name="permissionName">Name of the permission</param>
        Task<bool> IsGrantedAsync(string moduleCode, string permissionName);
    }
}