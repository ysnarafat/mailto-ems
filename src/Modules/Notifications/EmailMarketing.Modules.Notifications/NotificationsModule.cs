using EmailMarketing.Modules.Notifications.Repositories;
using EmailMarketing.Modules.Notifications.Services;
using EmailMarketing.Modules.Notifications.UnitOfWorks;
using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Modules.Notifications;

public class NotificationsModule : IModule
{
    public string Name => "Notifications";

    public void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ISmtpConfigRepository, SmtpConfigRepository>();
        services.AddScoped<INotificationUnitOfWork, NotificationUnitOfWork>();
        services.AddScoped<ISmtpConfigService, SmtpConfigService>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(NotificationsModule).Assembly));
    }
}