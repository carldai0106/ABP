using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Threading;

namespace Abp.Authorization
{
    /// <summary>
    ///     Extension methods for <see cref="IPermissionChecker{TTenantId, TUserId}" />
    /// </summary>
    public static class PermissionCheckerExtensions
    {
        /// <summary>
        ///     Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="permissionName">Name of the permission</param>
        public static bool IsGranted<TTenantId, TUserId>(this IPermissionChecker<TTenantId, TUserId> permissionChecker,
            string permissionName)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(permissionName));
        }

        /// <summary>
        ///     Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="userId">Id of the user to check</param>
        /// <param name="permissionName">Name of the permission</param>
        public static bool IsGranted<TTenantId, TUserId>(this IPermissionChecker<TTenantId, TUserId> permissionChecker,
            TUserId userId, string permissionName)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(userId, permissionName));
        }

        /// <summary>
        ///     Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="moduleCode">code of the module or menu</param>
        /// <param name="permissionName">Name of the permission</param>
        public static bool IsGranted<TTenantId, TUserId>(this IPermissionChecker<TTenantId, TUserId> permissionChecker,
            string moduleCode, string permissionName)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(moduleCode, permissionName));
        }

        /// <summary>
        ///     Authorizes current user for given permission or permissions,
        ///     throws <see cref="AbpAuthorizationException" /> if not authorized.
        ///     User it authorized if any of the <see cref="permissionNames" /> are granted.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static void Authorize<TTenantId, TUserId>(this IPermissionChecker<TTenantId, TUserId> permissionChecker,
            params string[] permissionNames)
            where TTenantId : struct
            where TUserId : struct
        {
            Authorize(permissionChecker, false, permissionNames);
        }

        /// <summary>
        ///     Authorizes current user for given permission or permissions,
        ///     throws <see cref="AbpAuthorizationException" /> if not authorized.
        ///     User it authorized if any of the <see cref="permissionNames" /> are granted.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="requireAll">
        ///     If this is set to true, all of the <see cref="permissionNames" /> must be granted.
        ///     If it's false, at least one of the <see cref="permissionNames" /> must be granted.
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static void Authorize<TTenantId, TUserId>(this IPermissionChecker<TTenantId, TUserId> permissionChecker,
            bool requireAll, params string[] permissionNames)
            where TTenantId : struct
            where TUserId : struct
        {
            AsyncHelper.RunSync(() => AuthorizeAsync(permissionChecker, requireAll, permissionNames));
        }

        /// <summary>
        ///     Authorizes current user for given permission or permissions,
        ///     throws <see cref="AbpAuthorizationException" /> if not authorized.
        ///     User it authorized if any of the <see cref="permissionNames" /> are granted.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static Task AuthorizeAsync<TTenantId, TUserId>(
            this IPermissionChecker<TTenantId, TUserId> permissionChecker, params string[] permissionNames)
            where TTenantId : struct
            where TUserId : struct
        {
            return AuthorizeAsync(permissionChecker, false, permissionNames);
        }

        /// <summary>
        ///     Authorizes current user for given permission or permissions,
        ///     throws <see cref="AbpAuthorizationException" /> if not authorized.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="requireAll">
        ///     If this is set to true, all of the <see cref="permissionNames" /> must be granted.
        ///     If it's false, at least one of the <see cref="permissionNames" /> must be granted.
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static async Task AuthorizeAsync<TTenantId, TUserId>(
            this IPermissionChecker<TTenantId, TUserId> permissionChecker, bool requireAll,
            params string[] permissionNames)
            where TTenantId : struct
            where TUserId : struct
        {
            if (permissionNames.IsNullOrEmpty())
            {
                return;
            }

            if (requireAll)
            {
                foreach (var permissionName in permissionNames)
                {
                    if (!(await permissionChecker.IsGrantedAsync(permissionName)))
                    {
                        throw new AbpAuthorizationException(
                            "Required permissions are not granted. All of these permissions must be granted: " +
                            string.Join(", ", permissionNames)
                            );
                    }
                }
            }
            else
            {
                foreach (var permissionName in permissionNames)
                {
                    if (await permissionChecker.IsGrantedAsync(permissionName))
                    {
                        return;
                    }
                }

                throw new AbpAuthorizationException(
                    "Required permissions are not granted. At least one of these permissions must be granted: " +
                    string.Join(", ", permissionNames)
                    );
            }
        }

        /// <summary>
        ///     Authorizes current user for given permission or permissions,
        ///     throws <see cref="AbpAuthorizationException" /> if not authorized.
        /// </summary>
        /// <typeparam name="TUserId">The type of UserId</typeparam>
        /// <typeparam name="TTenantId">The type of TenantId</typeparam>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="abpAuthorizeAttribute">Abp authorization attributes.</param>
        /// <returns></returns>
        public static async Task AuthorizeAsync<TTenantId, TUserId>(
            this IPermissionChecker<TTenantId, TUserId> permissionChecker,
            IAbpAuthorizeAttribute abpAuthorizeAttribute)
            where TTenantId : struct
            where TUserId : struct
        {
            var moduleCode = abpAuthorizeAttribute.ModuleCode;
            var permissions = abpAuthorizeAttribute.Permissions;
            var requireAll = abpAuthorizeAttribute.RequireAllPermissions;

            if (permissions.IsNullOrEmpty())
            {
                return;
            }

            if (string.IsNullOrEmpty(moduleCode))
            {
                await permissionChecker.AuthorizeAsync(requireAll, permissions);
                return;
            }

            if (requireAll)
            {
                foreach (var permissionName in permissions)
                {
                    if (!(await permissionChecker.IsGrantedAsync(moduleCode, permissionName)))
                    {
                        throw new AbpAuthorizationException(
                            "Required permissions of " + moduleCode +
                            " are not granted. All of these permissions must be granted: " +
                            string.Join(", ", permissions)
                            );
                    }
                }
            }
            else
            {
                foreach (var permissionName in permissions)
                {
                    if (await permissionChecker.IsGrantedAsync(moduleCode, permissionName))
                    {
                        return;
                    }
                }

                throw new AbpAuthorizationException(
                    "Required permissions of " + moduleCode +
                    " are not granted. At least one of these permissions must be granted: " +
                    string.Join(", ", permissions)
                    );
            }
        }
    }
}