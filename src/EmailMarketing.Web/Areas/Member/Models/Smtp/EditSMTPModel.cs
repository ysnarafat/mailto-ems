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
    public class EditSMTPModel : SMTPBaseModel
    {
        [Required]
        public Guid Id { get; set; }
        public string Server { get; set; }
        [Required]
        public int Port { get; set; }
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
        [Required]
        public bool EnableSSL { get; set; }

        public EditSMTPModel(ISMTPService smtpService, IApplicationUserService applicationUserService,
           ICurrentUserService currentUserService, ISmtpTestService smtpTestService, IOptions<AppSettings> appSettings) :
                base(smtpService, applicationUserService, currentUserService, smtpTestService, appSettings)
        {

        }

        public EditSMTPModel() : base()
        {

        }

        public async Task LoadByIdAsync(Guid id)
        {
            var result = await _smtpService.GetByIdAsync(id);
            this.Id = result.Id;
            this.Server = result.Server;
            this.Port = result.Port;
            this.SenderName = result.SenderName;
            this.SenderEmail = result.SenderEmail;
            this.UserName = result.UserName;
            //this.Password = result.Password;
            this.EnableSSL = result.EnableSSL;
        }

        public async Task UpdateAsync()
        {
            this.Password = this.Password.ToEncryptString(this._appSettings.EncryptionDecryptionKey);

            var entity = new SMTPConfig
            {
                Id = this.Id,
                Server = this.Server,
                Port = this.Port,
                SenderName = this.SenderName,
                SenderEmail = this.SenderEmail,
                UserName = this.UserName,
                Password = this.Password,
                EnableSSL = this.EnableSSL
            };
            await _smtpService.UpdateAsync(entity);
        }
    }
}
