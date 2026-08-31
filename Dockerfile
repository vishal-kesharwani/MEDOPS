FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/MedOps.Api/MedOps.Api.csproj", "src/MedOps.Api/"]
COPY ["src/MedOps.Application/MedOps.Application.csproj", "src/MedOps.Application/"]
COPY ["src/MedOps.Domain/MedOps.Domain.csproj", "src/MedOps.Domain/"]
COPY ["src/MedOps.Infrastructure/MedOps.Infrastructure.csproj", "src/MedOps.Infrastructure/"]
COPY ["src/MedOps.Contracts/MedOps.Contracts.csproj", "src/MedOps.Contracts/"]
RUN dotnet restore "src/MedOps.Api/MedOps.Api.csproj"
COPY src/ .
RUN dotnet build "src/MedOps.Api/MedOps.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/MedOps.Api/MedOps.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedOps.Api.dll"]