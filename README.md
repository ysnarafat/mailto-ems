

# MailTo EMS

MailTo EMS is a robust, extensible Email Marketing Software designed for organizations to manage, automate, and analyze email campaigns at scale. It supports contact management, group segmentation, campaign reporting, Excel import/export, and more, with a modular .NET architecture and Docker support.

## Features
- Contact and group management
- Bulk email sending and campaign management
- Excel import/export for contacts and reports
- Role-based access and membership management
- RESTful API and web interface
- Worker services for background processing
- Dockerized deployment

## Project Structure
```
EmailMarketing.Web/                # ASP.NET Core MVC web application
EmailMarketing.Framework/          # Core business logic and domain entities
EmailMarketing.Data/               # Data access and repository pattern
EmailMarketing.Common/             # Shared utilities and constants
EmailMarketing.Membership/         # User and membership management
EmailMarketing.ExcelWorkerService/ # Worker for Excel import
EmailMarketing.ExcelExportWorkerService/ # Worker for Excel export
EmailMarketing.EmailSendingWorkerService/ # Worker for sending emails
EmailMarketing.CampaingReportExcelExportService/ # Worker for campaign report export
... (see solution for all projects)
```

## Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [Docker](https://www.docker.com/) (for containerized deployment)
- SQL Server (local or remote)

## Getting Started

### 1. Clone the repository
```sh
git clone https://github.com/ysnarafat/mailto-ems.git
cd mailto-ems
```

### 2. Configuration
- Update `EmailMarketing.Web/appsettings.json` with your database connection string and SMTP settings.
- Environment variables can be set for Docker deployment (see `docker-compose.yml`).

### 3. Build and Run (Development)
```sh
dotnet build EmailMarketing.sln
dotnet run --project EmailMarketing.Web/EmailMarketing.Web.csproj
```

### 4. Run with Docker
```sh
docker-compose up --build
```

## Usage
- Access the web UI at `http://localhost:8080` (default Docker port)
- Register/login and start managing contacts, groups, and campaigns
- Import/export contacts using Excel files
- Monitor logs in `EmailMarketing.Web/wwwroot/Logs/`

## Testing
Unit tests are available in the `*.Tests` projects. Run all tests:
```sh
dotnet test EmailMarketing.sln
```

## Contributing
Pull requests are welcome! For major changes, please open an issue first to discuss what you would like to change.

## License
This project is for internal/organizational use. See individual library folders for third-party licenses.
