using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abp.Configuration
{
    /// <summary>
    /// Implements null pattern for ISettingStore.
    /// </summary>
    public class NullSettingStore<TTenantId, TUserId> : ISettingStore<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Gets singleton instance.
        /// </summary>
        public static NullSettingStore<TTenantId, TUserId> Instance { get { return SingletonInstance; } }
        private static readonly NullSettingStore<TTenantId, TUserId> SingletonInstance = new NullSettingStore<TTenantId, TUserId>();

        private NullSettingStore()
        {
        }

        public Task<SettingInfo<TTenantId, TUserId>> GetSettingOrNullAsync(TTenantId? tenantId, TUserId? userId, string name)
        {
            return Task.FromResult<SettingInfo<TTenantId, TUserId>>(null);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(SettingInfo<TTenantId, TUserId> setting)
        {
        }

        /// <inheritdoc/>
        public async Task CreateAsync(SettingInfo<TTenantId, TUserId> setting)
        {
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(SettingInfo<TTenantId, TUserId> setting)
        {
        }

        /// <inheritdoc/>
        public Task<List<SettingInfo<TTenantId, TUserId>>> GetAllListAsync(TTenantId? tenantId, TUserId? userId)
        {
            return Task.FromResult(new List<SettingInfo<TTenantId, TUserId>>());
        }
    }
}