using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EmailMarketing.Host.Extensions;
using EmailMarketing.Shared.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Use Autofac as the service provider
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Configure services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add shared infrastructure (DbContext, Identity, base MediatR)
builder.Services.AddSharedInfrastructure(builder.Configuration);

// Discover and register all modules via reflection (NO CIRCULAR DEPENDENCY)
builder.Services.AddModules();

// Configure Autofac container to include registered services
builder.Host.ConfigureContainer<ContainerBuilder>((context, containerBuilder) =>
{
    // Register modules in Autofac for any Autofac-specific needs
    containerBuilder.RegisterModules(builder.Services);

    // Populate Autofac from IServiceCollection registrations
    containerBuilder.Populate(builder.Services);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow }));

try
{
    Log.Information("Starting EmailMarketing Host application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}