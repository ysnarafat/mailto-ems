using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public interface IContactUploadGroupRepository : IRepository<ContactUploadGroup, int, ApplicationDbContext>
{
}


