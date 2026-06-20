using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class ContactGroupRepository : Repository<ContactGroup, int, ApplicationDbContext>, IContactGroupRepository
{
    public ContactGroupRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


