# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**MailTo EMS** is an Email Marketing Software built with a **modular monolith** architecture in .NET 10. It manages contacts, campaigns, email sending, and file import/export operations through loosely-coupled modules that communicate via domain events.

## Technology Stack

- **Framework**: ASP.NET Core 10.0
- **Database**: SQL Server with Entity Framework Core
- **DI Container**: Autofac
- **Event Handling**: MediatR
- **Logging**: Serilog
- **Testing**: xUnit (with existing tests in old structure)

## Solution Structure

Two solution files exist during the modular monolith migration:

- **EmailMarketing.sln** - Legacy monolithic structure (older projects with services and workers)
- **EmailMarketing.ModularMonolith.sln** - New modular structure (active development)

### Modular Monolith Architecture

```
src/
├── EmailMarketing.Host/              # Entry point (ASP.NET Core app)
├── EmailMarketing.Shared/            # Shared kernel (contracts, domain, infrastructure)
│   ├── Abstractions/                 # IModule, event contracts
│   ├── Domain/                       # Base entities, aggregate roots
│   └── Infrastructure/               # DbContext, service collection setup
└── Modules/                          # Business modules (independent, event-driven)
    ├── Users/                        # Authentication, authorization
    ├── Contacts/                     # Contact and group management
    ├── Campaigns/                    # Email campaigns
    ├── FileProcessing/               # Excel import/export
    └── Notifications/                # Email sending
```

## Key Architecture Concepts

### Module Structure

Each module implements `IModule` interface and registers its services via `RegisterServices()`:

```csharp
public class YourModule : IModule
{
    public string Name => "YourModule";
    
    public void RegisterServices(IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(YourModule).Assembly));
        // Register module services
    }
}
```

### Inter-Module Communication

Modules communicate exclusively through **domain events** published via MediatR:

- **Domain Events** (INotification) - Local events within a module
- **Integration Events** (IIntegrationEvent) - Cross-module events

Modules are registered in `src/EmailMarketing.Host/Extensions/ContainerBuilderExtensions.cs`. **To add a new module: import it and instantiate it in the modules list.**

### Database Access

Single shared DbContext (`ApplicationDbContext` in `Shared.Infrastructure`). Each module can have its own data models, but they share the same database context.

## Development Commands

### Build
```bash
dotnet build EmailMarketing.ModularMonolith.sln
```

### Run (Development)
```bash
dotnet run --project src/EmailMarketing.Host/EmailMarketing.Host.csproj
```

The application starts at `http://localhost:5000` (HTTPS) by default.

### Run Tests
```bash
# All tests
dotnet test EmailMarketing.ModularMonolith.sln

# Specific test project
dotnet test test/EmailMarketing.Framework.Tests/EmailMarketing.Framework.Tests.csproj

# Run single test
dotnet test --filter "FullyQualifiedName~TestClassName"
```

### Run with Docker
```bash
docker-compose -f docker-compose.modular.yml up --build
```

### Database Migrations (if using Code-First)
```bash
# Add migration
dotnet ef migrations add MigrationName --project src/EmailMarketing.Host/EmailMarketing.Host.csproj

# Update database
dotnet ef database update --project src/EmailMarketing.Host/EmailMarketing.Host.csproj
```

## Adding a New Module

1. Create folder structure: `src/Modules/ModuleName/EmailMarketing.Modules.ModuleName/`
2. Create `.csproj` file (reference `Shared.Abstractions` and `Shared.Domain`)
3. Implement `IModule` interface in a `ModuleNameModule.cs` class
4. Register MediatR in `RegisterServices()`
5. Add project reference to `EmailMarketing.Host.csproj`
6. Import and instantiate in `ContainerBuilderExtensions.RegisterModules()`

## Configuration

- **appsettings.json** in `EmailMarketing.Host` - Database connection string, SMTP, logging
- Connection string key: `DefaultConnection`
- Environment-specific overrides: `appsettings.Development.json`, `appsettings.Production.json`

## Testing Strategy

- **Unit tests** - Individual module logic (mock dependencies)
- **Integration tests** - Module interactions and event handling
- **Test projects** - Legacy structure has `EmailMarketing.*.Tests` projects (being modernized)

When writing tests for modules, ensure handlers are registered via MediatR and DbContext is properly seeded.

## Important Notes

- **No direct module dependencies** - Use events for cross-module communication
- **Single deployable unit** - Modular architecture supports future microservices extraction
- **Migration in progress** - Codebase is transitioning from monolithic to modular structure; both old and new projects may coexist temporarily
- **Autofac + IServiceCollection** - Both DI patterns are in use; understand both registration methods
- **Serilog logging** - Configured in `Program.cs`; check `appsettings.json` for log levels and sinks

## Claude Code Skills

Custom skills are available to assist with development tasks. These skills provide specialized validation and workflows.

### validate-modular-monolith

Validates code changes against modular monolith architecture patterns and the project's specific structure.

**Usage:**
```
/validate-modular-monolith

[Paste your code to validate]
```

**What it checks:**
- Module boundaries and independence (no cross-module dependencies)
- Event-driven communication (proper MediatR usage)
- DI registration patterns (services registered in their module)
- Shared kernel adherence (business logic stays in modules)
- Project structure alignment with established conventions

**When to use:**
- Before committing code changes
- When submitting a pull request
- When reviewing module code for architectural compliance

**See:** `.claude/skills/README.md` for detailed documentation and examples.

## Useful References

- See `MODULAR_MONOLITH.md` for detailed architecture decisions and migration phases
- See `README.md` for feature overview and deployment instructions
- See `.claude/skills/README.md` for available Claude Code skills and usage
