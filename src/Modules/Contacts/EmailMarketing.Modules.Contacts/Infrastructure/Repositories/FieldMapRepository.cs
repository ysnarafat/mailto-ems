using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Shared.Infrastructure.Data.Entities.Contacts;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.Repositories;

public class FieldMapRepository : Repository<FieldMap, int, ApplicationDbContext>, IFieldMapRepository
{
    public FieldMapRepository(ApplicationDbContext dbContext) : base(dbContext) { }
}


