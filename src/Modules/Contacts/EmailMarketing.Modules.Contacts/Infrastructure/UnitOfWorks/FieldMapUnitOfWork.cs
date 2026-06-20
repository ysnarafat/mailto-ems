using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Contacts.Repositories;
using EmailMarketing.Shared.Infrastructure;

namespace EmailMarketing.Modules.Contacts.UnitOfWorks;

public class FieldMapUnitOfWork : UnitOfWork, IFieldMapUnitOfWork
{
    public IFieldMapRepository FieldMapRepository { get; set; }

    public FieldMapUnitOfWork(ApplicationDbContext dbContext, IFieldMapRepository fieldMapRepository)
        : base(dbContext)
    {
        FieldMapRepository = fieldMapRepository;
    }
}

