using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class ContactGroupRepository : Repository<ContactGroup, int, FrameworkContext>, IContactGroupRepository
    {
        public ContactGroupRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
