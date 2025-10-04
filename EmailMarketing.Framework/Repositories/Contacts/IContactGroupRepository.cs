using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public interface IContactGroupRepository : IRepository<ContactGroup, int, FrameworkContext>
    {

    }
}
