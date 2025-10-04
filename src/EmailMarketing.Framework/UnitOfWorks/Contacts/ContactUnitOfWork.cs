using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Repositories.Contacts;

namespace EmailMarketing.Framework.UnitOfWorks.Contacts
{
    public class ContactUnitOfWork : EmailMarketing.Data.UnitOfWork, IContactUnitOfWork
    {
        public IContactRepository ContactRepository { get; set; }
        public IContactUploadRepository ContactUploadRepository { get; set; }
        public IFieldMapRepository FieldMapRepository { get; set; }
        public IContactUploadFieldMapRepository ContactUploadFieldMapRepository { get; set; }
        public IContactValueMapRepository ContactValueMapRepository { get; set; }

        public IGroupContactRepository GroupContactRepository { get; set; }

        public ContactUnitOfWork(FrameworkContext dbContext,
            IContactRepository contactRepository,
            IContactUploadRepository contactUploadRepository,
            IFieldMapRepository fieldMapRepository,
            IContactUploadFieldMapRepository contactUploadFieldMapRepository,
            IContactValueMapRepository contactValueMapRepository,
            IGroupContactRepository groupContactRepository) : base(dbContext)
        {
            this.ContactRepository = contactRepository;
            this.ContactUploadRepository = contactUploadRepository;
            this.FieldMapRepository = fieldMapRepository;
            this.ContactUploadFieldMapRepository = contactUploadFieldMapRepository;
            this.ContactValueMapRepository = contactValueMapRepository;
            this.GroupContactRepository = groupContactRepository;
        }
    }
}
