using EmailMarketing.Shared.Infrastructure.Data.Entities.Campaigns;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure.Data.Entities.FileProcessing;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Notifications;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Shared.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<
    ApplicationUser,
    ApplicationRole,
    Guid,
    ApplicationUserClaim,
    ApplicationUserRole,
    ApplicationUserLogin,
    ApplicationRoleClaim,
    ApplicationUserToken>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // ===== Campaign Module =====
    public DbSet<Campaign> Campaigns { get; set; } = null!;
    public DbSet<CampaignGroup> CampaignGroups { get; set; } = null!;
    public DbSet<CampaignReport> CampaignReports { get; set; } = null!;
    public DbSet<EmailTemplate> EmailTemplates { get; set; } = null!;

    // ===== Contact Module =====
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<ContactGroup> ContactGroups { get; set; } = null!;
    public DbSet<ContactUpload> ContactUploads { get; set; } = null!;
    public DbSet<ContactUploadFieldMap> ContactUploadFieldMaps { get; set; } = null!;
    public DbSet<ContactUploadGroup> ContactUploadGroups { get; set; } = null!;
    public DbSet<ContactValueMap> ContactValueMaps { get; set; } = null!;
    public DbSet<FieldMap> FieldMaps { get; set; } = null!;

    // ===== Groups Module =====
    public DbSet<Group> Groups { get; set; } = null!;

    // ===== Notifications Module =====
    public DbSet<SMTPConfig> SMTPConfigs { get; set; } = null!;

    // ===== FileProcessing Module =====
    public DbSet<DownloadQueue> DownloadQueues { get; set; } = null!;
    public DbSet<DownloadQueueSubEntity> DownloadQueueSubEntities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CampaignGroup>(entity =>
        {
            entity.HasKey(e => new { e.CampaignId, e.GroupId });
        });

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
