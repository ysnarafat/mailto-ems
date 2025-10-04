using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class ContactUploadGroupRepository : Repository<ContactUploadGroup, int, FrameworkContext>, IContactUploadGroupRepository
    {
        public ContactUploadGroupRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
