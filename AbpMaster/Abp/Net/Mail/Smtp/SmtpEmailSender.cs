using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Extensions;

namespace Abp.Net.Mail.Smtp
{
    /// <summary>
    ///     Used to send emails over SMTP.
    /// </summary>
    public class SmtpEmailSender<TTenantId, TUserId> : EmailSenderBase<TTenantId, TUserId>,
        ISmtpEmailSender<TTenantId, TUserId>, ITransientDependency
        where TTenantId : struct
        where TUserId : struct
    {
        private readonly ISmtpEmailSenderConfiguration<TTenantId, TUserId> _configuration;

        /// <summary>
        ///     Creates a new <see cref="SmtpEmailSender" />.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public SmtpEmailSender(ISmtpEmailSenderConfiguration<TTenantId, TUserId> configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        public SmtpClient BuildClient()
        {
            var host = _configuration.Host;
            var port = _configuration.Port;

            var smtpClient = new SmtpClient(host, port);
            try
            {
                if (_configuration.EnableSsl)
                {
                    smtpClient.EnableSsl = true;
                }

                if (_configuration.UseDefaultCredentials)
                {
                    smtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    smtpClient.UseDefaultCredentials = false;

                    var userName = _configuration.UserName;
                    if (!userName.IsNullOrEmpty())
                    {
                        var password = _configuration.Password;
                        var domain = _configuration.Domain;
                        smtpClient.Credentials = !domain.IsNullOrEmpty()
                            ? new NetworkCredential(userName, password, domain)
                            : new NetworkCredential(userName, password);
                    }
                }

                return smtpClient;
            }
            catch
            {
                smtpClient.Dispose();
                throw;
            }
        }

        protected override async Task SendEmailAsync(MailMessage mail)
        {
            using (var smtpClient = BuildClient())
            {
                await smtpClient.SendMailAsync(mail);
            }
        }

        protected override void SendEmail(MailMessage mail)
        {
            using (var smtpClient = BuildClient())
            {
                smtpClient.Send(mail);
            }
        }
    }
}