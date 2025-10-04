using EmailMarketing.Common.Extensions;
using EmailMarketing.Common.Services;
using EmailMarketing.EmailSendingWorkerService.Core;
using EmailMarketing.Framework.Entities.SMTP;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.EmailSendingWorkerService.Services
{
    public class WorkerMailerService : IWorkerMailerService, IMailerService
    {
        private readonly WorkerSmtpSettings _workerSmtpSettings;
        private readonly WorkerSettings _workerSettings;
        private readonly ILogger<WorkerMailerService> _logger;

        public WorkerMailerService(IOptions<WorkerSmtpSettings> smtpSettings, IOptions<WorkerSettings> workerSettings, ILogger<WorkerMailerService> logger)
        {
            _workerSmtpSettings = smtpSettings.Value;
            _workerSettings = workerSettings.Value;
            _logger = logger;
        }

        public async Task<bool> SendBulkEmailAsync(string email, string subject, string body, SMTPConfig sMTPConfig)
        {
            try
            {
                var messgae = new MimeMessage();
                messgae.From.Add(new MailboxAddress(sMTPConfig.SenderName, sMTPConfig.SenderEmail));
                messgae.To.Add(MailboxAddress.Parse(email));
                messgae.Subject = subject;
                messgae.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(sMTPConfig.Server, sMTPConfig.Port, true);

                    await client.AuthenticateAsync(sMTPConfig.UserName, sMTPConfig.Password.ToDecryptString(this._workerSettings.EncryptionDecryptionKey));
                    await client.SendAsync(messgae);
                    await client.DisconnectAsync(true);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send email for : {email} and Error Message: " + ex.Message);
                return false;
            }
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var messgae = new MimeMessage();
                messgae.From.Add(new MailboxAddress(_workerSmtpSettings.SenderName, _workerSmtpSettings.SenderEmail));
                InternetAddressList toList = new InternetAddressList();
                foreach (var item in email.Split(',').ToList())
                {
                    toList.Add(MailboxAddress.Parse(item.Trim()));
                }
                //messgae.To.Add(MailboxAddress.Parse(email));
                messgae.To.AddRange(toList);
                messgae.Subject = subject;
                messgae.Body = new TextPart("html")
                {
                    Text = body
                };

                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_workerSmtpSettings.Server, _workerSmtpSettings.Port, true);

                    await client.AuthenticateAsync(_workerSmtpSettings.UserName, _workerSmtpSettings.Password);
                    await client.SendAsync(messgae);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(e.Message);
            }
        }

    }
}
