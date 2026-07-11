using EmailMarketing.Shared.Infrastructure.Data;
using EmailMarketing.Modules.Contacts.Repositories;

namespace EmailMarketing.Modules.Contacts.UnitOfWorks;

public interface IFieldMapUnitOfWork : IUnitOfWork
{
    IFieldMapRepository FieldMapRepository { get; set; }
}

