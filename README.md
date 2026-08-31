# MedOps Admin Platform

Enterprise administrative platform for medical operations management, demonstrating modernization of a legacy application into a secure, maintainable, testable, and cloud-ready system.

## Tech Stack

| Layer | Technology |
|-------|-----------|
| Frontend | Angular 21 (standalone components, signals) |
| Backend | ASP.NET Core 10 Web API + GraphQL (HotChocolate) |
| ORM | Entity Framework Core 10 (SQL Server) |
| Auth | ASP.NET Core Identity + JWT Bearer |
| Caching | Redis (StackExchange.Redis) |
| Storage | Azure Blob Storage, Azure Table Storage |
| Testing | xUnit, Moq, FluentAssertions, InMemory EF Core |
| Container | Docker + Docker Compose |
| CI/CD | Azure Pipelines |

## Solution Structure

```
MedOps.slnx
src/
  MedOps.Domain/          - Entities, Enums, Value Objects, Exceptions, Interfaces
  MedOps.Application/     - DTOs, Validators (FluentValidation), Service Interfaces/Implementations
  MedOps.Infrastructure/  - EF Core DbContext, Repositories, Azure Services, Redis Cache
  MedOps.Contracts/       - Shared Events, Service Contracts
  MedOps.Api/             - Web API Controllers, GraphQL, Middleware, Program.cs
tests/
  MedOps.UnitTests/       - Domain, Validator, Service unit tests (34 tests)
  MedOps.IntegrationTests/- EF Core InMemory repository/service tests (10 tests)
  MedOps.ApiTests/        - WebApplicationFactory API tests (2 tests)
frontend/
  medops-web/             - Angular 21 SPA
```

## Quick Start

### Docker (recommended)
```bash
docker-compose up --build
```
API available at `http://localhost:5000`, Swagger at `http://localhost:5000/swagger`

### Local Development
```bash
# Backend
dotnet restore
dotnet build
dotnet test
dotnet run --project src/MedOps.Api

# Frontend
cd frontend/medops-web
npm install
ng serve
```
Frontend at `http://localhost:4200`, API at `http://localhost:5000`

### Environment Variables
| Variable | Description |
|----------|------------|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string |
| `Redis__ConnectionString` | Redis connection string |
| `Jwt__Key` | JWT signing key |
| `Jwt__Issuer` | JWT issuer |
| `Jwt__Audience` | JWT audience |
| `Azure:Storage:ConnectionString` | Azure Blob Storage connection |
| `Azure:Tables:ConnectionString` | Azure Table Storage connection |

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/studies` | List all studies |
| POST | `/api/studies` | Create study |
| GET | `/api/studies/{id}` | Get study by ID |
| PUT | `/api/studies/{id}` | Update study |
| DELETE | `/api/studies/{id}` | Delete study |
| GET | `/api/sites` | List all sites |
| POST | `/api/sites` | Create site |
| GET | `/api/tasks` | List all tasks |
| POST | `/api/tasks` | Create task |
| POST | `/api/tasks/{id}/start` | Start task |
| POST | `/api/tasks/{id}/complete` | Complete task |
| GET | `/api/requests` | List all requests |
| POST | `/api/requests` | Create request |
| GET | `/health` | Health check |
| GET | `/graphql` | GraphQL endpoint |
| GET | `/swagger` | Swagger UI |

## Testing

```bash
dotnet test --verbosity normal
```

- **34 unit tests**: Domain entities, FluentValidation validators, service logic with mocked repositories
- **10 integration tests**: EF Core InMemory database repository and service integration tests
- **2 API tests**: WebApplicationFactory-based health check and endpoint tests
