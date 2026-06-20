using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class ContactUploadFieldMapRepository : Repository<ContactUploadFieldMap, int, ApplicationDbContext>, IContactUploadFieldMapRepository
{
    public ContactUploadFieldMapRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


