using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class ContactUploadFieldMapRepository : Repository<ContactUploadFieldMap, int, FrameworkContext>, IContactUploadFieldMapRepository
    {
        public ContactUploadFieldMapRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
