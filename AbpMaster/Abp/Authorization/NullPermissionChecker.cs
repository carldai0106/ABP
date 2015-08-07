using System.Threading.Tasks;

namespace Abp.Authorization
{
    /// <summary>
    /// Null (and default) implementation of <see cref="IPermissionChecker{TUserId}"/>.
    /// </summary>
    public sealed class NullPermissionChecker<TUserId> : IPermissionChecker<TUserId> where TUserId : struct
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullPermissionChecker<TUserId> Instance { get { return SingletonInstance; } }
        private static readonly NullPermissionChecker<TUserId> SingletonInstance = new NullPermissionChecker<TUserId>();

        public Task<bool> IsGrantedAsync(string permissionName)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="userId">Id of the user to check</param>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns><c>true</c> if this instance is granted the specified userId permissionName; otherwise, <c>false</c>.</returns>
        public Task<bool> IsGrantedAsync(TUserId userId, string permissionName)
        {
            return Task.FromResult(true);
        }

        private NullPermissionChecker()
        {

        }

        /// <summary>
        /// Checks if a user is granted for a module code and permission.
        /// </summary>
        /// <param name="userId">Id of the user to check</param>
        /// <param name="moduleCode">Code of the menu or module</param>
        /// <param name="permissionName">Name of the permission</param>
        public Task<bool> IsGrantedAsync(TUserId userId, string moduleCode, string permissionName)
        {
            return Task.FromResult(true);
        }
    }
}