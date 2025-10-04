using EmailMarketing.Data;
using EmailMarketing.Framework.Repositories.Groups;

namespace EmailMarketing.Framework.UnitOfWorks.Groups
{
    public interface IGroupUnitOfWork : IUnitOfWork
    {
        IGroupRepository GroupRepository { get; set; }
    }
}
