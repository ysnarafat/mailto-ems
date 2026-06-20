using System;
using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Notifications;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Notifications.Repositories;

public interface ISmtpConfigRepository : IRepository<SMTPConfig, Guid, ApplicationDbContext>
{
}


