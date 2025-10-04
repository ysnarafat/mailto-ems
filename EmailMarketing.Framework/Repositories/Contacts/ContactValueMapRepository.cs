using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class ContactValueMapRepository : Repository<ContactValueMap, int, FrameworkContext>, IContactValueMapRepository
    {
        public ContactValueMapRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
