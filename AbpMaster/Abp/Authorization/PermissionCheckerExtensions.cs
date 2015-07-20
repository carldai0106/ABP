using System;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Threading;

namespace Abp.Authorization
{
    /// <summary>
    /// Extension methods for <see cref="IPermissionChecker{TUserId}"/>
    /// </summary>
    public static class PermissionCheckerExtensions
    {
        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="permissionName">Name of the permission</param>
        public static bool IsGranted<TUserId>(this IPermissionChecker<TUserId> permissionChecker, string permissionName)
            where TUserId : struct
        { 
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(permissionName));
        }

        /// <summary>
        /// Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="userId">Id of the user to check</param>
        /// <param name="permissionName">Name of the permission</param>
        public static bool IsGranted<TUserId>(this IPermissionChecker<TUserId> permissionChecker, TUserId userId, string permissionName)
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => permissionChecker.IsGrantedAsync(userId, permissionName));
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,
        /// throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// User it authorized if any of the <see cref="permissionNames"/> are granted.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static void Authorize<TUserId>(this IPermissionChecker<TUserId> permissionChecker, params string[] permissionNames)
            where TUserId : struct
        {
            Authorize(permissionChecker, false, permissionNames);
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,
        /// throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// User it authorized if any of the <see cref="permissionNames"/> are granted.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="requireAll">
        /// If this is set to true, all of the <see cref="permissionNames"/> must be granted.
        /// If it's false, at least one of the <see cref="permissionNames"/> must be granted.
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static void Authorize<TUserId>(this IPermissionChecker<TUserId> permissionChecker, bool requireAll, params string[] permissionNames)
            where TUserId : struct
        {
            AsyncHelper.RunSync(() => AuthorizeAsync(permissionChecker, requireAll, permissionNames));
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,
        /// throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// User it authorized if any of the <see cref="permissionNames"/> are granted.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static Task AuthorizeAsync<TUserId>(this IPermissionChecker<TUserId> permissionChecker, params string[] permissionNames)
            where TUserId : struct
        {
            return AuthorizeAsync(permissionChecker, false, permissionNames);
        }

        /// <summary>
        /// Authorizes current user for given permission or permissions,
        /// throws <see cref="AbpAuthorizationException"/> if not authorized.
        /// </summary>
        /// <param name="permissionChecker">Permission checker</param>
        /// <param name="requireAll">
        /// If this is set to true, all of the <see cref="permissionNames"/> must be granted.
        /// If it's false, at least one of the <see cref="permissionNames"/> must be granted.
        /// </param>
        /// <param name="permissionNames">Name of the permissions to authorize</param>
        /// <exception cref="AbpAuthorizationException">Throws authorization exception if</exception>
        public static async Task AuthorizeAsync<TUserId>(this IPermissionChecker<TUserId> permissionChecker, bool requireAll, params string[] permissionNames)
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
                            String.Join(", ", permissionNames)
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
                    String.Join(", ", permissionNames)
                    );
            }
        }
    }
}