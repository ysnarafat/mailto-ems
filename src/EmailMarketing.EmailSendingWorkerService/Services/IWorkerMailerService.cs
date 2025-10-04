using EmailMarketing.Framework.Entities.SMTP;
using System.Threading.Tasks;

namespace EmailMarketing.EmailSendingWorkerService.Services
{
    public interface IWorkerMailerService
    {
        Task<bool> SendBulkEmailAsync(string email, string subject, string body, SMTPConfig sMTPConfig);
    }
}
