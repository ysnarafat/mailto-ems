# Getting Started

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/) (optional, for containerized deployment)

---

## 1. Clone the repository

```sh
git clone https://github.com/ysnarafat/mailto-ems.git
cd mailto-ems
```

---

## 2. Configure the database

Copy the template and fill in your values:

```sh
cp src/EmailMarketing.Web/appsettings.template.json src/EmailMarketing.Web/appsettings.json
```

Update the connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=MailtoEMS;Username=postgres;Password=yourpassword;Port=5432;"
}
```

Do the same for any worker service you intend to run (each has its own `appsettings.template.json`).

---

## 3. Apply migrations

Run both migrations from the solution root:

```sh
# Identity / membership tables
dotnet ef database update \
  --project src/EmailMarketing.Web \
  --context ApplicationDbContext \
  --connection "Host=localhost;Database=MailtoEMS;Username=postgres;Password=yourpassword;Port=5432;"

# Business-logic tables
dotnet ef database update \
  --project src/EmailMarketing.Web \
  --context FrameworkContext \
  --connection "Host=localhost;Database=MailtoEMS;Username=postgres;Password=yourpassword;Port=5432;"
```

---

## 4. Run the web application

```sh
dotnet run --project src/EmailMarketing.Web
```

---

## 5. Demo credentials

On the **first startup** the application automatically seeds the database with roles and a SuperAdmin account. No manual step is required.

| Field    | Value             |
|----------|-------------------|
| Email    | `admin@mailto.com` |
| Password | `Admin@1234`      |
| Role     | SuperAdmin        |

> After logging in you will land on the **Admin dashboard**. From there you can create additional Admin or Member users via the Users section. New users receive a system default password and are prompted to change it on first login.

The seeder is idempotent — re-running the application does not create duplicate records.

---

## 6. Run with Docker

```sh
docker-compose up --build
```

The web UI will be available at `http://localhost:8080`. Set the connection string via the `ConnectionStrings__DefaultConnection` environment variable in `docker-compose.yml`.

---

## Roles

| Role       | Access                                              |
|------------|-----------------------------------------------------|
| SuperAdmin | Full system access; not visible in user management |
| Admin      | Manages member users; access to Admin dashboard    |
| Member     | Manages own contacts, groups, and campaigns        |
