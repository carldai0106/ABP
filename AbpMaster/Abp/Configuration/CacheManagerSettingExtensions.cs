using System.Collections.Generic;
using Abp.Runtime.Caching;

namespace Abp.Configuration
{
    /// <summary>
    ///     Extension methods for <see cref="ICacheManager" /> to get setting caches.
    /// </summary>
    public static class CacheManagerSettingExtensions
    {
        /// <summary>
        ///     Gets application settings cache.
        /// </summary>
        public static ITypedCache<string, Dictionary<string, SettingInfo<TTenantId, TUserId>>>
            GetApplicationSettingsCache<TTenantId, TUserId>(this ICacheManager cacheManager)
            where TTenantId : struct
            where TUserId : struct
        {
            return cacheManager
                .GetCache(SettingManager<TTenantId, TUserId>.ApplicationSettingsCacheName)
                .AsTyped<string, Dictionary<string, SettingInfo<TTenantId, TUserId>>>();
        }

        /// <summary>
        ///     Gets tenant settings cache.
        /// </summary>
        public static ITypedCache<TTenantId, Dictionary<string, SettingInfo<TTenantId, TUserId>>> GetTenantSettingsCache
            <TTenantId, TUserId>(this ICacheManager cacheManager)
            where TTenantId : struct
            where TUserId : struct
        {
            return cacheManager
                .GetCache(SettingManager<TTenantId, TUserId>.TenantSettingsCacheName)
                .AsTyped<TTenantId, Dictionary<string, SettingInfo<TTenantId, TUserId>>>();
        }

        /// <summary>
        ///     Gets user settings cache.
        /// </summary>
        public static ITypedCache<TUserId, Dictionary<string, SettingInfo<TTenantId, TUserId>>> GetUserSettingsCache
            <TTenantId, TUserId>(this ICacheManager cacheManager)
            where TTenantId : struct
            where TUserId : struct
        {
            return cacheManager
                .GetCache(SettingManager<TTenantId, TUserId>.UsersSettingsCacheName)
                .AsTyped<TUserId, Dictionary<string, SettingInfo<TTenantId, TUserId>>>();
        }
    }
}