using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Entities.Groups;
using EmailMarketing.Framework.Entities.SMTP;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Framework.Context
{
    public class FrameworkContext : DbContext
    {

        private string _connectionString;
        private string _migrationAssemblyName;

        public FrameworkContext(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                dbContextOptionsBuilder.UseSqlServer(
                    _connectionString,
                    m => m.MigrationsAssembly(_migrationAssemblyName));
            }

            base.OnConfiguring(dbContextOptionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CampaignGroup>(userRole =>
            {
                userRole.HasKey(ur => new { ur.CampaignId, ur.GroupId });
            });
        }

        public DbSet<SMTPConfig> SMTPConfigs { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignGroup> CampaignGroups { get; set; }
        public DbSet<CampaignReport> CampaignReports { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ContactGroup> ContactGroups { get; set; }
        public DbSet<ContactUploadGroup> ContactUploadGroups { get; set; }
        public DbSet<ContactValueMap> ContactValueMaps { get; set; }
        public DbSet<ContactUpload> ContactUploads { get; set; }
        public DbSet<FieldMap> FieldMaps { get; set; }
        public DbSet<ContactUploadFieldMap> ContactUploadFieldMaps { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<DownloadQueue> DownloadQueues { get; set; }
        public DbSet<DownloadQueueSubEntity> DownloadQueueSubEntities { get; set; }
    }
}
