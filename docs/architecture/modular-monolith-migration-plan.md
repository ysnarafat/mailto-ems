# Modular Monolith Migration Plan

## Overview

This document is a step-by-step implementation guide for restructuring mailto-ems from its current layered monolith into a **Modular Monolith with Vertical Slices**. It is written so a future Claude session can execute it without needing additional context.

---

## Current Architecture

```
Solution
├── EmailMarketing.Web           ← MVC host (controllers, page models, Startup)
├── EmailMarketing.Framework     ← All domain logic (services, repos, UoWs, entities)
├── EmailMarketing.Data          ← Generic repository + unit-of-work base classes
├── EmailMarketing.Common        ← Cross-cutting (ICurrentUserService, exceptions, etc.)
├── EmailMarketing.Membership    ← Identity (users, roles, managers, ApplicationDbContext)
└── 4× Worker Services           ← Background processing (email sending, Excel import/export)
```

### Problems with the current design
- No enforced module boundaries: any project can reference any other
- `FrameworkModule.cs` registers 30+ types in one flat list — no grouping
- Page models in the web project call Autofac's service locator directly (`Startup.AutofacContainer.Resolve<T>()`)
- Business logic leaks into page models (e.g. `CreateAdminUsersModel.cs`)
- Worker services are full copies of config/bootstrap code
- `FrameworkContext` owns all domain tables; one migration covers everything

---

## Target Architecture

```
Solution
├── src/
│   ├── Modules/
│   │   ├── Identity/           ← Users, roles, auth, email verification
│   │   ├── Contacts/           ← Contacts, field maps, groups, uploads, exports
│   │   ├── Campaigns/          ← Campaigns, email templates, reports, campaign export
│   │   └── Smtp/               ← SMTP configuration
│   ├── SharedKernel/           ← Base entities, exceptions, interfaces (replaces Common + Data)
│   └── Host/                   ← ASP.NET Core host (replaces Web — thin wiring layer only)
└── workers/                    ← Merged into Host as hosted services (IHostedService)
```

### Key principles
1. **Modules own their domain** — entities, commands/queries, handlers, validators, migrations, and Autofac registration live inside the module.
2. **Vertical slices** — inside each module, code is organized by feature, not by layer.
3. **No cross-module project references** — modules talk through contracts in `SharedKernel` or through an in-process event bus (`MediatR`).
4. **No service locator** — constructor injection everywhere; no `Startup.AutofacContainer.Resolve<T>()`.
5. **Host is a thin wire** — controllers call `ISender` (MediatR), nothing else.

---

## Module Boundaries

| Module | Owns | Exposes (contracts) |
|---|---|---|
| **Identity** | ApplicationUser, ApplicationRole, auth flows, email verification, ApplicationDbContext | `ICurrentUser`, `GetUserByIdQuery`, `UserCreatedEvent` |
| **Contacts** | Contact, FieldMap, ContactUpload, ContactGroup, ContactValueMap, DownloadQueue, contact import worker, contact export worker | `IContactExistsService`, `ContactCreatedEvent` |
| **Campaigns** | Campaign, CampaignReport, EmailTemplate, CampaignGroup, email-sending worker, campaign-export worker | `GetCampaignGroupsQuery`, `CampaignSentEvent` |
| **Smtp** | SMTPConfig | `ISMTPConfigProvider` |
| **SharedKernel** | `Entity<TKey>`, `AuditableEntity<TKey>`, `DomainException`, `DuplicationException`, `NotFoundException`, `IRepository<>`, `IUnitOfWork`, `IDateTime`, `ICurrentUser`, `IDomainEvent` | Everything |

---

## Step-by-Step Implementation

---

### PHASE 1 — Prepare SharedKernel

**Goal:** Create the foundation that all modules depend on. Nothing else changes.

#### Step 1.1 — Create `src/SharedKernel` project

Create a new class library `EmailMarketing.SharedKernel` targeting `net10.0`.

**Files to create:**

`Domain/Entity.cs`
```csharp
public abstract class Entity<TKey>
{
    public TKey Id { get; protected set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
}
```

`Domain/AuditableEntity.cs`
```csharp
public abstract class AuditableEntity<TKey> : Entity<TKey>
{
    public Guid? CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public Guid? LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
}
```

`Domain/IDomainEvent.cs`
```csharp
public interface IDomainEvent : INotification { }  // MediatR INotification
```

