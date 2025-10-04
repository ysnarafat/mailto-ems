using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class GroupContactRepository : Repository<ContactGroup, int, FrameworkContext>, IGroupContactRepository
    {
        public GroupContactRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
