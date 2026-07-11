using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Services.Contacts;
using System;

namespace EmailMarketing.Web.Areas.Member.Models.Contacts
{
    public class ContactUploadBaseModel : MemberBaseModel, IDisposable
    {
        protected readonly IContactUploadService _contactUploadService;
        protected new readonly ICurrentUserService _currentUserService;

        public ContactUploadBaseModel(IContactUploadService contactUpload,
            ICurrentUserService currentUserService)
        {
            _contactUploadService = contactUpload;
            _currentUserService = currentUserService;
        }

        public ContactUploadBaseModel()
        {
            _contactUploadService = Startup.AutofacContainer.Resolve<IContactUploadService>();
            _currentUserService = Startup.AutofacContainer.Resolve<ICurrentUserService>();
        }


        public void Dispose()
        {
            _contactUploadService?.Dispose();
        }
    }
}
