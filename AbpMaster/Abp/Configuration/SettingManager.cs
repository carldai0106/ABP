﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;

namespace Abp.Configuration
{
    /// <summary>
    ///     This class implements <see cref="ISettingManager{TTenantId, TUserId}" /> to manage setting values in the database.
    /// </summary>
    public class SettingManager<TTenantId, TUserId> : ISettingManager<TTenantId, TUserId>, ISingletonDependency
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        ///     Cache key for application settings.
        /// </summary>
        public const string ApplicationSettingsCacheKey = "ApplicationSettings";

        /// <summary>
        ///     Unique name of the application settings cache: AbpApplicationSettingsCache.
        /// </summary>
        public const string ApplicationSettingsCacheName = "AbpApplicationSettingsCache";

        /// <summary>
        ///     Unique name of the tenant settings cache: AbpApplicationSettingsCache.
        /// </summary>
        public const string TenantSettingsCacheName = "AbpTenantSettingsCache";

        /// <summary>
        ///     Unique name of the user settings cache: AbpApplicationSettingsCache.
        /// </summary>
        public const string UsersSettingsCacheName = "AbpUserSettingsCache";

        /// <summary>
        ///     Reference to the current Session.
        /// </summary>
        public IAbpSession<TTenantId, TUserId> AbpSession { get; set; }

        /// <summary>
        ///     Reference to the setting store.
        /// </summary>
        public ISettingStore<TTenantId, TUserId> SettingStore { get; set; }

        private readonly ISettingDefinitionManager _settingDefinitionManager;
 		private readonly ITypedCache<string, Dictionary<string, SettingInfo<TTenantId, TUserId>>> _applicationSettingCache;
        private readonly ITypedCache<TTenantId, Dictionary<string, SettingInfo<TTenantId, TUserId>>> _tenantSettingCache;
        private readonly ITypedCache<TUserId, Dictionary<string, SettingInfo<TTenantId, TUserId>>> _userSettingCache;

        /// <inheritdoc />
        public SettingManager(ISettingDefinitionManager settingDefinitionManager, ICacheManager cacheManager)
        {
            _settingDefinitionManager = settingDefinitionManager;

            AbpSession = NullAbpSession<TTenantId, TUserId>.Instance;
            SettingStore = DefaultConfigSettingStore<TTenantId, TUserId>.Instance;

            _applicationSettingCache = cacheManager.GetApplicationSettingsCache<TTenantId, TUserId>();
            _tenantSettingCache = cacheManager.GetTenantSettingsCache<TTenantId, TUserId>();
            _userSettingCache = cacheManager.GetUserSettingsCache<TTenantId, TUserId>();
        }

        #region Public methods

        /// <inheritdoc />
        public Task<string> GetSettingValueAsync(string name)
        {
            return GetSettingValueInternalAsync(name, AbpSession.TenantId, AbpSession.UserId);
        }

        public Task<string> GetSettingValueForApplicationAsync(string name)
        {
            return GetSettingValueInternalAsync(name);
        }

        public Task<string> GetSettingValueForTenantAsync(string name, TTenantId tenantId)
        {
            return GetSettingValueInternalAsync(name, tenantId);
        }

        public Task<string> GetSettingValueForUserAsync(string name, TTenantId? tenantId, TUserId userId)
        {
            return GetSettingValueInternalAsync(name, tenantId, userId);
        }

        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync()
        {
            return await GetAllSettingValuesAsync(SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User);
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync(SettingScopes scopes)
        {
            var settingDefinitions = new Dictionary<string, SettingDefinition>();
            var settingValues = new Dictionary<string, ISettingValue>();

            //Fill all setting with default values.
            foreach (var setting in _settingDefinitionManager.GetAllSettingDefinitions())
            {
                settingDefinitions[setting.Name] = setting;
                settingValues[setting.Name] = new SettingValueObject(setting.Name, setting.DefaultValue);
            }

            //Overwrite application settings
            if (scopes.HasFlag(SettingScopes.Application))
            {
                foreach (var settingValue in await GetAllSettingValuesForApplicationAsync())
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);

                    //TODO: Conditions get complicated, try to simplify it
                    if (setting == null || !setting.Scopes.HasFlag(SettingScopes.Application))
                    {
                        continue;
                    }

                    if (!setting.IsInherited &&
                        ((setting.Scopes.HasFlag(SettingScopes.Tenant) && AbpSession.TenantId.HasValue) ||
                         (setting.Scopes.HasFlag(SettingScopes.User) && AbpSession.UserId.HasValue)))
                    {
                        continue;
                    }

                    settingValues[settingValue.Name] = new SettingValueObject(settingValue.Name, settingValue.Value);
                }
            }

