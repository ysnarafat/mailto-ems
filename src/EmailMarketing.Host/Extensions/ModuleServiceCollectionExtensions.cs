using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Host.Extensions;

/// <summary>
/// Extension methods for registering modular services using reflection-based discovery.
/// This decouples the Host from specific module implementations and enables plugin architecture.
/// </summary>
public static class ModuleServiceCollectionExtensions
{
    /// <summary>
    /// Discovers and registers all modules that implement IModule interface.
    /// Uses reflection to find modules at runtime, avoiding circular dependencies.
    /// </summary>
    public static IServiceCollection AddModules(this IServiceCollection services)
    {
        // Discover all types implementing IModule interface across all loaded assemblies
        var moduleTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => GetLoadableTypes(assembly))
            .Where(type =>
                typeof(IModule).IsAssignableFrom(type) &&
                !type.IsInterface &&
                !type.IsAbstract)
            .ToList();

        // Instantiate and register each module
        foreach (var moduleType in moduleTypes)
        {
            var instance = (IModule)Activator.CreateInstance(moduleType)
                ?? throw new InvalidOperationException($"Failed to instantiate module {moduleType.Name}");

            instance.RegisterServices(services);
        }

        return services;
    }

    /// <summary>
    /// Gets all loadable types from an assembly, handling any type loading errors gracefully.
    /// </summary>
    private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null)!;
        }
    }
}
