# Running Test Integration

Quick reference for executing Integration Tests for `KonaAI.Master.Test.Integration` locally and in CI, including database setup, environment variables, and coverage collection.

## 1) Set Connection String (PowerShell)
Use your connection string for the test session:

```powershell
$env:Test__ConnectionStrings__DefaultConnection = "Data Source=dc-l-;Initial Catalog=KonaAI;Integrated Security=True;TrustServerCertificate=True"
```

Alternative local options:

```powershell
# LocalDB (Windows)
$env:Test__ConnectionStrings__DefaultConnection = "Server=(localdb)\MSSQLLocalDB;Database=KonaAI_Master_Test;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

# Docker SQL Server
$env:Test__ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=KonaAI_Master_Test;User Id=sa;Password=YourStrong!Passw0rd;Encrypt=False;TrustServerCertificate=True"
```

Notes:
- Ensure your account/credentials have access to the SQL Server instance and database.
- `Encrypt=False;TrustServerCertificate=True` is acceptable for local/dev only.

## 2) Apply Migrations (optional if fixture auto-migrates)
Run once to ensure schema is current when fixtures don’t apply migrations automatically:

```powershell
cd "C:\Users\UdayChaitanyaGurvind\Desktop\KonaAi\API-Covasant\KonaAI-API\KonaAI.Master"
dotnet ef database update --project .\KonaAI.Master.Repository --startup-project .\KonaAI.Master.API
```

## 3) Run Integration Tests with Coverage

```powershell
cd "C:\Users\UdayChaitanyaGurvind\Desktop\KonaAi\API-Covasant\KonaAI-API\KonaAI.Master"
dotnet test .\KonaAI.Master.Test.Integration\KonaAI.Master.Test.Integration.csproj --collect:"XPlat Code Coverage" --logger "trx;LogFileName=TestResults.trx"
```

Outputs:
- Test results summary in console and `TestResults.trx` (TRX format)
- Coverage file at `TestResults/**/coverage.cobertura.xml`

## 4) Start SQL Server in Docker (optional)

```powershell
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" -p 1433:1433 --name sql2019 -d mcr.microsoft.com/mssql/server:2019-latest
```

## 5) Troubleshooting
- Connection or login errors: verify `Test__ConnectionStrings__DefaultConnection` and server reachability.
- Pending model changes: run the migration command above.
- TLS/Encrypt errors locally: include `Encrypt=False;TrustServerCertificate=True`.
- Permission issues with Integrated Security: ensure your Windows user has access to the DB.

## 6) CI Hints
- Export `Test__ConnectionStrings__DefaultConnection` in the CI job environment.
- Ensure SQL Server is available (service/container) before running tests.
- Collect coverage with `--collect:"XPlat Code Coverage"` and publish the Cobertura/coverage artifact.

## References
- Project: `KonaAI.Master.Test.Integration/KonaAI.Master.Test.Integration.csproj`
- Rulebook: `TestIntegration-RuleBook.mdc`
- Repository migrations: `KonaAI.Master.Repository/Migrations/*`