`Exceptions/DomainException.cs` — move from `EmailMarketing.Common.Exceptions`
`Exceptions/DuplicationException.cs`
`Exceptions/NotFoundException.cs`
`Exceptions/ValidationException.cs`

`Abstractions/ICurrentUser.cs`
```csharp
public interface ICurrentUser
{
    Guid? UserId { get; }
    bool IsAuthenticated { get; }
}
```

`Abstractions/IDateTime.cs`
```csharp
public interface IDateTime
{
    DateTime Now { get; }
}
```

`Abstractions/IRepository.cs` — copy generic interface from `EmailMarketing.Data.IRepository`

`Abstractions/IUnitOfWork.cs` — copy from `EmailMarketing.Data.IUnitOfWork`

**NuGet packages for SharedKernel:**
- `MediatR` — for `IDomainEvent : INotification`
- `Microsoft.EntityFrameworkCore` — for `IQueryable<T>` in repository abstractions

#### Step 1.2 — Verify nothing breaks

Run `dotnet build` — no changes to existing projects yet, so this is a free-standing addition.

---

### PHASE 2 — Create Module Scaffolding

**Goal:** Create the four module projects with their folder structure. No logic moved yet.

#### Step 2.1 — Create module projects

Create four new class library projects:
- `src/Modules/Identity/EmailMarketing.Identity.csproj`
- `src/Modules/Contacts/EmailMarketing.Contacts.csproj`
- `src/Modules/Campaigns/EmailMarketing.Campaigns.csproj`
- `src/Modules/Smtp/EmailMarketing.Smtp.csproj`

Each module gets the same internal folder structure:

```
Module/
├── Domain/               ← Entities and domain logic
├── Features/             ← Vertical slices (one folder per feature)
│   └── FeatureName/
│       ├── Command.cs (or Query.cs)
│       ├── Handler.cs
│       ├── Validator.cs
│       └── Response.cs
├── Infrastructure/
│   ├── Persistence/
│   │   ├── ModuleDbContext.cs
│   │   ├── Migrations/
│   │   └── Configurations/   ← EF entity type configurations
│   └── Repositories/
├── Contracts/            ← Interfaces this module exposes to other modules
└── ModuleServiceExtensions.cs  ← IServiceCollection extension that wires the module
```

**Project references per module:**
- All modules reference `EmailMarketing.SharedKernel`
- `EmailMarketing.Campaigns` references `EmailMarketing.Contacts.Contracts` and `EmailMarketing.Smtp.Contracts` (for cross-module contracts only)
- No module references another module's `Domain`, `Features`, or `Infrastructure`

#### Step 2.2 — Create module registration pattern

Each module exposes one extension method:

```csharp
// EmailMarketing.Contacts/ModuleServiceExtensions.cs
public static class ContactsModule
{
    public static IServiceCollection AddContactsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ContactsDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ContactsModule).Assembly));

        services.AddValidatorsFromAssembly(typeof(ContactsModule).Assembly);

        return services;
    }
}
```

The `Host` project calls:
```csharp
builder.Services
    .AddIdentityModule(builder.Configuration)
    .AddContactsModule(builder.Configuration)
    .AddCampaignsModule(builder.Configuration)
    .AddSmtpModule(builder.Configuration);
```

---

### PHASE 3 — Migrate the Identity Module

**Goal:** Move all of `EmailMarketing.Membership` into `EmailMarketing.Identity`.

#### Step 3.1 — Move domain entities

Move to `Identity/Domain/`:
- `ApplicationUser.cs` → rename base to extend `AuditableEntity<Guid>`
- `ApplicationRole.cs`
- `ApplicationUserRole.cs`
- `ApplicationUserClaim.cs`
- `ApplicationUserLogin.cs`
- `ApplicationRoleClaim.cs`
- `ApplicationUserToken.cs`
- All enums (`EnumApplicationUserStatus`, `EnumApplicationRoleStatus`)

#### Step 3.2 — Move persistence

Move to `Identity/Infrastructure/Persistence/`:
- `ApplicationDbContext.cs` — now called `IdentityDbContext` (internal to module)
- Create EF entity type configurations for `ApplicationUser` and `ApplicationRole`
- Move `Migrations/Membership/` here

#### Step 3.3 — Create vertical slices

Each feature gets its own folder. Example structure:

