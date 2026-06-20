using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Groups.Repositories;

public interface IGroupRepository : IRepository<Group, int, ApplicationDbContext>
{
}


