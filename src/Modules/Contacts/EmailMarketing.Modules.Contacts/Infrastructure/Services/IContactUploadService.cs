using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;

namespace EmailMarketing.Modules.Contacts.Services;

public interface IContactUploadService : IDisposable
{
    Task<(int SucceedCount, int ExistCount, int InvalidCount)> ContactExcelImportAsync(ContactUpload contactUpload);
    Task<(int SucceedCount, int ExistCount, int InvalidCount)> ContactExcelImportAsync(int contactUploadId);
    Task<IList<(int Value, string Text, bool IsStandard)>> GetAllFieldMapForSelectAsync(Guid? userId);
    Task AddContactUploadAsync(ContactUpload entity);
    Task<bool> IsSelectedEmailFieldMap(IList<int> values);
    Task<IList<ContactUpload>> GetUploadedContact();
    Task<(IList<ContactUpload> Items, int Total, int TotalFilter)> GetAllAsync(
        Guid? userId, string searchText, string orderBy, int pageIndex, int pageSize);
    Task<ContactUpload> GetByIdAsync(int id);
    Task UpdateAsync(ContactUpload entity);
}

