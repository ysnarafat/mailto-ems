using EmailMarketing.Framework.Context;
using EmailMarketing.Framework.Repositories.Contacts;

namespace EmailMarketing.Framework.UnitOfWorks.Contacts
{
    public class FieldMapUnitOfWork : EmailMarketing.Data.UnitOfWork, IFieldMapUnitOfWork
    {
        public IFieldMapRepository FieldMapRepository { get; set; }
        public FieldMapUnitOfWork(FrameworkContext dbContext, IFieldMapRepository fieldMapRepository) : base(dbContext)
        {
            FieldMapRepository = fieldMapRepository;
        }


    }
}
