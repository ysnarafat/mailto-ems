using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Contacts.Repositories;

namespace EmailMarketing.Modules.Contacts.UnitOfWorks;

public interface IContactUnitOfWork : IUnitOfWork
{
    IContactRepository ContactRepository { get; set; }
    IContactUploadRepository ContactUploadRepository { get; set; }
    IFieldMapRepository FieldMapRepository { get; set; }
    IContactUploadFieldMapRepository ContactUploadFieldMapRepository { get; set; }
    IContactValueMapRepository ContactValueMapRepository { get; set; }
    IContactGroupRepository GroupContactRepository { get; set; }
}

