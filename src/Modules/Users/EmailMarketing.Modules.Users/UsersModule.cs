using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Modules.Users;

public class UsersModule : IModule
{
    public string Name => "Users";

    public void RegisterServices(IServiceCollection services)
    {
        // Register user-related services
        // services.AddScoped<IUserService, UserService>();
        // services.AddScoped<IUserRepository, UserRepository>();
        
        // Register MediatR handlers for this module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UsersModule).Assembly));
    }
}