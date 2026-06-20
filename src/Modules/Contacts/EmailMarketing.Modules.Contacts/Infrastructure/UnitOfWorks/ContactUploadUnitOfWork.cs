using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Contacts.Repositories;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.UnitOfWorks;

public class ContactUploadUnitOfWork : UnitOfWork, IContactUploadUnitOfWork
{
    public IContactRepository ContactRepository { get; set; }
    public IContactUploadRepository ContactUploadRepository { get; set; }
    public IFieldMapRepository FieldMapRepository { get; set; }
    public IContactUploadFieldMapRepository ContactUploadFieldMapRepository { get; set; }
    public IContactValueMapRepository ContactValueMapRepository { get; set; }
    public IContactUploadGroupRepository ContactUploadGroupRepository { get; set; }
    public IContactGroupRepository ContactGroupRepository { get; set; }

    public ContactUploadUnitOfWork(
        ApplicationDbContext dbContext,
        IContactRepository contactRepository,
        IContactUploadRepository contactUploadRepository,
        IFieldMapRepository fieldMapRepository,
        IContactUploadFieldMapRepository contactUploadFieldMapRepository,
        IContactValueMapRepository contactValueMapRepository,
        IContactUploadGroupRepository contactUploadGroupRepository,
        IContactGroupRepository contactGroupRepository)
        : base(dbContext)
    {
        ContactRepository = contactRepository;
        ContactUploadRepository = contactUploadRepository;
        FieldMapRepository = fieldMapRepository;
        ContactUploadFieldMapRepository = contactUploadFieldMapRepository;
        ContactValueMapRepository = contactValueMapRepository;
        ContactUploadGroupRepository = contactUploadGroupRepository;
        ContactGroupRepository = contactGroupRepository;
    }
}

