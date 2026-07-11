using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class ContactValueMapRepository : Repository<ContactValueMap, int, ApplicationDbContext>, IContactValueMapRepository
{
    public ContactValueMapRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


