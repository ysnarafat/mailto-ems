using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class ContactUploadGroupRepository : Repository<ContactUploadGroup, int, ApplicationDbContext>, IContactUploadGroupRepository
{
    public ContactUploadGroupRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


