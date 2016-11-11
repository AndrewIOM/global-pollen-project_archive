using Microsoft.Extensions.Options;
using SendGrid;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GlobalPollenProject.Infrastructure.Options;
using GlobalPollenProject.Core.Interfaces;

namespace GlobalPollenProject.Infrastructure
{
    public class SendgridEmailSender : IEmailSender
    {
        public AuthMessageSenderOptions Options { get; }
        public SendgridEmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var myMessage = new SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new MailAddress("noreply@globalpollenproject.org", "Global Pollen Project");
            myMessage.Subject = subject;
            myMessage.Text = message;
            myMessage.Html = message;
            var credentials = new NetworkCredential(
                Options.SendGridUser,
                Options.SendGridKey);
            var transportWeb = new Web(credentials);
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
        }
    }
}
