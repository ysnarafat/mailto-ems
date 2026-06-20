using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailMarketing.Shared.Infrastructure;
using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Groups;
using Microsoft.EntityFrameworkCore;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class GroupReadRepository : Repository<Group, int, ApplicationDbContext>, IGroupReadRepository
{
    public GroupReadRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IList<(int Value, string Text, int Count)>> GetGroupsForSelectAsync(Guid? userId)
    {
        var result = await _dbContext.Groups
            .Where(g => !g.IsDeleted && g.IsActive && (!userId.HasValue || g.UserId == userId.Value))
            .OrderBy(g => g.Name)
            .Select(g => new { g.Id, g.Name, Count = g.ContactGroups.Count })
            .ToListAsync();

        return result.Select(x => (Value: x.Id, Text: x.Name, Count: x.Count)).ToList();
    }
}
