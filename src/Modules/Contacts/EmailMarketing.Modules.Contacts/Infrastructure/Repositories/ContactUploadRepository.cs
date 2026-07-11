using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class ContactUploadRepository : Repository<ContactUpload, int, ApplicationDbContext>, IContactUploadRepository
{
    public ContactUploadRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


