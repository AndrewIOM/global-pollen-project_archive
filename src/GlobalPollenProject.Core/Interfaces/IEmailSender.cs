using System.Threading.Tasks;

namespace GlobalPollenProject.Core.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}