using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.Contacts;

namespace EmailMarketing.Framework.UnitOfWorks.Contacts
{
    public interface IFieldMapUnitOfWork : IUnitOfWork
    {
        IFieldMapRepository FieldMapRepository { get; set; }
    }
}
