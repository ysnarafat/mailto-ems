using EmailMarketing.Common.Extensions;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.SMTP;
using EmailMarketing.Framework.Services.SMTP;
using EmailMarketing.Membership.Services;
using EmailMarketing.Web.Core;
using EmailMarketing.Web.Services;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Smtp
{
    public class CreateSMTPModel : SMTPBaseModel
    {
        [Required]
        public string Server { get; set; }
        [Required]
        public int? Port { get; set; }
        [Required]
        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }
        [Required]
        [Display(Name = "Sender Email")]
        [EmailAddress]
        public string SenderEmail { get; set; }
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        [Display(Name = "Enable SSL")]
        public bool EnableSSL { get; set; }
        public Guid UserId { get; set; }

        public CreateSMTPModel(ISMTPService smtpService, IApplicationUserService applicationUserService,
            ICurrentUserService currentUserService, ISmtpTestService smtpTestService, IOptions<AppSettings> appSettings) :
                base(smtpService, applicationUserService, currentUserService, smtpTestService, appSettings)
        {

        }

        public CreateSMTPModel() : base()
        {

        }

        public async Task AddAsync()
        {
            this.Password = this.Password.ToEncryptString(this._appSettings.EncryptionDecryptionKey);

            var entity = new SMTPConfig
            {
                Server = this.Server,
                Port = this.Port ?? 0,
                SenderName = this.SenderName,
                SenderEmail = this.SenderEmail,
                UserName = this.UserName,
                Password = this.Password,
                EnableSSL = this.EnableSSL,
                UserId = _currentUserService.UserId
            };
            await _smtpService.AddAsync(entity);
        }

        public async Task<bool> SmtpTest()
        {
            var smtpConfig = new SMTPConfig
            {
                Server = this.Server,
                Port = this.Port ?? 0,
                SenderName = this.SenderName,
                SenderEmail = this.SenderEmail,
                UserName = this.UserName,
                Password = this.Password,
                EnableSSL = this.EnableSSL

            };
            return (await _smtpTestService.TestSmtpSettings(smtpConfig));
        }
    }
}
