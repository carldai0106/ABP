using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using Abp.Logging;

namespace Abp.Configuration
{
    /// <summary>
    /// Implements default behavior for ISettingStore.
    /// Only <see cref="GetSettingOrNullAsync"/> method is implemented and it gets setting's value
    /// from application's configuration file if exists, or returns null if not.
    /// </summary>
    public class DefaultConfigSettingStore<TTenantId, TUserId> : ISettingStore<TTenantId, TUserId> 
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Gets singleton instance.
        /// </summary>
        public static DefaultConfigSettingStore<TTenantId, TUserId> Instance { get { return SingletonInstance; } }
        private static readonly DefaultConfigSettingStore<TTenantId, TUserId> SingletonInstance = new DefaultConfigSettingStore<TTenantId, TUserId>();

        private DefaultConfigSettingStore()
        {
        }

        public Task<SettingInfo<TTenantId, TUserId>> GetSettingOrNullAsync(TTenantId? tenantId, TUserId? userId, string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            
            if (value == null)
            {
                return Task.FromResult<SettingInfo<TTenantId, TUserId>>(null);
            }

            return Task.FromResult(new SettingInfo<TTenantId, TUserId>(tenantId, userId, name, value));
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(SettingInfo<TTenantId, TUserId> setting)
        {
            LogHelper.Logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support DeleteAsync.");
        }

        /// <inheritdoc/>
        public async Task CreateAsync(SettingInfo<TTenantId, TUserId> setting)
        {
            LogHelper.Logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support CreateAsync.");
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(SettingInfo<TTenantId, TUserId> setting)
        {
            LogHelper.Logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support UpdateAsync.");
        }

        /// <inheritdoc/>
        public Task<List<SettingInfo<TTenantId, TUserId>>> GetAllListAsync(TTenantId? tenantId, TUserId? userId)
        {
            LogHelper.Logger.Warn("ISettingStore is not implemented, using DefaultConfigSettingStore which does not support GetAllListAsync.");
            return Task.FromResult(new List<SettingInfo<TTenantId, TUserId>>());
        }
    }
}