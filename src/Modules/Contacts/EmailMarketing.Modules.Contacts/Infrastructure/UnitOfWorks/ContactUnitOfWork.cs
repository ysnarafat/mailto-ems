using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Contacts.Repositories;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.UnitOfWorks;

public class ContactUnitOfWork : UnitOfWork, IContactUnitOfWork
{
    public IContactRepository ContactRepository { get; set; }
    public IContactUploadRepository ContactUploadRepository { get; set; }
    public IFieldMapRepository FieldMapRepository { get; set; }
    public IContactUploadFieldMapRepository ContactUploadFieldMapRepository { get; set; }
    public IContactValueMapRepository ContactValueMapRepository { get; set; }
    public IContactGroupRepository GroupContactRepository { get; set; }

    public ContactUnitOfWork(
        ApplicationDbContext dbContext,
        IContactRepository contactRepository,
        IContactUploadRepository contactUploadRepository,
        IFieldMapRepository fieldMapRepository,
        IContactUploadFieldMapRepository contactUploadFieldMapRepository,
        IContactValueMapRepository contactValueMapRepository,
        IContactGroupRepository groupContactRepository)
        : base(dbContext)
    {
        ContactRepository = contactRepository;
        ContactUploadRepository = contactUploadRepository;
        FieldMapRepository = fieldMapRepository;
        ContactUploadFieldMapRepository = contactUploadFieldMapRepository;
        ContactValueMapRepository = contactValueMapRepository;
        GroupContactRepository = groupContactRepository;
    }
}

