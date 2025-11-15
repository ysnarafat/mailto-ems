using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Modules.Contacts;

public class ContactsModule : IModule
{
    public string Name => "Contacts";

    public void RegisterServices(IServiceCollection services)
    {
        // Register contact-related services
        // services.AddScoped<IContactService, ContactService>();
        // services.AddScoped<IContactRepository, ContactRepository>();
        // services.AddScoped<IGroupService, GroupService>();
        
        // Register MediatR handlers for this module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ContactsModule).Assembly));
    }
}