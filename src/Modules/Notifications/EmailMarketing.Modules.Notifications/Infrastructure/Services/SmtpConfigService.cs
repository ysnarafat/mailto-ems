using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmailMarketing.Shared.Infrastructure.Exceptions;
using EmailMarketing.Shared.Infrastructure.Extensions;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Notifications;
using EmailMarketing.Modules.Notifications.UnitOfWorks;

namespace EmailMarketing.Modules.Notifications.Services;

public class SmtpConfigService : ISmtpConfigService
{
    private readonly INotificationUnitOfWork _unitOfWork;

    public SmtpConfigService(INotificationUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(IList<SMTPConfig> Items, int Total, int TotalFilter)> GetAllAsync(
        Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize)
    {
        var columnsMap = new Dictionary<string, Expression<Func<SMTPConfig, object>>>
        {
            ["Server"] = v => v.Server,
            ["Port"] = v => v.Port,
            ["SenderName"] = v => v.SenderName,
            ["SenderEmail"] = v => v.SenderEmail,
            ["UserName"] = v => v.UserName,
            ["EnableSSL"] = v => v.EnableSSL
        };

        var result = await _unitOfWork.SmtpConfigRepository.GetAsync<SMTPConfig>(
            x => x,
            x => x.Server.Contains(searchText) && (!userId.HasValue || x.UserId == userId.Value),
            x => x.ApplyOrdering(columnsMap, orderBy),
            null,
            pageIndex, pageSize, true);

        result.Total = await _unitOfWork.SmtpConfigRepository.GetCountAsync(x => x.UserId == userId);

        return (result.Items, result.Total, result.TotalFilter);
    }

    public async Task<SMTPConfig> GetByIdAsync(Guid id)
    {
        return await _unitOfWork.SmtpConfigRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(SMTPConfig entity)
    {
        var isExists = await _unitOfWork.SmtpConfigRepository.IsExistsAsync(
            x => x.UserId == entity.UserId && x.SenderEmail == entity.SenderEmail && x.Id != entity.Id);

        if (isExists)
            throw new DuplicationException(nameof(entity.SenderEmail));

        await _unitOfWork.SmtpConfigRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateAsync(SMTPConfig entity)
    {
        var isExists = await _unitOfWork.SmtpConfigRepository.IsExistsAsync(
            x => x.UserId == entity.UserId && x.SenderEmail == entity.SenderEmail && x.Id != entity.Id);

        if (isExists)
            throw new DuplicationException(nameof(entity.SenderEmail));

        var updateEntity = await GetByIdAsync(entity.Id);
        updateEntity.Server = entity.Server;
        updateEntity.Port = entity.Port;
        updateEntity.SenderName = entity.SenderName;
        updateEntity.SenderEmail = entity.SenderEmail;
        updateEntity.UserName = entity.UserName;
        updateEntity.Password = entity.Password;
        updateEntity.EnableSSL = entity.EnableSSL;

        await _unitOfWork.SmtpConfigRepository.UpdateAsync(updateEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<SMTPConfig> ActivateSmtpAsync(Guid id)
    {
        var smtp = await GetByIdAsync(id);
        smtp.IsActive = !smtp.IsActive;
        await _unitOfWork.SmtpConfigRepository.UpdateAsync(smtp);
        await _unitOfWork.SaveChangesAsync();
        return smtp;
    }

    public async Task<IList<SMTPConfig>> GetAllSmtpConfigAsync(Guid? userId)
    {
        return await _unitOfWork.SmtpConfigRepository.GetAsync(
            x => x,
            x => !x.IsDeleted && x.IsActive && x.UserId == userId,
            null, null, true);
    }

    public void Dispose()
    {
        _unitOfWork?.Dispose();
    }
}


