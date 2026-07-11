using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Contacts.Repositories;

namespace EmailMarketing.Modules.Contacts.UnitOfWorks;

public interface IContactUploadUnitOfWork : IUnitOfWork
{
    IContactRepository ContactRepository { get; set; }
    IContactUploadRepository ContactUploadRepository { get; set; }
    IFieldMapRepository FieldMapRepository { get; set; }
    IContactUploadFieldMapRepository ContactUploadFieldMapRepository { get; set; }
    IContactValueMapRepository ContactValueMapRepository { get; set; }
    IContactUploadGroupRepository ContactUploadGroupRepository { get; set; }
    IContactGroupRepository ContactGroupRepository { get; set; }
}

