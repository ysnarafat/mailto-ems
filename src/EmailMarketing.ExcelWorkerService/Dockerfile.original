
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EmailMarketing.ExcelWorkerService/EmailMarketing.ExcelWorkerService.csproj", "EmailMarketing.ExcelWorkerService/"]
COPY ["EmailMarketing.Framework/EmailMarketing.Framework.csproj", "EmailMarketing.Framework/"]
COPY ["EmailMarketing.Membership/EmailMarketing.Membership.csproj", "EmailMarketing.Membership/"]
COPY ["EmailMarketing.Common/EmailMarketing.Common.csproj", "EmailMarketing.Common/"]
COPY ["EmailMarketing.Data/EmailMarketing.Data.csproj", "EmailMarketing.Data/"]
RUN dotnet restore "EmailMarketing.ExcelWorkerService/EmailMarketing.ExcelWorkerService.csproj"
COPY . .
WORKDIR "/src/EmailMarketing.ExcelWorkerService"
RUN dotnet build "EmailMarketing.ExcelWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailMarketing.ExcelWorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailMarketing.ExcelWorkerService.dll"]