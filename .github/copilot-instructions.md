# Copilot Instructions for MailTo EMS

This guide helps Copilot sessions work effectively in the MailTo EMS repository.

## Quick Commands

### Build and Run
```bash
# Build the modular monolith
dotnet build EmailMarketing.ModularMonolith.sln

# Run the application (starts at http://localhost:5000)
dotnet run --project src/EmailMarketing.Host/EmailMarketing.Host.csproj

# Run with Docker
docker-compose -f docker-compose.modular.yml up --build
```

### Testing
```bash
# Run all tests
dotnet test EmailMarketing.ModularMonolith.sln

# Run a specific test project
dotnet test test/EmailMarketing.Framework.Tests/EmailMarketing.Framework.Tests.csproj

# Run a single test by class name
dotnet test --filter "FullyQualifiedName~MyTestClassName"
```

### Database Migrations
```bash
# Add a migration
dotnet ef migrations add MigrationName --project src/EmailMarketing.Host/EmailMarketing.Host.csproj

# Apply migrations to database
dotnet ef database update --project src/EmailMarketing.Host/EmailMarketing.Host.csproj
```

### Linting and Code Quality
- The project uses standard .NET tooling (no custom linters configured)
- Code is formatted using built-in C# conventions
- Use Visual Studio / VS Code C# extensions for on-the-fly validation

## Architecture Overview

### Two Solution Files During Migration
- **EmailMarketing.ModularMonolith.sln** - Active development with new modular architecture
- **EmailMarketing.sln** - Legacy monolithic structure (being phased out)

**Most work should target EmailMarketing.ModularMonolith.sln**

### Modular Monolith Structure

The solution follows a modular monolith pattern with a shared kernel and independent business modules:

```
src/
├── EmailMarketing.Host/              # Main application entry point
├── EmailMarketing.Shared/            # Shared kernel (never for business logic)
│   ├── Abstractions/                 # IModule interface, event contracts
│   ├── Domain/                       # Base entities, aggregate roots
│   └── Infrastructure/               # DbContext, DI setup
└── Modules/                          # Business modules (independent, event-driven)
    ├── Users/                        # Authentication, user management
    ├── Contacts/                     # Contact and group management
    ├── Campaigns/                    # Email campaign management
    ├── FileProcessing/               # Excel import/export operations
    └── Notifications/                # Email sending and notifications
```

### Module Communication Pattern

**Modules never directly depend on each other.** They communicate exclusively through domain events published via MediatR:

- **Domain Events** (implement `INotification`) - Local events within a module
- **Integration Events** (implement `IIntegrationEvent`) - Cross-module events

Example:
```csharp
// A module publishes an event
await _mediator.Publish(new ContactImportedEvent { ContactId = contact.Id });

// Another module handles it
public class ContactImportedEventHandler : INotificationHandler<ContactImportedEvent>
{
    public async Task Handle(ContactImportedEvent notification, CancellationToken cancellationToken)
    {
        // Handle the event
    }
}
```

### Module Registration

All modules are registered in `src/EmailMarketing.Host/Extensions/ContainerBuilderExtensions.cs`. To add a new module:

1. Create folder: `src/Modules/ModuleName/EmailMarketing.Modules.ModuleName/`
2. Implement `IModule` interface with `RegisterServices()` method
3. Register MediatR: `services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ModuleNameModule).Assembly))`
4. Reference `Shared.Abstractions` and `Shared.Domain`
5. Add project reference to `EmailMarketing.Host.csproj`
6. Instantiate module in `ContainerBuilderExtensions.RegisterModules()`

## Key Conventions

### Database Access
- Single `ApplicationDbContext` in `EmailMarketing.Shared.Infrastructure`
- Each module defines its own domain models but uses the shared DbContext
- All database operations go through Entity Framework Core

### Dependency Injection
- Both Autofac and `IServiceCollection` patterns are used
- Services must be registered in their module's `RegisterServices()` method
- Never use static service locators; always inject dependencies

### Configuration
- **appsettings.json** in `src/EmailMarketing.Host/` contains:
  - `DefaultConnection` - SQL Server connection string
  - SMTP settings for email
  - Logging levels and sinks (Serilog)
- Environment-specific files: `appsettings.Development.json`, `appsettings.Production.json`

### Logging
- Project uses **Serilog** for structured logging
- Configured in `src/EmailMarketing.Host/Program.cs`
- Check `appsettings.json` for log levels and output sinks

### Testing
- **xUnit** is the test framework
- Test projects in `test/` folder
- For module tests: mock MediatR handlers, seed test data in DbContext
- **Recommendation**: Use the `validate-modular-monolith` custom skill before committing module code (see below)

## Important Patterns to Follow

### ✅ DO
- Publish domain events from your module; let other modules handle them
- Register all services in the module's `RegisterServices()` method
- Mock dependencies in unit tests
- Validate that module boundaries stay loose

### ❌ DON'T
- Add direct project-to-project references between modules (use events instead)
- Put business logic in the shared kernel (it should only contain contracts and base types)
- Use static service locators or `ServiceProvider.GetService()` directly
- Deploy modules independently (it's a single monolith)

## Custom Skills

### validate-modular-monolith
Validates code changes against the modular monolith architecture and project conventions.

**When to use:**
- Before committing module code
- When adding inter-module communication
- When creating new modules
- When modifying DI registration

**How to use:**
```
/validate-modular-monolith
[Paste your code]
```

**What it checks:**
- Module boundary violations (cross-module dependencies)
- Event-driven communication patterns
- DI registration in correct modules
- Shared kernel is not used for business logic
- Project structure alignment

## Further Reading

- **CLAUDE.md** - In-depth guidance for Claude Code (claude.ai/code)
- **MODULAR_MONOLITH.md** - Detailed architecture decisions, migration phases, and design rationale
- **README.md** - Feature overview and deployment instructions
- `.claude/skills/README.md` - Additional custom skill documentation