```
Identity/Features/
├── Register/
│   ├── RegisterUserCommand.cs
│   ├── RegisterUserHandler.cs     ← uses UserManager
│   └── RegisterUserValidator.cs
├── Login/
│   ├── LoginCommand.cs
│   ├── LoginHandler.cs
│   └── LoginResponse.cs
├── ChangePassword/
│   ├── ChangePasswordCommand.cs
│   └── ChangePasswordHandler.cs
├── ResetPassword/
│   ├── RequestResetCommand.cs
│   └── RequestResetHandler.cs
├── GetUser/
│   ├── GetUserByIdQuery.cs
│   └── GetUserByIdHandler.cs
├── ListUsers/
│   ├── ListUsersQuery.cs
│   └── ListUsersHandler.cs
├── CreateAdminUser/
│   ├── CreateAdminUserCommand.cs
│   └── CreateAdminUserHandler.cs
└── ...
```

#### Step 3.4 — Move services and managers

Move to `Identity/Infrastructure/`:
- `ApplicationUserManager.cs`
- `ApplicationRoleManager.cs`
- `ApplicationSignInManager.cs`
- `ApplicationUserService.cs` — **split into individual feature handlers**
- `ApplicationClaimsPrincipalFactory.cs`

`IApplicationUserService` is replaced by MediatR commands/queries. Delete the interface.

#### Step 3.5 — Update contracts

`Identity/Contracts/ICurrentUser.cs` — already in SharedKernel, no duplicate needed.

`Identity/Contracts/IUserReader.cs` — if other modules need user info:
```csharp
public interface IUserReader
{
    Task<UserDto> GetByIdAsync(Guid id, CancellationToken ct = default);
}
```

#### Step 3.6 — Migrate DataSeeder

Move `DataSeeder.cs` into `Identity/Infrastructure/Seeding/IdentityDataSeeder.cs`.
Register it as `IHostedService` that runs once on startup.

#### Step 3.7 — Verify

`dotnet build` — Membership project still exists alongside; it can be deleted after Host is updated in Phase 6.

---

### PHASE 4 — Migrate the SMTP Module

**Goal:** Move SMTP domain from Framework into its own module. Smallest domain — good warm-up.

#### Step 4.1 — Move domain

Move to `Smtp/Domain/`:
- `SMTPConfig.cs` → extend `AuditableEntity<Guid>`

#### Step 4.2 — Move persistence

`Smtp/Infrastructure/Persistence/SmtpDbContext.cs`:
```csharp
public class SmtpDbContext : DbContext
{
    public DbSet<SmtpConfig> SmtpConfigs => Set<SmtpConfig>();
}
```

Move Framework migration for `SMTPConfigs` table, or plan a consolidated migration (see Phase 7 — Database Strategy).

#### Step 4.3 — Create vertical slices

```
Smtp/Features/
├── AddSmtpConfig/
│   ├── AddSmtpConfigCommand.cs
│   └── AddSmtpConfigHandler.cs
├── UpdateSmtpConfig/
│   ├── UpdateSmtpConfigCommand.cs
│   └── UpdateSmtpConfigHandler.cs
├── DeleteSmtpConfig/
│   ├── DeleteSmtpConfigCommand.cs
│   └── DeleteSmtpConfigHandler.cs
├── ActivateSmtpConfig/
│   ├── ActivateSmtpConfigCommand.cs
│   └── ActivateSmtpConfigHandler.cs
├── GetSmtpConfig/
│   ├── GetSmtpConfigByIdQuery.cs
│   └── GetSmtpConfigByIdHandler.cs
├── ListSmtpConfigs/
│   ├── ListSmtpConfigsQuery.cs
│   └── ListSmtpConfigsHandler.cs
└── TestSmtpConnection/
    ├── TestSmtpConnectionCommand.cs
    └── TestSmtpConnectionHandler.cs
```

`TestSmtpConnectionHandler` replaces `SmtpTestService`.

#### Step 4.4 — Expose contract

`Smtp/Contracts/ISmtpConfigProvider.cs`:
```csharp
public interface ISmtpConfigProvider
{
    Task<SmtpConfigDto> GetActiveByUserIdAsync(Guid userId, Guid smtpConfigId, CancellationToken ct);
}
```

This is used by the Campaigns module when sending emails — campaigns should not directly query SMTP tables.

---

### PHASE 5 — Migrate Contacts Module

**Goal:** Move Contacts, Groups, FieldMaps, Downloads from Framework into the Contacts module.

