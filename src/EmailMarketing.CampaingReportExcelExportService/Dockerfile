
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EmailMarketing.CampaingReportExcelExportService/EmailMarketing.CampaingReportExcelExportService.csproj", "EmailMarketing.CampaingReportExcelExportService/"]
COPY ["EmailMarketing.Framework/EmailMarketing.Framework.csproj", "EmailMarketing.Framework/"]
# COPY ["EmailMarketing.Membership/EmailMarketing.Membership.csproj", "EmailMarketing.Membership/"]
COPY ["EmailMarketing.Common/EmailMarketing.Common.csproj", "EmailMarketing.Common/"]
COPY ["EmailMarketing.Data/EmailMarketing.Data.csproj", "EmailMarketing.Data/"]
RUN dotnet restore "EmailMarketing.CampaingReportExcelExportService/EmailMarketing.CampaingReportExcelExportService.csproj"
COPY . .
WORKDIR "/src/EmailMarketing.CampaingReportExcelExportService"
RUN dotnet build "EmailMarketing.CampaingReportExcelExportService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailMarketing.CampaingReportExcelExportService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailMarketing.CampaingReportExcelExportService.dll"]