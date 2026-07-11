using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Host.Extensions;

public static class ContainerBuilderExtensions
{
    /// <summary>
    /// Registers all discovered modules with the Autofac container.
    /// This bridges the gap between IServiceCollection (Microsoft DI) and Autofac container.
    /// </summary>
    public static void RegisterModules(this ContainerBuilder builder, IServiceCollection services)
    {
        // Discover all module types using the same reflection-based approach as IServiceCollection
        var moduleTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => GetLoadableTypes(assembly))
            .Where(type =>
                typeof(IModule).IsAssignableFrom(type) &&
                !type.IsInterface &&
                !type.IsAbstract)
            .ToList();

        // For each module, instantiate it and let it register with both IServiceCollection and Autofac
        foreach (var moduleType in moduleTypes)
        {
            var instance = (IModule)Activator.CreateInstance(moduleType)
                ?? throw new InvalidOperationException($"Failed to instantiate module {moduleType.Name}");

            // Module registers with IServiceCollection (handled separately in Program.cs)
            // Here we just register the module itself in Autofac for reference
            builder.RegisterInstance(instance).As<IModule>();
        }
    }

    private static IEnumerable<Type> GetLoadableTypes(System.Reflection.Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (System.Reflection.ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null)!;
        }
    }
}