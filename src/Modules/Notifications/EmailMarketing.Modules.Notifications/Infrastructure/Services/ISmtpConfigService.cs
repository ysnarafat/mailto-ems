using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Notifications;

namespace EmailMarketing.Modules.Notifications.Services;

public interface ISmtpConfigService : IDisposable
{
    Task<(IList<SMTPConfig> Items, int Total, int TotalFilter)> GetAllAsync(
        Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize);

    Task<SMTPConfig> GetByIdAsync(Guid id);

    Task AddAsync(SMTPConfig entity);

    Task UpdateAsync(SMTPConfig entity);

    Task<SMTPConfig> ActivateSmtpAsync(Guid id);

    Task<IList<SMTPConfig>> GetAllSmtpConfigAsync(Guid? userId);
}

