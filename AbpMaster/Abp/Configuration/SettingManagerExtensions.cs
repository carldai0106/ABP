using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Extensions;
using Abp.Threading;

namespace Abp.Configuration
{
    /// <summary>
    ///     Extension methods for <see cref="ISettingManager" />.
    /// </summary>
    public static class SettingManagerExtensions
    {
        /// <summary>
        ///     Gets value of a setting in given type (<see cref="T" />).
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Value of the setting</returns>
        public static async Task<T> GetSettingValueAsync<T, TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name)
            where T : struct
            where TTenantId : struct
            where TUserId : struct
        {
            return (await settingManager.GetSettingValueAsync(name)).To<T>();
        }

        /// <summary>
        ///     Gets current value of a setting for the application level.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting for the application</returns>
        public static async Task<T> GetSettingValueForApplicationAsync<T, TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name)
            where T : struct
            where TTenantId : struct
            where TUserId : struct
        {
            return (await settingManager.GetSettingValueForApplicationAsync(name)).To<T>();
        }

        /// <summary>
        ///     Gets current value of a setting for a tenant level.
        ///     It gets the setting value, overwritten by given tenant.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="tenantId">Tenant id</param>
        /// <returns>Current value of the setting</returns>
        public static async Task<T> GetSettingValueForTenantAsync<T, TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name, TTenantId tenantId)
            where T : struct
            where TTenantId : struct
            where TUserId : struct
        {
            return (await settingManager.GetSettingValueForTenantAsync(name, tenantId)).To<T>();
        }

        /// <summary>
        ///     Gets current value of a setting for a user level.
        ///     It gets the setting value, overwritten by given tenant and user.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="tenantId">Tenant id</param>
        /// <param name="userId">User id</param>
        /// <returns>Current value of the setting for the user</returns>
        public static async Task<T> GetSettingValueForUserAsync<T, TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name, TTenantId tenantId, TUserId userId)
            where T : struct
            where TTenantId : struct
            where TUserId : struct
        {
            return (await settingManager.GetSettingValueForUserAsync(name, tenantId, userId)).To<T>();
        }

        /// <summary>
        ///     Gets current value of a setting.
        ///     It gets the setting value, overwritten by application and the current user if exists.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting</returns>
        public static string GetSettingValue<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueAsync(name));
        }

        /// <summary>
        ///     Gets current value of a setting for the application level.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting for the application</returns>
        public static string GetSettingValueForApplication<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForApplicationAsync(name));
        }

        /// <summary>
        ///     Gets current value of a setting for a tenant level.
        ///     It gets the setting value, overwritten by given tenant.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="tenantId">Tenant id</param>
        /// <returns>Current value of the setting</returns>
        public static string GetSettingValueForTenant<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name, TTenantId tenantId)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForTenantAsync(name, tenantId));
        }

        /// <summary>
        ///     Gets current value of a setting for a user level.
        ///     It gets the setting value, overwritten by given tenant and user.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="tenantId">Tenant id</param>
        /// <param name="userId">User id</param>
        /// <returns>Current value of the setting for the user</returns>
        public static string GetSettingValueForUser<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name, TTenantId tenantId, TUserId userId)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueForUserAsync(name, tenantId, userId));
        }

        /// <summary>
        ///     Gets value of a setting.
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Value of the setting</returns>
        public static T GetSettingValue<T, TTenantId, TUserId>(this ISettingManager<TTenantId, TUserId> settingManager,
            string name)
            where T : struct
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetSettingValueAsync<T, TTenantId, TUserId>(name));
        }

        /// <summary>
        ///     Gets current value of a setting for the application level.
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting for the application</returns>
        public static T GetSettingValueForApplication<T, TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name)
            where T : struct
            where TTenantId : struct
            where TUserId : struct
        {
            return
                AsyncHelper.RunSync(() => settingManager.GetSettingValueForApplicationAsync<T, TTenantId, TUserId>(name));
        }

        /// <summary>
        ///     Gets current value of a setting for a tenant level.
        ///     It gets the setting value, overwritten by given tenant.
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="tenantId">Tenant id</param>
        /// <returns>Current value of the setting</returns>
        public static T GetSettingValueForTenant<T, TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name, TTenantId tenantId)
            where T : struct
            where TTenantId : struct
            where TUserId : struct
        {
            return
                AsyncHelper.RunSync(
                    () => settingManager.GetSettingValueForTenantAsync<T, TTenantId, TUserId>(name, tenantId));
        }

        /// <summary>
        ///     Gets current value of a setting for a user level.
        ///     It gets the setting value, overwritten by given tenant and user.
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="tenantId">Tenant id</param>
        /// <param name="userId">User id</param>
        /// <returns>Current value of the setting for the user</returns>
        public static T GetSettingValueForUser<T, TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name, TTenantId tenantId, TUserId userId)
            where T : struct
            where TTenantId : struct
            where TUserId : struct
        {
            return
                AsyncHelper.RunSync(
                    () => settingManager.GetSettingValueForUserAsync<T, TTenantId, TUserId>(name, tenantId, userId));
        }

        /// <summary>
        ///     Gets current values of all settings.
        ///     It gets all setting values, overwritten by application and the current user if exists.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <returns>List of setting values</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValues<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(settingManager.GetAllSettingValuesAsync);
        }

        /// <summary>
        ///     Gets a list of all setting values specified for the application.
        ///     It returns only settings those are explicitly set for the application.
        ///     If a setting's default value is used, it's not included the result list.
        ///     If you want to get current values of all settings, use <see cref="GetAllSettingValues" /> method.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <returns>List of setting values</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValuesForApplication<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(settingManager.GetAllSettingValuesForApplicationAsync);
        }

        /// <summary>
        ///     Gets a list of all setting values specified for a tenant.
        ///     It returns only settings those are explicitly set for the tenant.
        ///     If a setting's default value is used, it's not included the result list.
        ///     If you want to get current values of all settings, use <see cref="GetAllSettingValues" /> method.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="tenantId">Tenant to get settings</param>
        /// <returns>List of setting values</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValuesForTenant<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, TTenantId tenantId)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetAllSettingValuesForTenantAsync(tenantId));
        }

        /// <summary>
        ///     Gets a list of all setting values specified for a user.
        ///     It returns only settings those are explicitly set for the user.
        ///     If a setting's value is not set for the user (for example if user uses the default value), it's not included the
        ///     result list.
        ///     If you want to get current values of all settings, use <see cref="GetAllSettingValues" /> method.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="userId">User to get settings</param>
        /// <returns>All settings of the user</returns>
        public static IReadOnlyList<ISettingValue> GetAllSettingValuesForTenant<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, TUserId userId)
            where TTenantId : struct
            where TUserId : struct
        {
            return AsyncHelper.RunSync(() => settingManager.GetAllSettingValuesForUserAsync(userId));
        }

        /// <summary>
        ///     Changes setting for the application level.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public static void ChangeSettingForApplication<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, string name, string value)
            where TTenantId : struct
            where TUserId : struct
        {
            AsyncHelper.RunSync(() => settingManager.ChangeSettingForApplicationAsync(name, value));
        }

        /// <summary>
        ///     Changes setting for a Tenant.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="tenantId">TenantId</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public static void ChangeSettingForTenant<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, TTenantId tenantId, string name, string value)
            where TTenantId : struct
            where TUserId : struct
        {
            AsyncHelper.RunSync(() => settingManager.ChangeSettingForTenantAsync(tenantId, name, value));
        }

        /// <summary>
        ///     Changes setting for a user.
        /// </summary>
        /// <param name="settingManager">Setting manager</param>
        /// <param name="userId">UserId</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        public static void ChangeSettingForUser<TTenantId, TUserId>(
            this ISettingManager<TTenantId, TUserId> settingManager, TUserId userId, string name, string value)
            where TTenantId : struct
            where TUserId : struct
        {
            AsyncHelper.RunSync(() => settingManager.ChangeSettingForUserAsync(userId, name, value));
        }
    }
}