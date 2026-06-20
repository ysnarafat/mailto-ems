# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**MailTo EMS** is an Email Marketing Software built with a **modular monolith** architecture in .NET 10. It manages contacts, campaigns, email sending, and file import/export through loosely-coupled modules that communicate via MediatR events.

## Technology Stack

- **Framework**: ASP.NET Core 10.0
- **Database**: SQL Server with Entity Framework Core
- **DI Container**: Autofac (bridged from `IServiceCollection`)
- **Event Handling**: MediatR (`INotification` / `INotificationHandler`)
- **Logging**: Serilog (configured in `Program.cs`, driven by `appsettings.json`)

## Solution File

`EmailMarketing.ModularMonolith.sln` — the only active solution.

## Architecture

```
src/
├── EmailMarketing.Host/              # ASP.NET Core entry point
├── EmailMarketing.Shared/
│   ├── Shared.Abstractions/          # IModule, IDomainEvent, IIntegrationEvent, service interfaces
│   ├── Shared.Domain/                # Entity, AggregateRoot base classes
│   └── Shared.Infrastructure/        # ApplicationDbContext, Identity, Repository, UoW, extensions
└── Modules/
    ├── Users/                        # Auth, identity management
    ├── Contacts/                     # Contact CRUD, upload, field maps, export
    ├── Groups/                       # Group management
    ├── Campaigns/                    # Campaign lifecycle
    ├── FileProcessing/               # Excel import/export queue
    └── Notifications/                # SMTP config, email sending
```

### Module Internal Layout

Every module follows this folder convention:

```
EmailMarketing.Modules.{Name}/
  Infrastructure/
    Repositories/     # Repository interfaces + implementations
    Services/         # Service interfaces + implementations
    UnitOfWorks/      # UnitOfWork interfaces + implementations
  Domain/
    Events/           # IDomainEvent / IIntegrationEvent implementations
  Application/
    Commands/         # MediatR IRequest command objects
    Queries/          # MediatR IRequest query objects
    Handlers/         # IRequestHandler / INotificationHandler implementations
  {Name}Module.cs     # IModule implementation — root of module
```

### Module Registration

Modules are **auto-discovered at runtime via reflection** — `AddModules()` in `Program.cs` scans all loaded assemblies for `IModule` implementations and calls `RegisterServices()` on each.

To add a new module:
1. Create `src/Modules/{Name}/EmailMarketing.Modules.{Name}/`
2. Create `.csproj` referencing `Shared.Abstractions`, `Shared.Domain`, `Shared.Infrastructure`
3. Implement `IModule`:
   ```csharp
   public class {Name}Module : IModule
   {
       public string Name => "{Name}";
       public void RegisterServices(IServiceCollection services)
       {
           services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof({Name}Module).Assembly));
           // register repos, services, UoW here
       }
   }
   ```
4. Add `<ProjectReference>` to `EmailMarketing.Host.csproj` (so the assembly is loaded at startup)
5. Add the project to `EmailMarketing.ModularMonolith.sln`

No manual registration in `ContainerBuilderExtensions` needed — reflection handles discovery.

### Database & Entities

Single shared `ApplicationDbContext` (in `Shared.Infrastructure`). All entity `DbSet<>` properties live there.

Entity base classes (in `Shared.Infrastructure.Data`):
- `IEntity<TKey>` — `Id`, `IsDeleted`, `IsActive`
- `IAuditableEntity<TKey>` — extends `IEntity<TKey>` + `CreatedBy`, `Created`, `LastModifiedBy`, `LastModified`

All module entities extend one of these and live in `Shared.Infrastructure/Data/Entities/{Module}/`.

### Repository & Unit of Work Pattern

Every module repository extends the generic base:

```csharp
public class GroupRepository : Repository<Group, int, ApplicationDbContext>, IGroupRepository
{
    public GroupRepository(ApplicationDbContext dbContext) : base(dbContext) {}
}
```

`Repository<TEntity, TKey, TContext>` is in `Shared.Infrastructure.Data` and requires `TEntity : IEntity<TKey>`.

`IUnitOfWork` / `UnitOfWork` (also in `Shared.Infrastructure.Data`) wrap the DbContext's save/transaction operations. Each module defines its own `I{Module}UnitOfWork` that composes its repositories.

### Cross-Module Data Access

**No direct `ProjectReference` between modules.** When a module needs to read another module's data:

- Create a read-only repository in the consuming module (e.g., `IGroupReadRepository` in Contacts)
- It extends `Repository<Group, int, ApplicationDbContext>` using the shared DbContext
- Register it in the consuming module's `RegisterServices()`

Example: `Contacts` reads `Group` data via its own `GroupReadRepository`, not via the Groups module.

### Inter-Module Events

For write-side cross-module communication use MediatR events:
- `IDomainEvent` (`INotification`) — intra-module, synchronous
- `IIntegrationEvent` — cross-module, published via `IMediator.Publish()`

Event definitions go in `Domain/Events/`, handlers in `Application/Handlers/`.

## Development Commands

```bash
# Build
dotnet build EmailMarketing.ModularMonolith.sln

# Run
dotnet run --project src/EmailMarketing.Host/EmailMarketing.Host.csproj

# Test (legacy test projects — modular tests not yet created)
dotnet test test/EmailMarketing.Framework.Tests/EmailMarketing.Framework.Tests.csproj
dotnet test --filter "FullyQualifiedName~TestClassName"

# Docker
docker-compose -f docker-compose.modular.yml up --build

# EF migrations
dotnet ef migrations add MigrationName --project src/EmailMarketing.Host/EmailMarketing.Host.csproj
dotnet ef database update --project src/EmailMarketing.Host/EmailMarketing.Host.csproj
```

## Configuration

`src/EmailMarketing.Host/appsettings.json` — connection string (`DefaultConnection`), SMTP, Serilog sinks.

## Claude Code Skills

### `/validate-modular-monolith`

Validates code against modular monolith patterns:
- No cross-module `ProjectReference`
- Event-driven cross-module communication
- Services registered within their own module
- Entities and repositories in correct locations

Use before committing or submitting a PR. See `.claude/skills/README.md` for details.

