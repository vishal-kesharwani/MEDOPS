$env:PATH = "C:\Users\DELL\.dotnet;" + $env:PATH
dotnet new classlib -n MedOps.Domain -o src/MedOps.Domain --framework net10.0
dotnet new classlib -n MedOps.Contracts -o src/MedOps.Contracts --framework net10.0
dotnet new classlib -n MedOps.Application -o src/MedOps.Application --framework net10.0
dotnet new classlib -n MedOps.Infrastructure -o src/MedOps.Infrastructure --framework net10.0
dotnet new webapi -n MedOps.Api -o src/MedOps.Api --framework net10.0 --auth None --use-controllers
dotnet new xunit -n MedOps.UnitTests -o tests/MedOps.UnitTests --framework net10.0
dotnet new xunit -n MedOps.IntegrationTests -o tests/MedOps.IntegrationTests --framework net10.0
dotnet new xunit -n MedOps.ApiTests -o tests/MedOps.ApiTests --framework net10.0

dotnet sln add src/MedOps.Domain/MedOps.Domain.csproj
dotnet sln add src/MedOps.Contracts/MedOps.Contracts.csproj
dotnet sln add src/MedOps.Application/MedOps.Application.csproj
dotnet sln add src/MedOps.Infrastructure/MedOps.Infrastructure.csproj
dotnet sln add src/MedOps.Api/MedOps.Api.csproj
dotnet sln add tests/MedOps.UnitTests/MedOps.UnitTests.csproj
dotnet sln add tests/MedOps.IntegrationTests/MedOps.IntegrationTests.csproj
dotnet sln add tests/MedOps.ApiTests/MedOps.ApiTests.csproj
