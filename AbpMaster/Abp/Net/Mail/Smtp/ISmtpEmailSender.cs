using System.Net.Mail;

namespace Abp.Net.Mail.Smtp
{
    /// <summary>
    /// Used to send emails over SMTP.
    /// </summary>
    public interface ISmtpEmailSender<TTenantId, TUserId> : IEmailSender<TTenantId, TUserId>
        where TTenantId : struct
        where TUserId : struct
    {
        /// <summary>
        /// Creates and configures new <see cref="SmtpClient"/> object to send emails. 
        /// </summary>
        /// <returns>
        /// An <see cref="SmtpClient"/> object that is ready to send emails.
        /// </returns>
        SmtpClient BuildClient();
    }
}