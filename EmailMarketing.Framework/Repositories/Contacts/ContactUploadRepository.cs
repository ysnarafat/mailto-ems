using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class ContactUploadRepository : Repository<ContactUpload, int, FrameworkContext>, IContactUploadRepository
    {
        public ContactUploadRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
