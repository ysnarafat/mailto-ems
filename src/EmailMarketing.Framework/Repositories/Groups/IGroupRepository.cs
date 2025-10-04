using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Groups;

namespace EmailMarketing.Framework.Repositories.Groups
{
    public interface IGroupRepository : IRepository<Group, int, FrameworkContext>
    {
    }
}
