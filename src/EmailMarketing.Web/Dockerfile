
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EmailMarketing.Web/EmailMarketing.Web.csproj", "EmailMarketing.Web/"]
COPY ["EmailMarketing.Membership/EmailMarketing.Membership.csproj", "EmailMarketing.Membership/"]
COPY ["EmailMarketing.Common/EmailMarketing.Common.csproj", "EmailMarketing.Common/"]
COPY ["EmailMarketing.Data/EmailMarketing.Data.csproj", "EmailMarketing.Data/"]
COPY ["EmailMarketing.Framework/EmailMarketing.Framework.csproj", "EmailMarketing.Framework/"]
RUN dotnet restore "EmailMarketing.Web/EmailMarketing.Web.csproj"
COPY . .
WORKDIR "/src/EmailMarketing.Web"
RUN dotnet build "EmailMarketing.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmailMarketing.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailMarketing.Web.dll"]