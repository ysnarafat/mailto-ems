using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class ContactRepository : Repository<Contact, int, ApplicationDbContext>, IContactRepository
{
    public ContactRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


