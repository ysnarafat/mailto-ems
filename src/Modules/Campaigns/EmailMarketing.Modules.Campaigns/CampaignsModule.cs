using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Modules.Campaigns;

public class CampaignsModule : IModule
{
    public string Name => "Campaigns";

    public void RegisterServices(IServiceCollection services)
    {
        // Register campaign-related services
        // services.AddScoped<ICampaignService, CampaignService>();
        // services.AddScoped<ICampaignRepository, CampaignRepository>();
        // services.AddScoped<ICampaignReportService, CampaignReportService>();
        
        // Register MediatR handlers for this module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CampaignsModule).Assembly));
    }
}