            //Overwrite tenant settings
            if (scopes.HasFlag(SettingScopes.Tenant) && AbpSession.TenantId.HasValue)
            {
                foreach (var settingValue in await GetAllSettingValuesForTenantAsync(AbpSession.TenantId.Value))
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);

                    //TODO: Conditions get complicated, try to simplify it
                    if (setting == null || !setting.Scopes.HasFlag(SettingScopes.Tenant))
                    {
                        continue;
                    }

                    if (!setting.IsInherited &&
                        (setting.Scopes.HasFlag(SettingScopes.User) && AbpSession.UserId.HasValue))
                    {
                        continue;
                    }

                    settingValues[settingValue.Name] = new SettingValueObject(settingValue.Name, settingValue.Value);
                }
            }

            //Overwrite user settings
            if (scopes.HasFlag(SettingScopes.User) && AbpSession.UserId.HasValue)
            {
                foreach (var settingValue in await GetAllSettingValuesForUserAsync(AbpSession.UserId.Value))
                {
                    var setting = settingDefinitions.GetOrDefault(settingValue.Name);
                    if (setting != null && setting.Scopes.HasFlag(SettingScopes.User))
                    {
                        settingValues[settingValue.Name] = new SettingValueObject(settingValue.Name, settingValue.Value);
                    }
                }
            }

            return settingValues.Values.ToImmutableList();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForApplicationAsync()
        {
            return (await GetApplicationSettingsAsync()).Values
                .Select(setting => new SettingValueObject(setting.Name, setting.Value))
                .ToImmutableList();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForTenantAsync(TTenantId tenantId)
        {
            return (await GetReadOnlyTenantSettings(tenantId)).Values
                .Select(setting => new SettingValueObject(setting.Name, setting.Value))
                .ToImmutableList();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForUserAsync(TUserId userId)
        {
            return (await GetReadOnlyUserSettings(userId)).Values
                .Select(setting => new SettingValueObject(setting.Name, setting.Value))
                .ToImmutableList();
        }

        /// <inheritdoc />
        [UnitOfWork]
        public virtual async Task ChangeSettingForApplicationAsync(string name, string value)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, null, null);
            await _applicationSettingCache.RemoveAsync(ApplicationSettingsCacheKey);
        }

        /// <inheritdoc />
        [UnitOfWork]
        public virtual async Task ChangeSettingForTenantAsync(TTenantId tenantId, string name, string value)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, tenantId, null);
            await _tenantSettingCache.RemoveAsync(tenantId);
        }

        /// <inheritdoc />
        [UnitOfWork]
        public virtual async Task ChangeSettingForUserAsync(TUserId userId, string name, string value)
        {
            await InsertOrUpdateOrDeleteSettingValueAsync(name, value, null, userId);
            await _userSettingCache.RemoveAsync(userId);
        }

        #endregion

        #region Private methods

        private async Task<string> GetSettingValueInternalAsync(string name, TTenantId? tenantId = null,
            TUserId? userId = null)
        {
            var settingDefinition = _settingDefinitionManager.GetSettingDefinition(name);

            //Get for user if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.User) && userId.HasValue)
            {
                var settingValue = await GetSettingValueForUserOrNullAsync(userId.Value, name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }

                if (!settingDefinition.IsInherited)
                {
                    return settingDefinition.DefaultValue;
                }
            }

            //Get for tenant if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.Tenant) && tenantId.HasValue)
            {
                var settingValue = await GetSettingValueForTenantOrNullAsync(tenantId.Value, name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }

                if (!settingDefinition.IsInherited)
                {
                    return settingDefinition.DefaultValue;
                }
            }

            //Get for application if defined
            if (settingDefinition.Scopes.HasFlag(SettingScopes.Application))
            {
                var settingValue = await GetSettingValueForApplicationOrNullAsync(name);
                if (settingValue != null)
                {
                    return settingValue.Value;
                }
            }

            //Not defined, get default value
            return settingDefinition.DefaultValue;
        }

        private async Task<SettingInfo<TTenantId, TUserId>> InsertOrUpdateOrDeleteSettingValueAsync(string name,
            string value, TTenantId? tenantId, TUserId? userId)
        {
            if (tenantId.HasValue && userId.HasValue)
            {
                throw new ApplicationException("Both of tenantId and userId can not be set!");
            }

            var settingDefinition = _settingDefinitionManager.GetSettingDefinition(name);
            var settingValue = await SettingStore.GetSettingOrNullAsync(tenantId, userId, name);

            //Determine defaultValue
            var defaultValue = settingDefinition.DefaultValue;

            if (settingDefinition.IsInherited)
            {
                //For Tenant and User, Application's value overrides Setting Definition's default value.
                if (tenantId.HasValue || userId.HasValue)
                {
                    var applicationValue = await GetSettingValueForApplicationOrNullAsync(name);
                    if (applicationValue != null)
                    {
                        defaultValue = applicationValue.Value;
                    }
                }

                //For User, Tenants's value overrides Application's default value.
                if (userId.HasValue && AbpSession.TenantId.HasValue)
                {
                    var tenantValue = await GetSettingValueForTenantOrNullAsync(AbpSession.TenantId.Value, name);
                    if (tenantValue != null)
                    {
                        defaultValue = tenantValue.Value;
                    }
                }
            }

            //No need to store on database if the value is the default value
            if (value == defaultValue)
            {
                if (settingValue != null)
                {
                    await SettingStore.DeleteAsync(settingValue);
                }

                return null;
            }

            //If it's not default value and not stored on database, then insert it
            if (settingValue == null)
            {
                settingValue = new SettingInfo<TTenantId, TUserId>
                {
                    TenantId = tenantId,
                    UserId = userId,
                    Name = name,
                    Value = value
                };

                await SettingStore.CreateAsync(settingValue);
                return settingValue;
            }

            //It's same value in database, no need to update
            if (settingValue.Value == value)
            {
                return settingValue;
            }

            //Update the setting on database.
            settingValue.Value = value;
            await SettingStore.UpdateAsync(settingValue);

            return settingValue;
        }

        private async Task<SettingInfo<TTenantId, TUserId>> GetSettingValueForApplicationOrNullAsync(string name)
        {
            return (await GetApplicationSettingsAsync()).GetOrDefault(name);
        }

        private async Task<SettingInfo<TTenantId, TUserId>> GetSettingValueForTenantOrNullAsync(TTenantId tenantId,
            string name)
        {
            return (await GetReadOnlyTenantSettings(tenantId)).GetOrDefault(name);
        }

        private async Task<SettingInfo<TTenantId, TUserId>> GetSettingValueForUserOrNullAsync(TUserId userId,
            string name)
        {
            return (await GetReadOnlyUserSettings(userId)).GetOrDefault(name);
        }

        private async Task<Dictionary<string, SettingInfo<TTenantId, TUserId>>> GetApplicationSettingsAsync()
        {
            return await _applicationSettingCache.GetAsync(ApplicationSettingsCacheKey, async () =>
            {
                var dictionary = new Dictionary<string, SettingInfo<TTenantId, TUserId>>();

                var settingValues = await SettingStore.GetAllListAsync(null, null);
                foreach (var settingValue in settingValues)
                {
                    dictionary[settingValue.Name] = settingValue;
                }

                return dictionary;
            });
        }

        private async Task<ImmutableDictionary<string, SettingInfo<TTenantId, TUserId>>> GetReadOnlyTenantSettings(
            TTenantId tenantId)
        {
            var cachedDictionary = await GetTenantSettingsFromCache(tenantId);
            lock (cachedDictionary)
            {
                return cachedDictionary.ToImmutableDictionary();
            }
        }

        private async Task<ImmutableDictionary<string, SettingInfo<TTenantId, TUserId>>> GetReadOnlyUserSettings(
            TUserId userId)
        {
            var cachedDictionary = await GetUserSettingsFromCache(userId);
            lock (cachedDictionary)
            {
                return cachedDictionary.ToImmutableDictionary();
            }
        }

        private async Task<Dictionary<string, SettingInfo<TTenantId, TUserId>>> GetTenantSettingsFromCache(
            TTenantId tenantId)
        {
            return await _tenantSettingCache.GetAsync(
                tenantId,
                async () =>
                {
                    var dictionary = new Dictionary<string, SettingInfo<TTenantId, TUserId>>();

                    var settingValues = await SettingStore.GetAllListAsync(tenantId, null);
                    foreach (var settingValue in settingValues)
                    {
                        dictionary[settingValue.Name] = settingValue;
                    }

                    return dictionary;
                });
        }

        private async Task<Dictionary<string, SettingInfo<TTenantId, TUserId>>> GetUserSettingsFromCache(TUserId userId)
        {
            return await _userSettingCache.GetAsync(
                userId,
                async () =>
                {
                    var dictionary = new Dictionary<string, SettingInfo<TTenantId, TUserId>>();

                    var settingValues = await SettingStore.GetAllListAsync(null, userId);
                    foreach (var settingValue in settingValues)
                    {
                        dictionary[settingValue.Name] = settingValue;
                    }

                    return dictionary;
                });
        }

        #endregion

        #region Nested classes

        private class SettingValueObject : ISettingValue
        {
            public string Name { get; private set; }

            public string Value { get; private set; }

            public SettingValueObject(string name, string value)
            {
                Value = value;
                Name = name;
            }
        }

        #endregion
    }
}