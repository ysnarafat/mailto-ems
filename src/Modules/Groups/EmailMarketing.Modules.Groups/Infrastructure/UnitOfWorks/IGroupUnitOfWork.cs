using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Groups.Repositories;

namespace EmailMarketing.Modules.Groups.UnitOfWorks;

public interface IGroupUnitOfWork : IUnitOfWork
{
    IGroupRepository GroupRepository { get; set; }
}

