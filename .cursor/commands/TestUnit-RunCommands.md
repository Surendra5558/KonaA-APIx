# Running Unit Tests

Quick reference for executing Unit Tests for `KonaAI.Master.Test.Unit` locally, including coverage, filtering, and TRX results.

## 1) Run all unit tests with coverage
```powershell
cd "C:\Users\UdayChaitanyaGurvind\Desktop\KonaAi\API-Covasant\KonaAI-API\KonaAI.Master"
dotnet test .\KonaAI.Master.Test.Unit\KonaAI.Master.Test.Unit.csproj --collect:"XPlat Code Coverage" --logger "trx;LogFileName=UnitTestResults.trx"
```

## 2) Run solution-wide tests (all projects)
```powershell
cd "C:\Users\UdayChaitanyaGurvind\Desktop\KonaAi\API-Covasant\KonaAI-API\KonaAI.Master"
dotnet test --collect:"XPlat Code Coverage" --logger "trx;LogFileName=AllTestResults.trx"
```

## 3) Filter by class or method
```powershell
# By class
cd "C:\Users\UdayChaitanyaGurvind\Desktop\KonaAi\API-Covasant\KonaAI-API\KonaAI.Master"
dotnet test .\KonaAI.Master.Test.Unit\KonaAI.Master.Test.Unit.csproj --filter "FullyQualifiedName~Namespace.ClassName"

# By method
cd "C:\Users\UdayChaitanyaGurvind\Desktop\KonaAi\API-Covasant\KonaAI-API\KonaAI.Master"
dotnet test .\KonaAI.Master.Test.Unit\KonaAI.Master.Test.Unit.csproj --filter "FullyQualifiedName=Namespace.ClassName.MethodName"
```

## 4) Save TRX/coverage to a predictable folder
```powershell
cd "C:\Users\UdayChaitanyaGurvind\Desktop\KonaAi\API-Covasant\KonaAI-API\KonaAI.Master"
# Create an output folder at solution root
mkdir .\TestResults -ErrorAction SilentlyContinue | Out-Null

# Unit tests to custom results directory
dotnet test .\KonaAI.Master.Test.Unit\KonaAI.Master.Test.Unit.csproj `
  -r .\TestResults `
  --logger "trx;LogFileName=UnitTestResults.trx" `
  --collect:"XPlat Code Coverage"
```

Outputs:
- TRX: `KonaAI.Master\TestResults\<run-guid>\UnitTestResults.trx`
- Coverage: `KonaAI.Master\TestResults\<run-guid>\coverage.cobertura.xml`

## 5) Troubleshooting
- Restore/build first if needed:
```powershell
dotnet restore
dotnet build -c Debug
```
- No TRX file: ensure `--logger "trx;..."` is included or specify `-r` to set a result directory.
- Discovery failures: run `dotnet test -v diag` for diagnostic output.
- Keep unit tests isolated from external dependencies (no DB/network/filesystem).
