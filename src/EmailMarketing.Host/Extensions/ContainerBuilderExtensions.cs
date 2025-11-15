using Autofac;
using EmailMarketing.Modules.Campaigns;
using EmailMarketing.Modules.Contacts;
using EmailMarketing.Modules.FileProcessing;
using EmailMarketing.Modules.Notifications;
using EmailMarketing.Modules.Users;
using EmailMarketing.Shared.Abstractions;

namespace EmailMarketing.Host.Extensions;

public static class ContainerBuilderExtensions
{
    public static void RegisterModules(this ContainerBuilder builder, IConfiguration configuration)
    {
        // Register all modules
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
            builder.RegisterModule(new ModuleRegistration(module));
        }
    }
}

public class ModuleRegistration : Autofac.Module
{
    private readonly IModule _module;

    public ModuleRegistration(IModule module)
    {
        _module = module;
    }

    protected override void Load(ContainerBuilder builder)
    {
        // Module-specific registrations would go here
        // For now, we'll use the IServiceCollection approach
    }
}