#### Step 5.1 — Move domain entities

Move to `Contacts/Domain/`:
- `Contact.cs`
- `ContactGroup.cs`
- `ContactUpload.cs`
- `ContactUploadFieldMap.cs`
- `ContactUploadGroup.cs`
- `ContactValueMap.cs`
- `FieldMap.cs`
- `DownloadQueue.cs`
- `DownloadQueueSubEntity.cs`
- `Group.cs`

**Note:** `Group` conceptually belongs to Contacts — it is a segmentation tool for contacts, consumed by Campaigns (which groups get a campaign). Groups are owned by Contacts; Campaigns only need group IDs.

#### Step 5.2 — Move persistence

`Contacts/Infrastructure/Persistence/ContactsDbContext.cs` owns all Contacts, Groups, FieldMaps, Downloads tables.

Create EF entity type configurations in `Contacts/Infrastructure/Persistence/Configurations/`.

#### Step 5.3 — Create vertical slices

```
Contacts/Features/
├── Contacts/
│   ├── CreateContact/
│   ├── UpdateContact/
│   ├── DeleteContact/
│   ├── GetContact/
│   └── ListContacts/
├── FieldMaps/
│   ├── CreateFieldMap/
│   ├── UpdateFieldMap/
│   ├── DeleteFieldMap/
│   ├── ActivateFieldMap/
│   └── ListFieldMaps/
├── Groups/
│   ├── CreateGroup/
│   ├── UpdateGroup/
│   ├── DeleteGroup/
│   ├── ActivateGroup/
│   ├── GetGroup/
│   └── ListGroups/
├── Imports/
│   ├── InitiateContactImport/
│   ├── ProcessContactImport/    ← command handled by background worker
│   └── GetImportStatus/
└── Exports/
    ├── RequestContactExport/
    ├── ProcessContactExport/    ← command handled by background worker
    └── GetExportStatus/
```

#### Step 5.4 — Migrate worker services

`ExcelWorkerService` → background handler in `Contacts/Infrastructure/Workers/ContactImportWorker.cs`:
```csharp
public class ContactImportWorker : BackgroundService
{
    // polls DownloadQueue where DownloadQueueFor == ContactImport && IsProcessing == true
}
```

`ExcelExportWorkerService` → `Contacts/Infrastructure/Workers/ContactExportWorker.cs`

Workers are registered in `ContactsModule.AddContactsModule()`:
```csharp
services.AddHostedService<ContactImportWorker>();
services.AddHostedService<ContactExportWorker>();
```

The four separate worker service projects are deleted. All background work runs in the single host.

#### Step 5.5 — Expose contracts

`Contacts/Contracts/IContactExistsService.cs`:
```csharp
public interface IContactExistsService
{
    Task<bool> ExistsAsync(string email, Guid userId, CancellationToken ct);
}
```

`Contacts/Contracts/IGroupReader.cs`:
```csharp
public interface IGroupReader
{
    Task<IReadOnlyList<GroupDto>> GetByIdsAsync(IEnumerable<int> groupIds, CancellationToken ct);
    Task<IReadOnlyList<ContactEmailDto>> GetContactEmailsByGroupIdAsync(int groupId, Guid userId, CancellationToken ct);
}
```

`Campaigns` calls `IGroupReader` to fetch contact emails when sending a campaign.

---

### PHASE 6 — Migrate Campaigns Module

**Goal:** Move Campaigns, CampaignReport, EmailTemplate from Framework into the Campaigns module.

#### Step 6.1 — Move domain entities

Move to `Campaigns/Domain/`:
- `Campaign.cs`
- `CampaignGroup.cs` (join of Campaign + Group — only stores GroupId as a value, not navigation to Group entity)
- `CampaignReport.cs`
- `EmailTemplate.cs`

#### Step 6.2 — Move persistence

`Campaigns/Infrastructure/Persistence/CampaignsDbContext.cs` owns Campaigns, CampaignGroups, CampaignReports, EmailTemplates tables.

**Important:** `CampaignGroup` links to `Group` by ID only — no EF navigation to `Group`. The FK constraint to the Groups table is kept at the DB level but not modelled as an EF navigation across module boundaries.

#### Step 6.3 — Create vertical slices

