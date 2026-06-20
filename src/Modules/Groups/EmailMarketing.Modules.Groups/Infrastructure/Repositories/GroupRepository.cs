using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Groups.Repositories;

public class GroupRepository : Repository<Group, int, ApplicationDbContext>, IGroupRepository
{
    public GroupRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }
}


