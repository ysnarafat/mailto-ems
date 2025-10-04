using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class ContactRepository : Repository<Contact, int, FrameworkContext>, IContactRepository
    {
        public ContactRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
