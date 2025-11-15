# Database

## Provider

MailTo EMS uses **PostgreSQL** (via `Npgsql.EntityFrameworkCore.PostgreSQL`).

---

## Contexts

| Context                | Project                       | Purpose                                      |
|------------------------|-------------------------------|----------------------------------------------|
| `ApplicationDbContext` | `EmailMarketing.Membership`   | ASP.NET Identity tables (users, roles, etc.) |
| `FrameworkContext`     | `EmailMarketing.Framework`    | Business-logic tables (campaigns, contacts, etc.) |

Both contexts share the same database and connection string. Migrations for both live in `src/EmailMarketing.Web/Migrations/`.

---

## Connection string format

```
Host=<host>;Database=<dbname>;Username=<user>;Password=<password>;Port=5432;
```

Set this as `ConnectionStrings:DefaultConnection` in `appsettings.json` for the web project and in each worker service's `appsettings.json`.

---

## Applying migrations

From the solution root:

```sh
# Membership context
dotnet ef database update \
  --project src/EmailMarketing.Web \
  --context ApplicationDbContext

# Framework context
dotnet ef database update \
  --project src/EmailMarketing.Web \
  --context FrameworkContext
```

---

## Adding a new migration

```sh
# Membership context
dotnet ef migrations add <MigrationName> \
  --project src/EmailMarketing.Web \
  --context ApplicationDbContext \
  --output-dir Migrations/Membership

# Framework context
dotnet ef migrations add <MigrationName> \
  --project src/EmailMarketing.Web \
  --context FrameworkContext \
  --output-dir Migrations/Framework
```

---

## Data seeding

Roles and the default SuperAdmin account are seeded automatically on first startup by `DataSeeder` (`EmailMarketing.Membership/Seeds/DataSeeder.cs`).

The seeder:
- Creates the **SuperAdmin**, **Admin**, and **Member** roles if they do not exist.
- Creates the SuperAdmin user (`admin@mailto.com`) if it does not exist.
- Is **idempotent** — safe to run on every startup.

See [getting-started.md](getting-started.md) for the default credentials.
