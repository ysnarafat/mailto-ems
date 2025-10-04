using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Entities.SMTP;
using EmailMarketing.Framework.Services.SMTP;
using EmailMarketing.Membership.Services;
using EmailMarketing.Web.Core;
using EmailMarketing.Web.Services;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmailMarketing.Web.Areas.Member.Models.Smtp
{
    public class SMTPModel : SMTPBaseModel
    {
        public SMTPModel(ISMTPService smtpService, IApplicationUserService applicationUserService,
            ICurrentUserService currentUserService, ISmtpTestService smtpTestService, IOptions<AppSettings> appSettings) :
                base(smtpService, applicationUserService, currentUserService, smtpTestService, appSettings)
        {
        }

        public SMTPModel() : base() { }

        public async Task<object> GetAllAsync(DataTablesAjaxRequestModel tableModel)
        {
            var userId = _currentUserService.UserId;
            var result = await _smtpService.GetAllAsync(
                userId,
                tableModel.SearchText,
                tableModel.GetSortText(new string[] { "Server", "Port", "SenderName", "SenderEmail", "UserName", "EnableSSL" }),
                tableModel.PageIndex, tableModel.PageSize);

            return new
            {
                recordsTotal = result.Total,
                recordsFiltered = result.TotalFilter,

                data = (from item in result.Items
                        select new string[]
                        {
                                    item.Server,
                                    item.Port.ToString(),
                                    item.SenderName,
                                    item.SenderEmail,
                                    item.UserName,
                                    item.EnableSSL.ToString(),
                                    item.IsActive ? "Yes" : "No",
                                    item.Id.ToString()
                        }
                        ).ToArray()

            };
        }

        public async Task<SMTPConfig> ActivateSmtpAsync(Guid id)
        {
            var smtp = await _smtpService.ActivateSmtpAsync(id);
            return smtp;
        }
    }
}
