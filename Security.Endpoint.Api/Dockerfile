FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
RUN mkdir -p /app/logs && chmod 777 /app/logs
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Security.Endpoint.Api/Security.Endpoint.Api.csproj", "Security.Endpoint.Api/"]
COPY ["Security.Application/Security.Application.csproj", "Security.Application/"]
COPY ["Security.Domain/Security.Domain.csproj", "Security.Domain/"]
COPY ["Security.Infrastructure/Security.Infrastructure.csproj", "Security.Infrastructure/"]
RUN dotnet restore "Security.Endpoint.Api/Security.Endpoint.Api.csproj"
COPY . .
WORKDIR "/src/Security.Endpoint.Api"
RUN dotnet build "Security.Endpoint.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Security.Endpoint.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Security.Endpoint.Api/HealthCheck ./HealthCheck
RUN mkdir -p /app/logs && chmod 777 /app/logs
ENTRYPOINT ["dotnet", "Security.Endpoint.Api.dll"]
