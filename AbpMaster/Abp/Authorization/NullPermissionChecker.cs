using System.Threading.Tasks;
using Abp.Runtime.Session;

namespace Abp.Authorization
{
    /// <summary>
    ///     Null (and default) implementation of <see cref="IPermissionChecker{TTenantId, TUserId}" />.
    /// </summary>
    public sealed class NullPermissionChecker<TTenantId, TUserId> : IPermissionChecker<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        private static readonly NullPermissionChecker<TTenantId, TUserId> SingletonInstance =
            new NullPermissionChecker<TTenantId, TUserId>();

        private NullPermissionChecker()
        {
        }

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        public static NullPermissionChecker<TTenantId, TUserId> Instance
        {
            get { return SingletonInstance; }
        }

        public IAbpSession<TTenantId, TUserId> AbpSession { get; set; }

        /// <summary>
        ///     Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        /// <returns><c>true</c> if this instance is granted the specified userId permissionName; otherwise, <c>false</c>.</returns>
        public Task<bool> IsGrantedAsync(string permissionName)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="userId">Id of user to check</param>
        /// <param name="permissionName">Name of the permission</param>
        public Task<bool> IsGrantedAsync(TUserId userId, string permissionName)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        ///     Checks if a user is granted for a module code and permission.
        /// </summary>
        /// <param name="moduleCode">Code of the menu or module</param>
        /// <param name="permissionName">Name of the permission</param>
        public Task<bool> IsGrantedAsync(string moduleCode, string permissionName)
        {
            return Task.FromResult(true);
        }
    }
}