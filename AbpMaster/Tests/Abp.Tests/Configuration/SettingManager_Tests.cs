﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Configuration;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Abp.Tests.Configuration
{
    public class SettingManager_Tests : TestBaseWithLocalIocManager
    {
        private const string MyAppLevelSetting = "MyAppLevelSetting";
        private const string MyAllLevelsSetting = "MyAllLevelsSetting";
        private const string MyNotInheritedSetting = "MyNotInheritedSetting";

        [Fact]
        public async Task Should_Get_Default_Values_With_No_Store_And_No_Session()
        {
            var settingManager = new SettingManager<int, long>(CreateMockSettingDefinitionManager());

            (await settingManager.GetSettingValueAsync<int, int, long>(MyAppLevelSetting)).ShouldBe(42);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("application level default value");
        }

        [Fact]
        public async Task Should_Get_Stored_Application_Value_With_No_Session()
        {
            var settingManager = new SettingManager<int, long>(CreateMockSettingDefinitionManager())
            {
                SettingStore = new MemorySettingStore()
            };

            (await settingManager.GetSettingValueAsync<int, int, long>(MyAppLevelSetting)).ShouldBe(48);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("application level stored value");
        }

        [Fact]
        public async Task Should_Get_Correct_Values()
        {
            var session = new MyChangableSession<int, long>();

            var settingManager = new SettingManager<int, long>(CreateMockSettingDefinitionManager());
            settingManager.SettingStore = new MemorySettingStore();
            settingManager.AbpSession = session;

            session.TenantId = 1;

            //Inherited setting

            session.UserId = 1;
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("user 1 stored value");

            session.UserId = 2;
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("user 2 stored value");

            session.UserId = 3;
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("tenant 1 stored value"); //Because no user value in the store

            session.TenantId = 3;
            session.UserId = 3;
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("application level stored value"); //Because no user and tenant value in the store

            //Not inherited setting

            session.TenantId = 1;
            session.UserId = 1;

            (await settingManager.GetSettingValueForApplicationAsync(MyNotInheritedSetting)).ShouldBe("application value");
            (await settingManager.GetSettingValueForTenantAsync(MyNotInheritedSetting, session.TenantId.Value)).ShouldBe("default-value");
            (await settingManager.GetSettingValueAsync(MyNotInheritedSetting)).ShouldBe("default-value");
        }

        [Fact]
        public async Task Should_Get_All_Values()
        {
            var settingManager = new SettingManager<int, long>(CreateMockSettingDefinitionManager())
            {
                SettingStore = new MemorySettingStore()
            };

            (await settingManager.GetAllSettingValuesAsync()).Count.ShouldBe(3);

            (await settingManager.GetAllSettingValuesForApplicationAsync()).Count.ShouldBe(3);

            (await settingManager.GetAllSettingValuesForTenantAsync(1)).Count.ShouldBe(1);
            (await settingManager.GetAllSettingValuesForTenantAsync(2)).Count.ShouldBe(0);
            (await settingManager.GetAllSettingValuesForTenantAsync(3)).Count.ShouldBe(0);

            (await settingManager.GetAllSettingValuesForUserAsync(1)).Count.ShouldBe(1);
            (await settingManager.GetAllSettingValuesForUserAsync(2)).Count.ShouldBe(1);
            (await settingManager.GetAllSettingValuesForUserAsync(3)).Count.ShouldBe(0);
        }

        [Fact]
        public async Task Should_Change_Setting_Values()
        {
            var session = new MyChangableSession<int, long>();

            var settingManager = new SettingManager<int, long>(CreateMockSettingDefinitionManager())
            {
                SettingStore = new MemorySettingStore(),
                AbpSession = session
            };

            //Application level changes

            await settingManager.ChangeSettingForApplicationAsync(MyAppLevelSetting, "53");
            await settingManager.ChangeSettingForApplicationAsync(MyAppLevelSetting, "54");
            await settingManager.ChangeSettingForApplicationAsync(MyAllLevelsSetting, "application level changed value");

            (await settingManager.SettingStore.GetSettingOrNullAsync(null, null, MyAppLevelSetting)).Value.ShouldBe("54");

            (await settingManager.GetSettingValueAsync<int, int, long>(MyAppLevelSetting)).ShouldBe(54);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("application level changed value");

            //Tenant level changes

            session.TenantId = 1;
            await settingManager.ChangeSettingForTenantAsync(1, MyAllLevelsSetting, "tenant 1 changed value");
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("tenant 1 changed value");

            //User level changes

            session.UserId = 1;
            await settingManager.ChangeSettingForUserAsync(1, MyAllLevelsSetting, "user 1 changed value");
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("user 1 changed value");
        }

        [Fact]
        public async Task Should_Delete_Setting_Values_On_Default_Value()
        {
            var session = new MyChangableSession<int, long>();
            var store = new MemorySettingStore();

            var settingManager = new SettingManager<int, long>(CreateMockSettingDefinitionManager())
            {
                SettingStore = store,
                AbpSession = session
            };

            session.TenantId = 1;
            session.UserId = 1;

            //We can get user's personal stored value
            (await store.GetSettingOrNullAsync(null, 1, MyAllLevelsSetting)).ShouldNotBe(null);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("user 1 stored value");

            //This will delete setting for the user since it's same as tenant's setting value
            await settingManager.ChangeSettingForUserAsync(1, MyAllLevelsSetting, "tenant 1 stored value");
            (await store.GetSettingOrNullAsync(null, 1, MyAllLevelsSetting)).ShouldBe(null);

            //We can get tenant's setting value
            (await store.GetSettingOrNullAsync(1, null, MyAllLevelsSetting)).ShouldNotBe(null);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("tenant 1 stored value");

            //This will delete setting for tenant since it's same as application's setting value
            await settingManager.ChangeSettingForTenantAsync(1, MyAllLevelsSetting, "application level stored value");
            (await store.GetSettingOrNullAsync(null, 1, MyAllLevelsSetting)).ShouldBe(null);

            //We can get application's value
            (await store.GetSettingOrNullAsync(null, null, MyAllLevelsSetting)).ShouldNotBe(null);
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("application level stored value");

            //This will delete setting for application since it's same as the default value of the setting
            await settingManager.ChangeSettingForApplicationAsync(MyAllLevelsSetting, "application level default value");
            (await store.GetSettingOrNullAsync(null, null, MyAllLevelsSetting)).ShouldBe(null);

            //Now, there is no setting value, default value should return
            (await settingManager.GetSettingValueAsync(MyAllLevelsSetting)).ShouldBe("application level default value");
        }

        private static ISettingDefinitionManager CreateMockSettingDefinitionManager()
        {
            var settings = new Dictionary<string, SettingDefinition>
            {
                {MyAppLevelSetting, new SettingDefinition(MyAppLevelSetting, "42")},
                {MyAllLevelsSetting, new SettingDefinition(MyAllLevelsSetting, "application level default value", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User)},
                {MyNotInheritedSetting, new SettingDefinition(MyNotInheritedSetting, "default-value", scopes: SettingScopes.Application | SettingScopes.Tenant, isInherited: false)},
            };

            var definitionManager = Substitute.For<ISettingDefinitionManager>();

            //Implement methods
            definitionManager.GetSettingDefinition(Arg.Any<string>()).Returns(x => settings[x[0].ToString()]);
            definitionManager.GetAllSettingDefinitions().Returns(settings.Values.ToList());

            return definitionManager;
        }

        private class MemorySettingStore : ISettingStore<int, long>
        {
            private readonly List<SettingInfo<int, long>> _settings;

            public MemorySettingStore()
            {
                _settings = new List<SettingInfo<int, long>>
                {
                    new SettingInfo<int, long>(null, null, MyAppLevelSetting, "48"),
                    new SettingInfo<int, long>(null, null, MyAllLevelsSetting, "application level stored value"),
                    new SettingInfo<int, long>(1, null, MyAllLevelsSetting, "tenant 1 stored value"),
                    new SettingInfo<int, long>(null, 1, MyAllLevelsSetting, "user 1 stored value"),
                    new SettingInfo<int, long>(null, 2, MyAllLevelsSetting, "user 2 stored value"),
                    new SettingInfo<int, long>(null, null, MyNotInheritedSetting, "application value"),
                };
            }


            public Task<SettingInfo<int, long>> GetSettingOrNullAsync(int? tenantId, long? userId, string name)
            {
                return Task.FromResult(_settings.FirstOrDefault(s => s.TenantId == tenantId && s.UserId == userId && s.Name == name));
            }

            public async Task DeleteAsync(SettingInfo<int, long> setting)
            {
                _settings.RemoveAll(s => s.TenantId == setting.TenantId && s.UserId == setting.UserId && s.Name == setting.Name);
            }

            public async Task CreateAsync(SettingInfo<int, long> setting)
            {
                _settings.Add(setting);
            }

            public async Task UpdateAsync(SettingInfo<int, long> setting)
            {
                var s = await GetSettingOrNullAsync(setting.TenantId, setting.UserId, setting.Name);
                if (s != null)
                {
                    s.Value = setting.Value;
                }
            }

            public Task<List<SettingInfo<int, long>>> GetAllListAsync(int? tenantId, long? userId)
            {
                return Task.FromResult(_settings.Where(s => s.TenantId == tenantId && s.UserId == userId).ToList());
            }
        }
    }
}