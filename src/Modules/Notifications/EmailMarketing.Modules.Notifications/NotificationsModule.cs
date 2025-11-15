using EmailMarketing.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace EmailMarketing.Modules.Notifications;

public class NotificationsModule : IModule
{
    public string Name => "Notifications";

    public void RegisterServices(IServiceCollection services)
    {
        // Register notification services
        // services.AddScoped<IEmailService, EmailService>();
        // services.AddScoped<INotificationService, NotificationService>();
        // services.AddScoped<ISmtpService, SmtpService>();
        
        // Register background services
        // services.AddHostedService<EmailSendingWorkerService>();
        
        // Register MediatR handlers for this module
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(NotificationsModule).Assembly));
    }
}