using System;
using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Notifications;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Notifications.Repositories;

public class SmtpConfigRepository : Repository<SMTPConfig, Guid, ApplicationDbContext>, ISmtpConfigRepository
{
    public SmtpConfigRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}