```
Campaigns/Features/
├── Campaigns/
│   ├── CreateCampaign/
│   ├── UpdateCampaign/
│   ├── DeleteCampaign/
│   ├── ActivateCampaign/
│   ├── GetCampaign/
│   └── ListCampaigns/
├── EmailTemplates/
│   ├── CreateEmailTemplate/
│   ├── UpdateEmailTemplate/
│   ├── DeleteEmailTemplate/
│   ├── GetEmailTemplate/
│   └── ListEmailTemplates/
├── CampaignReports/
│   ├── GetCampaignReport/
│   ├── ListCampaignReports/
│   └── MarkEmailSeen/           ← tracking pixel endpoint
└── Exports/
    ├── RequestCampaignExport/
    └── ProcessCampaignExport/
```

#### Step 6.4 — Migrate worker services

`EmailSendingWorkerService` → `Campaigns/Infrastructure/Workers/EmailSendingWorker.cs`:
```csharp
public class EmailSendingWorker : BackgroundService
{
    // injects IGroupReader (from Contacts.Contracts) to get contact emails
    // injects ISmtpConfigProvider (from Smtp.Contracts) to get SMTP settings
    // polls Campaigns where IsProcessing == true
}
```

`CampaingReportExcelExportService` → `Campaigns/Infrastructure/Workers/CampaignExportWorker.cs`

Both registered in `CampaignsModule.AddCampaignsModule()`.

---

### PHASE 7 — Migrate the Host (Web Application)

**Goal:** Strip `EmailMarketing.Web` down to a thin hosting layer. Controllers dispatch MediatR commands/queries only.

#### Step 7.1 — Restructure the host project

Delete from `EmailMarketing.Web`:
- All `Areas/Admin/Models/` — logic moved to Identity module handlers
- All `Areas/Member/Models/` — logic moved to module handlers
- `WebModule.cs` — Autofac module replaced by `IServiceCollection` extensions
- `FrameworkModule.cs` — replaced by module registrations
- Service locator calls (`Startup.AutofacContainer.Resolve<T>()`) — removed everywhere

Keep in `EmailMarketing.Web`:
- `Controllers/` — thin, dispatch to MediatR only
- `Startup.cs` / `Program.cs`
- `Views/` — Razor views unchanged
- `wwwroot/` — static assets unchanged
- `appsettings.json`

#### Step 7.2 — Rewrite controllers

**Before (current pattern):**
```csharp
[HttpPost]
public async Task<IActionResult> Add(CreateAdminUsersModel model)
{
    if (ModelState.IsValid)
    {
        await model.CreateAdminAsync();  // model does the work
        ...
    }
}
```

**After (vertical slice pattern):**
```csharp
[HttpPost]
public async Task<IActionResult> Add(CreateAdminUserRequest request)
{
    var result = await _sender.Send(new CreateAdminUserCommand(
        request.FullName, request.Email, request.PhoneNumber));
    ...
}
```

Controllers depend only on `ISender` (MediatR). All business logic is in handlers.

#### Step 7.3 — Rewrite Startup.cs

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddIdentityModule(builder.Configuration)
    .AddContactsModule(builder.Configuration)
    .AddCampaignsModule(builder.Configuration)
    .AddSmtpModule(builder.Configuration);

