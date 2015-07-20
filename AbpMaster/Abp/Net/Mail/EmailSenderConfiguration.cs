using System;
using Abp.Configuration;
using Abp.Extensions;

namespace Abp.Net.Mail
{
    /// <summary>
    /// Implementation of <see cref="IEmailSenderConfiguration"/> that reads settings
    /// from <see cref="ISettingManager"/>.
    /// </summary>
    public abstract class EmailSenderConfiguration<TTenantId, TUserId> : IEmailSenderConfiguration<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        public string DefaultFromAddress
        {
            get { return GetNotEmptySettingValue(EmailSettingNames.DefaultFromAddress); }
        }

        public string DefaultFromDisplayName
        {
            get { return SettingManager.GetSettingValue(EmailSettingNames.DefaultFromDisplayName); }
        }

        protected readonly ISettingManager<TTenantId, TUserId> SettingManager;

        /// <summary>
        /// Creates a new <see cref="EmailSenderConfiguration"/>.
        /// </summary>
        protected EmailSenderConfiguration(ISettingManager<TTenantId, TUserId> settingManager)
        {
            SettingManager = settingManager;
        }

        /// <summary>
        /// Gets a setting value by checking. Throws <see cref="AbpException"/> if it's null or empty.
        /// </summary>
        /// <param name="name">Name of the setting</param>
        /// <returns>Value of the setting</returns>
        protected string GetNotEmptySettingValue(string name)
        {
            var value = SettingManager.GetSettingValue(name);
            if (value.IsNullOrEmpty())
            {
                throw new AbpException(String.Format("Setting value for '{0}' is null or empty!", name));
            }

            return value;
        }
    }
}