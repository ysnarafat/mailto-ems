using System;
using EmailMarketing.Framework.Entities;
using EmailMarketing.Framework.Entities.Campaigns;
using EmailMarketing.Framework.Entities.Contacts;
using EmailMarketing.Framework.Entities.Groups;
using EmailMarketing.Framework.Entities.SMTP;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Framework.Context
{
    /// <summary>
    /// LEGACY: FrameworkContext is deprecated and kept for backward compatibility during migration.
    /// New code should use ApplicationDbContext from EmailMarketing.Shared.Infrastructure instead.
    /// This context is being phased out as part of the modular monolith migration.
    /// </summary>
    [Obsolete("Use ApplicationDbContext from EmailMarketing.Shared.Infrastructure instead", false)]
    public class FrameworkContext : DbContext
    {
        public FrameworkContext(DbContextOptions<FrameworkContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CampaignGroup>(userRole =>
            {
                userRole.HasKey(ur => new { ur.CampaignId, ur.GroupId });
            });
        }

        public DbSet<SMTPConfig> SMTPConfigs { get; set; } = null!;
        public DbSet<Campaign> Campaigns { get; set; } = null!;
        public DbSet<CampaignGroup> CampaignGroups { get; set; } = null!;
        public DbSet<CampaignReport> CampaignReports { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<ContactGroup> ContactGroups { get; set; } = null!;
        public DbSet<ContactUploadGroup> ContactUploadGroups { get; set; } = null!;
        public DbSet<ContactValueMap> ContactValueMaps { get; set; } = null!;
        public DbSet<ContactUpload> ContactUploads { get; set; } = null!;
        public DbSet<FieldMap> FieldMaps { get; set; } = null!;
        public DbSet<ContactUploadFieldMap> ContactUploadFieldMaps { get; set; } = null!;
        public DbSet<EmailTemplate> EmailTemplates { get; set; } = null!;
        public DbSet<DownloadQueue> DownloadQueues { get; set; } = null!;
        public DbSet<DownloadQueueSubEntity> DownloadQueueSubEntities { get; set; } = null!;
    }
}