builder.Services.AddScoped<ICurrentUser, HttpContextCurrentUser>();
builder.Services.AddSingleton<IDateTime, DateTimeService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();
// ... middleware pipeline (unchanged)
```

**Remove Autofac entirely.** The default ASP.NET Core DI container is sufficient for module-scoped registration.

#### Step 7.4 — Migrate migrations

Each module manages its own migrations. Move migration files:

| Current location | New location |
|---|---|
| `EmailMarketing.Web/Migrations/Membership/` | `EmailMarketing.Identity/Infrastructure/Persistence/Migrations/` |
| `EmailMarketing.Web/Migrations/Framework/` | Split across modules (see below) |

Splitting the Framework migration is the most complex step. Options:

**Option A (Recommended) — Drop and recreate:**
1. Drop the existing Framework migration
2. Create new migrations per module (`dotnet ef migrations add InitialCreate --project EmailMarketing.Contacts`)
3. Tables are identical; only the migration metadata changes
4. Only viable if there is no production data yet

**Option B — Keep shared context temporarily:**
During migration, keep `FrameworkContext` running alongside module contexts. Add a feature flag that gradually shifts reads/writes to the new module contexts. Once all modules are migrated, delete `FrameworkContext`.

**Choose Option A** — this project is pre-production.

#### Step 7.5 — Delete obsolete projects

After all modules compile and the Host builds:
- Delete `EmailMarketing.Framework`
- Delete `EmailMarketing.Membership` (replaced by `EmailMarketing.Identity`)
- Delete `EmailMarketing.Data` (absorbed into SharedKernel)
- Delete all 4 worker service projects (workers are hosted inside Campaigns and Contacts modules)
- Delete `EmailMarketing.Common` (absorbed into SharedKernel)

---

### PHASE 8 — Database Strategy

**Goal:** Each module owns its own tables and migrations. One shared database (single PostgreSQL instance).

#### Step 8.1 — DbContext per module

| Module | DbContext | Tables |
|---|---|---|
| Identity | `IdentityDbContext` | AspNetUsers, AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetUserLogins, AspNetUserTokens, AspNetRoleClaims |
| Contacts | `ContactsDbContext` | Contacts, ContactGroups, ContactUploads, ContactUploadFieldMaps, ContactUploadGroups, ContactValueMaps, FieldMaps, Groups, DownloadQueues, DownloadQueueSubEntities |
| Campaigns | `CampaignsDbContext` | Campaigns, CampaignGroups, CampaignReports, EmailTemplates |
| Smtp | `SmtpDbContext` | SMTPConfigs |

All four contexts connect to the **same PostgreSQL database** but own separate schemas:

```sql
-- Recommended schema separation
CREATE SCHEMA identity;
CREATE SCHEMA contacts;
CREATE SCHEMA campaigns;
CREATE SCHEMA smtp;
```

Configure in each `DbContext.OnModelCreating`:
```csharp
modelBuilder.HasDefaultSchema("contacts");
```

#### Step 8.2 — Cross-module FK enforcement

`CampaignGroup.GroupId` references `Groups.Id` (owned by Contacts module).
`CampaignReport.ContactId` references `Contacts.Id` (owned by Contacts module).
`Campaign.SMTPConfigId` references `SMTPConfigs.Id` (owned by SMTP module).

**Rule:** DB-level FK constraints are kept. EF navigation properties across modules are replaced with plain ID references. EF does not model cross-module relationships.

Example:
```csharp
// In Campaigns module — no EF navigation to Group entity
public class CampaignGroup
{
    public int CampaignId { get; private set; }
    public int GroupId { get; private set; }   // just an ID, no Group nav prop
}
```

#### Step 8.3 — Migration commands

Run from solution root, specifying the startup project (Host) and the migration project (module):

```sh
# Identity module
dotnet ef migrations add InitialCreate \
  --project src/Modules/Identity/EmailMarketing.Identity.csproj \
  --startup-project src/Host/EmailMarketing.Web.csproj \
  --context IdentityDbContext

# Contacts module
dotnet ef migrations add InitialCreate \
  --project src/Modules/Contacts/EmailMarketing.Contacts.csproj \
  --startup-project src/Host/EmailMarketing.Web.csproj \
  --context ContactsDbContext

# Campaigns module
dotnet ef migrations add InitialCreate \
  --project src/Modules/Campaigns/EmailMarketing.Campaigns.csproj \
  --startup-project src/Host/EmailMarketing.Web.csproj \
  --context CampaignsDbContext

# Smtp module
dotnet ef migrations add InitialCreate \
  --project src/Modules/Smtp/EmailMarketing.Smtp.csproj \
  --startup-project src/Host/EmailMarketing.Web.csproj \
  --context SmtpDbContext
```

Apply all migrations on startup by calling `dbContext.Database.MigrateAsync()` in each module's startup sequence.

---

### PHASE 9 — Enforce Module Boundaries

**Goal:** Make it a compile error to reach across module internals.

#### Step 9.1 — Use `internal` access modifier

Within a module, make everything `internal` except what the module intentionally exports:

```csharp
// Internal — not accessible from other modules
internal class ContactRepository { ... }
internal class ContactsDbContext : DbContext { ... }
internal class CreateContactHandler : IRequestHandler<...> { ... }

