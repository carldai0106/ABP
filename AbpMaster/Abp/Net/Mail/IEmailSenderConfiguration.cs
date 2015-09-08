namespace Abp.Net.Mail
{
    /// <summary>
    ///     Defines configurations used while sending emails.
    /// </summary>
    public interface IEmailSenderConfiguration<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        ///     Default from address.
        /// </summary>
        string DefaultFromAddress { get; }

        /// <summary>
        ///     Default display name.
        /// </summary>
        string DefaultFromDisplayName { get; }
    }
}