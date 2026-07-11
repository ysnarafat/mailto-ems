using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Notifications.Repositories;

namespace EmailMarketing.Modules.Notifications.UnitOfWorks;

public interface INotificationUnitOfWork : IUnitOfWork
{
    ISmtpConfigRepository SmtpConfigRepository { get; set; }
}

