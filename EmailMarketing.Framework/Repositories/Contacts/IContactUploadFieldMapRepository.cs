using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public interface IContactUploadFieldMapRepository : IRepository<ContactUploadFieldMap, int, FrameworkContext>
    {

    }
}
