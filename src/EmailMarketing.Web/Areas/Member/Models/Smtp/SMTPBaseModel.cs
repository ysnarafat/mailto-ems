using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Services.SMTP;
using EmailMarketing.Membership.Services;
using EmailMarketing.Web.Core;
using EmailMarketing.Web.Services;
using Microsoft.Extensions.Options;
using System;

namespace EmailMarketing.Web.Areas.Member.Models.Smtp
{
    public class SMTPBaseModel : MemberBaseModel, IDisposable
    {
        protected readonly ISMTPService _smtpService;
        protected readonly IApplicationUserService _applicationUserService;
        protected readonly ICurrentUserService _currentUserService;
        protected readonly ISmtpTestService _smtpTestService;
        protected readonly AppSettings _appSettings;

        public SMTPBaseModel(ISMTPService smtpService, IApplicationUserService applicationUserService,
            ICurrentUserService currentUserService, ISmtpTestService smtpTestService, IOptions<AppSettings> appSettings)
        {
            _smtpService = smtpService;
            _applicationUserService = applicationUserService;
            _currentUserService = currentUserService;
            _smtpTestService = smtpTestService;
            this._appSettings = appSettings.Value;
        }

        public SMTPBaseModel()
        {
            _smtpService = Startup.AutofacContainer.Resolve<ISMTPService>();
            _applicationUserService = Startup.AutofacContainer.Resolve<IApplicationUserService>();
            _currentUserService = Startup.AutofacContainer.Resolve<ICurrentUserService>();
            _smtpTestService = Startup.AutofacContainer.Resolve<ISmtpTestService>();
            this._appSettings = Startup.AutofacContainer.Resolve<IOptions<AppSettings>>().Value;
        }

        public void Dispose()
        {
            _smtpService?.Dispose();
        }
    }
}
