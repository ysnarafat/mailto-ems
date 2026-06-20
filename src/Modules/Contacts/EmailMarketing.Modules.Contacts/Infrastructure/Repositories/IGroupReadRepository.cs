using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;

namespace EmailMarketing.Modules.Contacts.Repositories;

public interface IGroupReadRepository : IRepository<Group, int, EmailMarketing.Shared.Infrastructure.ApplicationDbContext>
{
    Task<IList<(int Value, string Text, int Count)>> GetGroupsForSelectAsync(Guid? userId);
}
