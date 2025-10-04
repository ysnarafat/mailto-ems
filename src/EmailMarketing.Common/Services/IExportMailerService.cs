using System.Threading.Tasks;

namespace EmailMarketing.Common.Services
{
    public interface IExportMailerService
    {
        Task SendEmailAsync(string email, string subject, string body, string url);
    }
}
