using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Groups;

namespace EmailMarketing.Framework.Repositories.Groups
{
    public class GroupRepository : Repository<Group, int, FrameworkContext>, IGroupRepository
    {
        public GroupRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
