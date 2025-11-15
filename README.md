
# MailTo EMS

MailTo EMS is a robust, extensible Email Marketing Software designed for organizations to manage, automate, and analyze email campaigns at scale. It supports contact management, group segmentation, campaign reporting, Excel import/export, and more, with a modular .NET architecture and Docker support.

## Features

- Contact and group management
- Bulk email sending and campaign management
- Excel import/export for contacts and reports
- Role-based access and membership management
- Worker services for background processing
- Dockerized deployment

## Project Structure

```
EmailMarketing.Web/                          # ASP.NET Core MVC web application
EmailMarketing.Framework/                    # Core business logic and domain entities
EmailMarketing.Data/                         # Data access and repository pattern
EmailMarketing.Common/                       # Shared utilities and constants
EmailMarketing.Membership/                   # User and membership management
EmailMarketing.ExcelWorkerService/           # Worker for Excel import
EmailMarketing.ExcelExportWorkerService/     # Worker for Excel export
EmailMarketing.EmailSendingWorkerService/    # Worker for sending emails
EmailMarketing.CampaingReportExcelExportService/ # Worker for campaign report export
```

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [PostgreSQL 14+](https://www.postgresql.org/download/)
- [Docker](https://www.docker.com/) (optional)

## Quick Start

See **[docs/getting-started.md](docs/getting-started.md)** for the full setup guide including database configuration, running migrations, and demo login credentials.

### TL;DR

```sh
# 1. Clone
git clone https://github.com/ysnarafat/mailto-ems.git && cd mailto-ems

# 2. Configure (copy template and set your DB password)
cp src/EmailMarketing.Web/appsettings.template.json src/EmailMarketing.Web/appsettings.json

# 3. Apply migrations
dotnet ef database update --project src/EmailMarketing.Web --context ApplicationDbContext
dotnet ef database update --project src/EmailMarketing.Web --context FrameworkContext

# 4. Run
dotnet run --project src/EmailMarketing.Web
```

On first startup the app seeds a **SuperAdmin** account automatically:

| Email | Password |
|---|---|
| `admin@mailto.com` | `Admin@1234` |

## Documentation

| Doc | Description |
|-----|-------------|
| [docs/getting-started.md](docs/getting-started.md) | Setup, migrations, demo credentials, Docker |
| [docs/database.md](docs/database.md) | Database provider, contexts, seeding, migration commands |

## Testing

```sh
dotnet test EmailMarketing.sln
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
