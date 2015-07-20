using Abp.Net.Mail.Smtp;
using NSubstitute;
using Xunit;

namespace Abp.Tests.Net.Mail
{
    public class SmtpEmailSender_Tests
    {
        private readonly SmtpEmailSender<int,long> _smtpEmailSender;

        public SmtpEmailSender_Tests()
        {
            var configuration = Substitute.For<ISmtpEmailSenderConfiguration<int, long>>();

            configuration.DefaultFromAddress.Returns("...");
            configuration.DefaultFromDisplayName.Returns("...");

            configuration.Host.Returns("...");
            configuration.Port.Returns(25);

            //configuration.Domain.Returns("...");
            configuration.UserName.Returns("...");
            configuration.Password.Returns("...");

            //configuration.EnableSsl.Returns(false);
            //configuration.UseDefaultCredentials.Returns(false);

            _smtpEmailSender = new SmtpEmailSender<int, long>(configuration);
        }

        //[Fact] //Need to set configuration before executing this test
        public void Test_Send_Email()
        {
            _smtpEmailSender.Send(
                "...", 
                "Test email", 
                "An email body"
                );
        }
    }
}
