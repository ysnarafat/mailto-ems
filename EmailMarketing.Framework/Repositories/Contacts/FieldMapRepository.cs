using EmailMarketing.Data;
using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Entities.Contacts;

namespace EmailMarketing.Framework.Repositories.Contacts
{
    public class FieldMapRepository : Repository<FieldMap, int, FrameworkContext>, IFieldMapRepository
    {
        public FieldMapRepository(FrameworkContext dbContext)
            : base(dbContext)
        {

        }
    }
}
