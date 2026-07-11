using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Groups.Repositories;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Groups.UnitOfWorks;

public class GroupUnitOfWork : UnitOfWork, IGroupUnitOfWork
{
    public IGroupRepository GroupRepository { get; set; }

    public GroupUnitOfWork(ApplicationDbContext dbContext, IGroupRepository groupRepository)
        : base(dbContext)
    {
        GroupRepository = groupRepository;
    }
}

