
FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EmailMarketing.EmailSendingWorkerService/EmailMarketing.EmailSendingWorkerService.csproj", "EmailMarketing.EmailSendingWorkerService/"]
COPY ["EmailMarketing.Common/EmailMarketing.Common.csproj", "EmailMarketing.Common/"]
COPY ["EmailMarketing.Framework/EmailMarketing.Framework.csproj", "EmailMarketing.Framework/"]
# COPY ["EmailMarketing.Membership/EmailMarketing.Membership.csproj", "EmailMarketing.Membership/"]
COPY ["EmailMarketing.Data/EmailMarketing.Data.csproj", "EmailMarketing.Data/"]
RUN dotnet restore "EmailMarketing.EmailSendingWorkerService/EmailMarketing.EmailSendingWorkerService.csproj"
COPY . .
WORKDIR "/src/EmailMarketing.EmailSendingWorkerService"
RUN dotnet build "EmailMarketing.EmailSendingWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailMarketing.EmailSendingWorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailMarketing.EmailSendingWorkerService.dll"]