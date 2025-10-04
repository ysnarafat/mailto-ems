using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.Contacts;

namespace EmailMarketing.Framework.UnitOfWorks.Contacts
{
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
}
