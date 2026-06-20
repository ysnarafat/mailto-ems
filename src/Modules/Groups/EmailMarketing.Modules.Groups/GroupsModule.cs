using EmailMarketing.Modules.Groups.Repositories;
using EmailMarketing.Modules.Groups.Services;
using EmailMarketing.Modules.Groups.UnitOfWorks;
using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Modules.Groups;

public class GroupsModule : IModule
{
    public string Name => "Groups";

    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IGroupUnitOfWork, GroupUnitOfWork>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GroupsModule).Assembly));
    }
}
