using EmailMarketing.Modules.Campaigns;
using EmailMarketing.Modules.Contacts;
using EmailMarketing.Modules.FileProcessing;
using EmailMarketing.Modules.Notifications;
using EmailMarketing.Modules.Users;
using EmailMarketing.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Shared.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Entity Framework
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Add Identity
        services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // Add MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        // Register all modules
        RegisterModules(services);

        return services;
    }

    private static void RegisterModules(IServiceCollection services)
    {
        var modules = new List<IModule>
        {
            new UsersModule(),
            new ContactsModule(),
            new CampaignsModule(),
            new FileProcessingModule(),
            new NotificationsModule()
        };

        foreach (var module in modules)
        {
            module.RegisterServices(services);
        }
    }
}

// Placeholder classes - these would be moved from existing projects
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets would be defined here
}

public class ApplicationUser
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}