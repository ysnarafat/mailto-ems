using Autofac;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Repositories.Campaigns;
using EmailMarketing.Framework.Repositories.Contacts;
using EmailMarketing.Framework.Repositories.Groups;
using EmailMarketing.Framework.Repositories.SMTP;
using EmailMarketing.Framework.Services.Campaigns;
using EmailMarketing.Framework.Services.Contacts;
using EmailMarketing.Framework.Services.Groups;
using EmailMarketing.Framework.Services.SMTP;
using EmailMarketing.Framework.UnitOfWorks.Campaigns;
using EmailMarketing.Framework.UnitOfWorks.Contacts;
using EmailMarketing.Framework.UnitOfWorks.Groups;
using EmailMarketing.Framework.UnitOfWorks.SMTP;

namespace EmailMarketing.Framework
{
    public class FrameworkModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public FrameworkModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FrameworkContext>()
                   .WithParameter("connectionString", _connectionString)
                   .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                   .InstancePerLifetimeScope();

            builder.RegisterType<CampaignReportExportUnitOfWork>().As<ICampaignReportExportUnitOfWork>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<CampaignReportExportService>().As<ICampaignReportExportService>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<CampaignReportExportRepository>().As<ICampaignReportExportRepository>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<CampaignReportRepository>().As<ICampaignReportRepository>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<CampaignReportUnitOfWork>().As<ICampaignReportUnitOfWork>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<CampaignUnitOfWork>().As<ICampaignUnitOfWork>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<CampaignService>().As<ICampaignService>()
                  .InstancePerLifetimeScope();
            builder.RegisterType<CampaignReportRepository>().As<ICampaignReportRepository>()
                  .InstancePerLifetimeScope();
            builder.RegisterType<CampaignRepository>().As<ICampaignRepository>()
                  .InstancePerLifetimeScope();

            builder.RegisterType<CampaignRepository>().As<ICampaignRepository>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<DownloadQueueRepository>().As<IDownloadQueueRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DownloadQueueSubEntityRepository>().As<IDownloadQueueSubEntityRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<SMTPRepository>().As<ISMTPRepository>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<SMTPUnitOfWork>().As<ISMTPUnitOfWork>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<SMTPService>().As<ISMTPService>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<ContactUploadUnitOfWork>().As<IContactUploadUnitOfWork>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<GroupUnitOfWork>().As<IGroupUnitOfWork>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<ContactUnitOfWork>().As<IContactUnitOfWork>()
                   .InstancePerLifetimeScope();

            builder.RegisterType<GroupRepository>().As<IGroupRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactRepository>().As<IContactRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactUploadRepository>().As<IContactUploadRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<FieldMapService>().As<IFieldMapService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<FieldMapUnitOfWork>().As<IFieldMapUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<FieldMapRepository>().As<IFieldMapRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ContactUploadFieldMapRepository>().As<IContactUploadFieldMapRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactValueMapRepository>().As<IContactValueMapRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ContactUploadGroupRepository>().As<IContactUploadGroupRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactGroupRepository>().As<IContactGroupRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GroupContactRepository>().As<IGroupContactRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<GroupService>().As<IGroupService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ContactUploadService>().As<IContactUploadService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ContactService>().As<IContactService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactUnitOfWork>().As<IContactUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactRepository>().As<IContactRepository>()
                .InstancePerLifetimeScope();


            builder.RegisterType<DownloadQueueRepository>().As<IDownloadQueueRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DownloadQueueSubEntityRepository>().As<IDownloadQueueSubEntityRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactExportUnitOfWork>().As<IContactExportUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactExportService>().As<IContactExportService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CampaignRepository>().As<ICampaignRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CampaignUnitOfWork>().As<ICampaignUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CampaignService>().As<ICampaignService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CampaignReportService>().As<ICampaignReportService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EmailTemplateRepository>().As<IEmailTemplateRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EmailTemplateService>().As<IEmailTemplateService>()
                .InstancePerLifetimeScope();



            base.Load(builder);
        }
    }
}

