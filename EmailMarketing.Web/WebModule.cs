using Autofac;
using EmailMarketing.Web.Areas.Admin.Models;
using EmailMarketing.Web.Areas.Admin.Models.AdminUsers;
using EmailMarketing.Web.Areas.Member.Models;
using EmailMarketing.Web.Areas.Member.Models.Campaigns;
using EmailMarketing.Web.Areas.Member.Models.Contacts;
using EmailMarketing.Web.Areas.Member.Models.Groups;
using EmailMarketing.Web.Areas.Member.Models.Smtp;
using EmailMarketing.Web.Models.Account;

namespace EmailMarketing.Web
{
    public class WebModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public WebModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AdminUsersModel>();
            builder.RegisterType<MemberUserModel>();
            builder.RegisterType<GroupModel>();
            builder.RegisterType<CampaignsModel>();
            builder.RegisterType<CampaignBaseModel>();
            builder.RegisterType<ContactsModel>();
            builder.RegisterType<FieldMapModel>();
            builder.RegisterType<SMTPModel>();
            builder.RegisterType<ChangeDefaultPasswordViewModel>();
            builder.RegisterType<ContactUploadModel>();
            builder.RegisterType<ViewContactUploadModel>();
            builder.RegisterType<ContactsBaseModel>();
            builder.RegisterType<MemberBaseModel>();

            base.Load(builder);
        }
    }
}
