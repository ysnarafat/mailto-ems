using Autofac;
using EmailMarketing.Common.Services;
using EmailMarketing.Framework.Services.Groups;
using EmailMarketing.Membership.Services;
using System;

namespace EmailMarketing.Web.Areas.Member.Models.Groups
{
    public class GroupBaseModel : MemberBaseModel, IDisposable
    {
        protected readonly IGroupService _groupService;
        protected readonly IApplicationUserService _applicationUserService;
        protected new readonly ICurrentUserService _currentUserService;

        public GroupBaseModel(IGroupService groupService, IApplicationUserService applicationUserService,
            ICurrentUserService currentUserService)
        {
            _groupService = groupService;
            _applicationUserService = applicationUserService;
            _currentUserService = currentUserService;
        }

        public GroupBaseModel()
        {
            _groupService = Startup.AutofacContainer.Resolve<IGroupService>();
            _applicationUserService = Startup.AutofacContainer.Resolve<IApplicationUserService>();
            _currentUserService = Startup.AutofacContainer.Resolve<ICurrentUserService>();
        }

        public void Dispose()
        {
            _groupService?.Dispose();
        }
    }
}
