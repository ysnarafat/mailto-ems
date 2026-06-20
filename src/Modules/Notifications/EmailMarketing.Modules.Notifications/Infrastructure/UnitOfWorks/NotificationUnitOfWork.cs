using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Notifications.Repositories;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Notifications.UnitOfWorks;

public class NotificationUnitOfWork : UnitOfWork, INotificationUnitOfWork
{
    public ISmtpConfigRepository SmtpConfigRepository { get; set; }

    public NotificationUnitOfWork(ApplicationDbContext dbContext, ISmtpConfigRepository smtpConfigRepository)
        : base(dbContext)
    {
        SmtpConfigRepository = smtpConfigRepository;
    }
}