// Public — intentionally exported contract
public interface IGroupReader { ... }
public class GroupDto { ... }
```

#### Step 9.2 — Project reference rules

The host's `EmailMarketing.Web.csproj` references all four modules.
A module may only reference:
- `EmailMarketing.SharedKernel`
- The `Contracts` assembly of a peer module (if needed)

Never: `EmailMarketing.Contacts` references `EmailMarketing.Campaigns` (circular). If campaigns need a contact, it goes through `IGroupReader` or a domain event.

#### Step 9.3 — Add an architecture test project

Create `test/EmailMarketing.ArchitectureTests` using `NetArchTest.Rules`:

```csharp
[Test]
public void ContactsModule_ShouldNotReference_CampaignsModule()
{
    var result = Types.InAssembly(typeof(ContactsModule).Assembly)
        .ShouldNot().HaveDependencyOn("EmailMarketing.Campaigns")
        .GetResult();
    result.IsSuccessful.ShouldBeTrue();
}
```

---

### PHASE 10 — Feature Flag Cutover

**Goal:** Switch the running application from old to new code with zero downtime.

#### Step 10.1 — Run both architectures simultaneously

During the migration, the solution can have:
- Old `EmailMarketing.Framework` (still compiling)
- New `EmailMarketing.Contacts`, `EmailMarketing.Campaigns`, etc. (new code)

Both old services and new handlers can coexist temporarily. The Host switches between them via a configuration flag:

```json
"Features": {
    "UseModularContactsModule": false,
    "UseModularCampaignsModule": false
}
```

Once a module passes all tests, flip the flag and remove the old code.

#### Step 10.2 — Cutover order

Recommended order (least coupled first):
1. SMTP module (no dependencies on other modules)
2. Identity module (no framework dependencies)
3. Contacts module (depends on Identity via `ICurrentUser`)
4. Campaigns module (depends on Contacts via `IGroupReader` and SMTP via `ISmtpConfigProvider`)

---

## NuGet Packages to Add

| Package | Purpose | Where |
|---|---|---|
| `MediatR` | In-process CQRS dispatch | SharedKernel, all modules, Host |
| `FluentValidation.DependencyInjectionExtensions` | Request validation | All modules |
| `NetArchTest.Rules` | Architecture enforcement tests | Test project |
| `Microsoft.Extensions.Hosting` | `BackgroundService` base for workers | Contacts, Campaigns |

## NuGet Packages to Remove

| Package | Reason |
|---|---|
| `Autofac` | Replaced by ASP.NET Core DI |
| `Autofac.Extensions.DependencyInjection` | No longer needed |
| `Autofac.Extras.Moq` | Replace test mocks with plain Moq |

---

## Final Solution Layout

```
EmailMarketing.sln
├── src/
│   ├── SharedKernel/
│   │   └── EmailMarketing.SharedKernel.csproj
│   ├── Modules/
│   │   ├── Identity/
│   │   │   └── EmailMarketing.Identity.csproj
│   │   ├── Contacts/
│   │   │   └── EmailMarketing.Contacts.csproj
│   │   ├── Campaigns/
│   │   │   └── EmailMarketing.Campaigns.csproj
│   │   └── Smtp/
│   │       └── EmailMarketing.Smtp.csproj
│   └── Host/
│       └── EmailMarketing.Web.csproj   ← thin; references all modules
└── test/
    ├── EmailMarketing.Identity.Tests/
    ├── EmailMarketing.Contacts.Tests/
    ├── EmailMarketing.Campaigns.Tests/
    ├── EmailMarketing.Smtp.Tests/
    └── EmailMarketing.ArchitectureTests/
```

---

## What NOT to Change

- PostgreSQL — stays; schemas add logical separation without infrastructure cost
- EF Core — stays; one `DbContext` per module
- ASP.NET Core MVC + Razor Views — stays; controllers become thin
- The appsettings structure — stays; each module reads from the same `appsettings.json`
- The CI pipeline — stays; just add the new test projects

---

## Risks and Mitigations

| Risk | Mitigation |
|---|---|
| Cross-module queries (e.g. dashboard stats) | Add a `Reporting` read model that aggregates across modules via SQL views or projections |
| Distributed transactions (e.g. create campaign + update contact count) | Use eventual consistency — raise a domain event, handle in the other module |
| EF navigation properties across module boundaries | Replace with IDs + contract queries. Enforce in architecture test. |
| Autofac-dependent test mocks (`AutoMock`) | Rewrite tests to use plain `new` or `Moq.Mock<T>` — already partially done during the warning-fix pass |
| Migration complexity (splitting FrameworkContext) | Drop-and-recreate approach (Option A). Safe because the app is pre-production. |
