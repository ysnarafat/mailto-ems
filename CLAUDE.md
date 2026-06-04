# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

MailTo EMS is an Email Marketing Software platform built with .NET 10.0 and PostgreSQL. It manages contacts, campaigns, bulk email sending, and campaign performance tracking.

## Build & Test Commands

```bash
# Restore and build
dotnet restore EmailMarketing.sln
dotnet build EmailMarketing.sln --configuration Release

# Run all tests
dotnet test EmailMarketing.sln

# Run a specific test project
dotnet test test/EmailMarketing.Framework.Tests/EmailMarketing.Framework.Tests.csproj

# Run a single test class (NUnit filter syntax)
dotnet test test/EmailMarketing.Framework.Tests/ --filter "FullyQualifiedName~ContactServiceTests"

# Run the web application
dotnet run --project src/EmailMarketing.Web

# Docker (full stack with PostgreSQL)
docker-compose up --build
```

## Database Migrations

Two separate DbContexts require separate migration commands:

```bash
# ApplicationDbContext (Identity/Membership)
dotnet ef database update --project src/EmailMarketing.Web --context ApplicationDbContext
dotnet ef migrations add <Name> --project src/EmailMarketing.Web --context ApplicationDbContext --output-dir Migrations/Membership

# FrameworkContext (Business domain)
dotnet ef database update --project src/EmailMarketing.Web --context FrameworkContext
dotnet ef migrations add <Name> --project src/EmailMarketing.Web --context FrameworkContext --output-dir Migrations/Framework
```

## Configuration

Copy `src/EmailMarketing.Web/appsettings.template.json` to `appsettings.json` and configure:
- `ConnectionStrings.DefaultConnection` — PostgreSQL connection string
- `AppSettings` — file paths for import/export/email files
- `SmtpSettings` — transactional email server

Each worker service also has its own `appsettings.json` that must be configured with the same connection string.

## Architecture

### Project Structure

| Project | Role |
|---|---|
| `EmailMarketing.Web` | ASP.NET Core MVC app; Areas: Admin and Member |
| `EmailMarketing.Framework` | Business logic, entities, repositories, services |
| `EmailMarketing.Data` | Generic repository/UoW interfaces and base implementation |
| `EmailMarketing.Common` | Shared exceptions, email utilities, file storage, datetime helpers |
| `EmailMarketing.Membership` | ASP.NET Identity, user/role management, data seeding |
| `EmailMarketing.ExcelWorkerService` | Background: imports contacts from Excel |
| `EmailMarketing.ExcelExportWorkerService` | Background: exports contacts to Excel |
| `EmailMarketing.EmailSendingWorkerService` | Background: sends queued campaign emails |
| `EmailMarketing.CampaingReportExcelExportService` | Background: exports campaign reports |

### Two DbContexts

- **`ApplicationDbContext`** (Membership project) — Identity tables (users, roles, claims). Migrations in `src/EmailMarketing.Web/Migrations/Membership/`.
- **`FrameworkContext`** (Framework project) — all business domain tables (campaigns, contacts, groups, SMTP, templates). Migrations in `src/EmailMarketing.Web/Migrations/Framework/`.

Both contexts are registered in `Startup.cs` and injected via Autofac.

### Dependency Injection

Autofac is used instead of the built-in .NET DI container. Registrations are split across:
- `FrameworkModule` (`src/EmailMarketing.Framework/FrameworkModule.cs`) — registers repositories, UoWs, services
- `WebModule` (`src/EmailMarketing.Web/`) — registers web-layer services and models

### Repository & Unit of Work Pattern

`EmailMarketing.Data` defines the generic interfaces. The pattern has two levels:
1. `IRepository<TEntity, TKey, TContext>` — CRUD, async LINQ queries, pagination, includes
2. Domain-specific UoWs (e.g., `IContactUnitOfWork`, `ICampaignUnitOfWork`) — group related repositories and expose `SaveAsync()` / `BeginTransaction()`

All entities inherit from `IEntity<TKey>` which includes `IsDeleted` (soft delete) and `IsActive` flags.

### Key Domain Relationships

- **Campaign** → uses one EmailTemplate, one SMTPConfig, targets many Groups
- **Campaign** → produces many CampaignReports (per-contact send tracking)
- **Contact** → belongs to many Groups (via ContactGroup join table)
- **Contact** → has many ContactValueMap entries (custom field data defined by FieldMap)
- **ContactUpload** → spawns Contact records (Excel import source tracking)

### Role-Based Access

Three roles seeded on startup: `SuperAdmin`, `Admin`, `Member`. Default seeded credentials: `admin@mailto.com` / `Admin@1234` (see `EmailMarketing.Membership/Seeds/DataSeeder.cs`).

### Background Workers

Worker services poll the database for queued jobs. They share the same `FrameworkContext` and are enabled/disabled by commenting them in/out of `docker-compose.yml`.

## Testing

Tests use **NUnit** with **Autofac.Extras.Moq** and **Shouldly** assertions. The standard setup:

```csharp
[OneTimeSetUp] public void ClassSetup() => _mock = AutoMock.GetLoose();
[SetUp] public void Setup() => _service = _mock.Create<ConcreteService>();
```

Mock repositories via `_mock.Mock<IRepository>().Setup(...)` then assert with Shouldly (`result.ShouldNotBeNull()`).
