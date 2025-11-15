using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Shared.Abstractions;

public interface IModule
{
    string Name { get; }
    void RegisterServices(IServiceCollection services);
}