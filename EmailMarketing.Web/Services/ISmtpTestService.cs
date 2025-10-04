using EmailMarketing.Framework.Entities.SMTP;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Services
{
    public interface ISmtpTestService
    {
        Task<bool> TestSmtpSettings(SMTPConfig sMTPConfig);
    }
}
