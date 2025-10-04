using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Repositories.Groups;

namespace EmailMarketing.Framework.UnitOfWorks.Groups
{
    public class GroupUnitOfWork : EmailMarketing.Data.UnitOfWork, IGroupUnitOfWork
    {
        public IGroupRepository GroupRepository { get; set; }
        public GroupUnitOfWork(FrameworkContext dbContext, IGroupRepository groupRepository) : base(dbContext)
        {
            GroupRepository = groupRepository;
        }


    }
}
