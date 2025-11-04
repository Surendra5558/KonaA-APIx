# Local Code Review Script - Complete Guide

## Executive Summary

The local PowerShell code review script (`scripts/code-review-agent.ps1`) is a sophisticated pre-push quality gate that mirrors the GitHub Actions workflow, providing developers with the same quality standards locally before code reaches the CI pipeline.

## Script Overview

### Purpose & Design Philosophy
- **Pre-Push Quality Gate**: Catches issues before they reach GitHub
- **CI Mirroring**: Identical behavior to GitHub Actions workflow
- **Fast Feedback**: Immediate validation during development
- **Team Consistency**: Enforces uniform quality standards across team

### Key Features
- **Parallel Test Execution**: Unit and integration tests run separately
- **Focused Coverage**: 80% threshold for Controllers & Business logic only
- **Flexible Execution**: Multiple execution modes for different scenarios
- **Security Integration**: Optional security scanning with Semgrep and Gitleaks
- **Error Handling**: Comprehensive error handling with clear guidance

## Script Execution Flow

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                           SCRIPT INITIALIZATION                                 │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Parameter Parsing (-VerboseOutput, -SkipSecurity, -QuickOnly)              │
│  • Error Action Preference (Stop on first error)                              │
│  • Solution Path Validation (KonaAI.Master/KonaAI.Master.sln)                 │
│  • Logging Functions Setup (Info, Success, Warning, Error)                    │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                            JOB 1: QUICK CHECKS                                 │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Tool Restoration (dotnet tool restore)                                     │
│  • Code Formatting Verification (dotnet format --verify-no-changes)          │
│  • Duration: ~30-60 seconds                                                   │
│  • Exit Point: If -QuickOnly specified, script exits here                     │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                         JOB 2: BUILD AND TEST                                  │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Build Solution (Release mode, warnings as errors)                           │
│  • Unit Tests (Debug mode, coverage collection)                               │
│  • Integration Tests (Debug mode, coverage collection)                         │
│  • Duration: ~3-5 minutes                                                     │
│  • Note: Tests run sequentially (PowerShell limitation)                       │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        COVERAGE ANALYSIS                                        │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Coverage File Discovery (Unit & Integration)                                 │
│  • XML Parsing (Cobertura format)                                             │
│  • Combined Coverage Calculation (Simple average)                             │
│  • Threshold Enforcement (80% for Controllers & Business logic)                │
│  • Duration: ~10-30 seconds                                                    │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        JOB 3: SECURITY SCANS (Optional)                         │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Semgrep SAST (if installed)                                                │
│  • Gitleaks Secret Scanning (if installed)                                    │
│  • Duration: ~1-3 minutes (if tools installed)                               │
│  • Conditional: Skipped if -SkipSecurity specified                            │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                            SUCCESS EXIT                                        │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • All checks passed successfully                                              │
│  • Exit code: 0                                                                │
│  • Ready for push/PR                                                           │
└─────────────────────────────────────────────────────────────────────────────────┘
```

## Execution Modes & Parameters

### 1. Full Review Mode (Default)
```powershell
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1 -VerboseOutput
```
**What it does**:
- Quick checks (formatting, tools)
- Build validation (Release mode, warnings as errors)
- Unit tests with coverage (Debug mode)
- Integration tests with coverage (Debug mode)
- Coverage analysis and threshold enforcement (80%)
- Optional security scans (if tools installed)

**Duration**: ~3-5 minutes
**Use Case**: Pre-push validation, pre-PR checks

### 2. Quick-Only Mode
```powershell
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1 -QuickOnly
```
**What it does**:
- Quick checks only (formatting, tools)
- Exits after quick checks complete

**Duration**: ~30-60 seconds
**Use Case**: Fast feedback during development, quick formatting checks

### 3. Skip Security Mode
```powershell
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1 -SkipSecurity
```
**What it does**:
- All quality gates except security scans
- Faster execution without security tools

**Duration**: ~3-4 minutes
**Use Case**: When security tools not installed, faster execution needed

### 4. Verbose Output Mode
```powershell
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1 -VerboseOutput
```
**What it does**:
- Detailed logging for all operations
- Progress information for each step
- Debugging information for troubleshooting

**Use Case**: Debugging issues, detailed progress monitoring

## Execution Modes Comparison

| Mode | Command | Duration | What It Does | Use Case |
|------|---------|----------|--------------|----------|
| **Full Review** | `script.ps1 -VerboseOutput` | ~3-5 min | All quality gates + security | Pre-push, pre-PR |
| **Quick-Only** | `script.ps1 -QuickOnly` | ~30-60 sec | Formatting + tools only | Fast feedback |
| **Skip Security** | `script.ps1 -SkipSecurity` | ~3-4 min | All gates except security | When security tools not installed |
| **Verbose** | `script.ps1 -VerboseOutput` | ~3-5 min | All gates + detailed logging | Debugging issues |

## Detailed Job Analysis

### Job 1: Quick Checks (Fast Feedback)
**Purpose**: Immediate feedback on simple issues
**Duration**: ~30-60 seconds
**Dependencies**: None

#### Tool Restoration
```powershell
dotnet tool restore
```
- **Purpose**: Ensure all .NET tools are available
- **Error Handling**: Fails if tools cannot be restored
- **Duration**: ~10-20 seconds
- **Dependencies**: .NET SDK, tool manifest

#### Code Formatting Verification
```powershell
dotnet format --verify-no-changes $solutionPath
```
- **Purpose**: Verify code formatting compliance
- **Error Handling**: Fails if formatting issues found
- **Fix Command**: `dotnet format` to auto-fix issues
- **Duration**: ~20-40 seconds
- **Dependencies**: Solution file, .NET SDK

**Common Issues & Solutions**:
- **Formatting Issues**: Run `dotnet format` to fix
- **Tool Missing**: Ensure .NET tools are properly configured
- **Solution Not Found**: Run from repository root directory

### Job 2: Build and Test (Core Validation)
**Purpose**: Comprehensive validation of code quality and functionality
**Duration**: ~3-5 minutes
**Dependencies**: Quick checks (if not in QuickOnly mode)

#### Build Phase
```powershell
dotnet build $solutionPath -c Release /warnaserror
```
- **Configuration**: Release mode (production-ready)
- **Quality Enforcement**: Warnings as errors
- **Error Handling**: Fails on any compilation issues
- **Duration**: ~1-2 minutes

**Build Features**:
- **Release Configuration**: Production-ready build
- **Warnings as Errors**: Treats all warnings as compilation errors
- **Code Style**: Enforces analyzer rules during build
- **Quality Gates**: Blocks code with compilation issues

#### Test Execution (Sequential Approach)
**Note**: Tests run sequentially in local script (vs parallel in CI) due to PowerShell limitations

**Unit Tests**:
```powershell
dotnet test "KonaAI.Master/KonaAI.Master.Test.Unit/KonaAI.Master.Test.Unit.csproj" -c Debug --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```
- **Project**: Unit test project only
- **Configuration**: Debug mode (required for coverage)
- **Coverage**: XPlat Code Coverage with focused settings
- **Duration**: ~1-2 minutes

**Integration Tests**:
```powershell
dotnet test "KonaAI.Master/KonaAI.Master.Test.Integration/KonaAI.Master.Test.Integration.csproj" -c Debug --collect:"XPlat Code Coverage" --settings coverlet.runsettings
```
- **Project**: Integration test project only
- **Configuration**: Debug mode (required for coverage)
- **Coverage**: XPlat Code Coverage with focused settings
- **Duration**: ~1-2 minutes

**Test Configuration**:
- **Debug Mode**: Required for coverage collection
- **Coverage Collection**: XPlat Code Coverage with coverlet
- **Settings File**: Uses `coverlet.runsettings` for focused coverage
- **Logging**: TRX format for test results

### Job 3: Coverage Analysis & Threshold Enforcement
**Purpose**: Analyze coverage reports and enforce 80% threshold
**Duration**: ~10-30 seconds
**Dependencies**: Both unit and integration tests completed

#### Coverage File Discovery
```powershell
$unitCoverageFiles = Get-ChildItem -Path "KonaAI.Master" -Recurse -Filter "coverage.cobertura.xml" | Where-Object { $_.FullName -like "*Test.Unit*" }
$integrationCoverageFiles = Get-ChildItem -Path "KonaAI.Master" -Recurse -Filter "coverage.cobertura.xml" | Where-Object { $_.FullName -like "*Test.Integration*" }
```

**File Discovery Logic**:
- **Recursive Search**: Searches entire `KonaAI.Master` directory
- **Pattern Matching**: Identifies coverage files by test project type
- **Error Handling**: Warns if coverage files not found
- **Validation**: Ensures both unit and integration coverage files exist

#### Coverage Parsing
```powershell
[xml]$unitXml = Get-Content $unitCoverageFiles[0].FullName
$unitCoverage = [double]$unitXml.coverage.'line-rate'
```

**XML Parsing**:
- **Cobertura Format**: Standard coverage XML format
- **Line Rate**: Extracts line coverage percentage
- **Error Handling**: Graceful handling of parsing errors
- **Validation**: Ensures valid XML structure

#### Threshold Calculation
```powershell
$combinedCoverage = ($unitCoverage + $integrationCoverage) / 2
$coveragePercent = [math]::Round($combinedCoverage * 100, 2)
```

**Calculation Method**:
- **Unit Coverage**: From unit test Cobertura XML
- **Integration Coverage**: From integration test Cobertura XML
- **Combined**: Simple average of both coverage percentages
- **Threshold**: 80% minimum for combined coverage

#### Enforcement Logic
```powershell
if ($coveragePercent -lt 80) {
    Write-Error "Coverage below threshold: $coveragePercent% < 80%"
    Write-Host "Focus on adding tests for Controllers and Business logic to reach 80% coverage." -ForegroundColor Yellow
} else {
    Write-Success "Coverage threshold met: $coveragePercent% >= 80%"
}
```

**Enforcement Features**:
- **Threshold**: 80% minimum for Controllers & Business logic
- **Focus**: Only measures critical application layers
- **Error Handling**: Fails if below threshold
- **Guidance**: Provides specific recommendations for improvement

### Job 4: Security Scans (Optional)
**Purpose**: Optional security scanning for comprehensive validation
**Duration**: ~1-3 minutes (if tools installed)
**Dependencies**: Build and test completion
**Conditional**: Skipped if `-SkipSecurity` specified

#### Semgrep SAST (Static Analysis)
```powershell
if (Get-Command semgrep -ErrorAction SilentlyContinue) {
    semgrep --config p/ci --error --exclude .git --lang csharp
}
```

**Semgrep Features**:
- **Static Analysis**: Code-level security analysis
- **Language**: C# specific analysis
- **Configuration**: CI-focused security policies
- **Installation**: `pip install semgrep`
- **Error Handling**: Graceful handling if tool not installed

#### Gitleaks (Secret Scanning)
```powershell
if (Get-Command gitleaks -ErrorAction SilentlyContinue) {
    gitleaks protect --staged
}
```

**Gitleaks Features**:
- **Secret Scanning**: Detects secrets and credentials
- **Scope**: Staged changes only
- **Installation**: Download from GitHub releases
- **Error Handling**: Graceful handling if tool not installed

## Job Dependencies Matrix

| Job | Depends On | Duration | Exit Point | Error Handling |
|-----|------------|----------|------------|----------------|
| **Quick Checks** | None | ~30-60s | If -QuickOnly | Stop on error |
| **Build** | Quick Checks | ~1-2min | On build failure | Stop on error |
| **Unit Tests** | Build | ~1-2min | On test failure | Stop on error |
| **Integration Tests** | Build | ~1-2min | On test failure | Stop on error |
| **Coverage Analysis** | Both Tests | ~10-30s | On threshold failure | Stop on error |
| **Security Scans** | Coverage | ~1-3min | On security failure | Stop on error |

## Coverage Analysis Flow

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        COVERAGE FILE DISCOVERY                                  │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Search: KonaAI.Master/**/coverage.cobertura.xml                             │
│  • Filter: *Test.Unit* and *Test.Integration*                                 │
│  • Validation: Ensure both files exist                                        │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                          XML PARSING                                            │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Unit Coverage: Parse unit test Cobertura XML                                │
│  • Integration Coverage: Parse integration test Cobertura XML                   │
│  • Line Rate: Extract coverage percentage from XML                             │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        COMBINED CALCULATION                                     │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Formula: (Unit Coverage + Integration Coverage) / 2                         │
│  • Rounding: Round to 2 decimal places                                         │
│  • Display: Show percentage with threshold info                                │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        THRESHOLD ENFORCEMENT                                    │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Threshold: 80% minimum for Controllers & Business logic                     │
│  • Success: Continue to next job                                               │
│  • Failure: Stop execution with error message                                   │
│  • Guidance: Provide specific improvement recommendations                      │
└─────────────────────────────────────────────────────────────────────────────────┘
```

## Error Handling & Logging

### 1. Error Action Preference
```powershell
$ErrorActionPreference = "Stop"
```
- **Behavior**: Script stops on first error
- **Purpose**: Fail-fast approach to catch issues early
- **Consistency**: Matches CI behavior

### 2. Logging Functions
```powershell
function Write-Info($msg) {
    if ($VerboseOutput) { Write-Host "[info] $msg" -ForegroundColor Cyan }
}

function Write-Success($msg) {
    Write-Host "[SUCCESS] $msg" -ForegroundColor Green
}

function Write-Warning($msg) {
    Write-Host "[WARNING] $msg" -ForegroundColor Yellow
}
```

**Logging Levels**:
- **Info**: Detailed progress (only with `-VerboseOutput`)
- **Success**: Successful operations (always shown)
- **Warning**: Non-critical issues (always shown)
- **Error**: Critical failures (always shown, stops execution)

### 3. Try-Catch Error Handling
```powershell
try { 
    dotnet build $solutionPath -c Release /warnaserror
    Write-Success "Build completed successfully"
} catch { 
    Write-Error "Build failed - fix compilation errors" 
}
```

**Error Handling Strategy**:
- **Graceful Degradation**: Continues execution where possible
- **Clear Messages**: Specific error messages with fix suggestions
- **Exit Codes**: Non-zero exit on failures to block push

## Error Handling Flow

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                           ERROR OCCURRENCE                                     │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Try-Catch Block: Captures specific operation errors                         │
│  • Error Type: Determines error category and handling                          │
│  • Logging: Write-Error with specific message                                 │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        ERROR CATEGORIZATION                                    │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Build Errors: Compilation issues, missing dependencies                     │
│  • Test Errors: Failing tests, test setup issues                               │
│  • Coverage Errors: Below threshold, missing files                           │
│  • Security Errors: SAST failures, secret detection                           │
└─────────────────────────────────────────────────────────────────────────────────┘
                                        │
                                        ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                        ERROR RESPONSE                                           │
├─────────────────────────────────────────────────────────────────────────────────┤
│  • Clear Message: Specific error description                                   │
│  • Fix Guidance: Suggested solutions or commands                               │
│  • Exit Code: Non-zero to block push/PR                                        │
│  • Stop Execution: Fail-fast approach                                         │
└─────────────────────────────────────────────────────────────────────────────────┘
```

## Logging Levels & Output

### Logging Functions
```powershell
Write-Info($msg)     # Cyan - Detailed progress (VerboseOutput only)
Write-Success($msg)  # Green - Successful operations (always shown)
Write-Warning($msg)  # Yellow - Non-critical issues (always shown)
Write-Error($msg)    # Red - Critical failures (always shown, stops execution)
```

### Output Examples
```
KonaAI Local Code Review Agent
=================================

Job 1: Quick Checks (Fast Feedback)
[info] Restoring dotnet tools
[SUCCESS] Dotnet tools restored
[info] Checking code formatting
[SUCCESS] Code formatting verified

Job 2: Build and Test (Core Validation)
[info] Building solution (Release, warnings as errors)
[SUCCESS] Build completed successfully
[info] Running unit tests with coverage (Debug mode)
[SUCCESS] Unit tests completed successfully
[info] Running integration tests with coverage (Debug mode)
[SUCCESS] Integration tests completed successfully

Coverage Analysis
[info] Analyzing coverage reports for Controllers & Business logic
[info] Unit test coverage: 1.46%
[info] Integration test coverage: 80.18%
Combined Coverage: 40.82% (minimum 80% for Controllers & Business logic)
[ERROR] Coverage below threshold: 40.82% < 80%
Focus on adding tests for Controllers and Business logic to reach 80% coverage.
```

## Coverage Configuration

### 1. Coverlet Settings
**File**: `coverlet.runsettings`
**Purpose**: Focus coverage measurement on critical layers

```xml
<Include>
  <ModulePath>.*KonaAI\.Master\.(API|Business).*</ModulePath>
</Include>
```

**Included Layers**:
- **Controllers**: API behavior, authentication, validation
- **Business Logic**: Core rules, use cases, business processes

**Excluded Layers**:
- **Repository**: Well-tested via integration tests
- **Models**: DTOs with minimal executable logic

### 2. Coverage Analysis Logic
```powershell
# Calculate combined coverage (weighted average)
$combinedCoverage = ($unitCoverage + $integrationCoverage) / 2
$coveragePercent = [math]::Round($combinedCoverage * 100, 2)
```

**Calculation Method**:
- **Unit Coverage**: From unit test Cobertura XML
- **Integration Coverage**: From integration test Cobertura XML
- **Combined**: Simple average of both coverage percentages
- **Threshold**: 80% minimum for combined coverage

### 3. Threshold Enforcement
```powershell
if ($coveragePercent -lt 80) {
    Write-Error "Coverage below threshold: $coveragePercent% < 80%"
    Write-Host "Focus on adding tests for Controllers and Business logic to reach 80% coverage." -ForegroundColor Yellow
} else {
    Write-Success "Coverage threshold met: $coveragePercent% >= 80%"
}
```

## Performance & Optimization

### 1. Execution Times
- **Quick-Only Mode**: ~30-60 seconds
- **Full Review**: ~3-5 minutes
- **With Security**: ~4-8 minutes (if tools installed)

### 2. Optimization Strategies
- **Sequential Execution**: Tests run one after another (PowerShell limitation)
- **Debug Mode**: Required for coverage collection
- **Focused Coverage**: Only measures critical layers
- **Optional Security**: Can be skipped for faster execution

### 3. Resource Usage
- **CPU**: Moderate during build and test phases
- **Memory**: Standard .NET test execution requirements
- **Disk**: Coverage files and test artifacts
- **Network**: Minimal (only for tool restoration)

## Performance Metrics

### Execution Times
- **Quick-Only Mode**: ~30-60 seconds
- **Full Review**: ~3-5 minutes
- **With Security**: ~4-8 minutes (if tools installed)

### Resource Usage
- **CPU**: Moderate during build and test phases
- **Memory**: Standard .NET test execution requirements
- **Disk**: Coverage files and test artifacts
- **Network**: Minimal (only for tool restoration)

### Optimization Strategies
- **Sequential Execution**: Tests run one after another (PowerShell limitation)
- **Debug Mode**: Required for coverage collection
- **Focused Coverage**: Only measures critical layers
- **Optional Security**: Can be skipped for faster execution

## Integration with GitHub Workflow

### 1. Behavior Alignment
- **Same Quality Gates**: Format, build, test, coverage
- **Same Thresholds**: 80% coverage for Controllers & Business logic
- **Same Focus**: Critical application layers only
- **Same Error Handling**: Fail-fast approach

### 2. Coverage Consistency
- **Same Settings**: Uses identical `coverlet.runsettings`
- **Same Calculation**: Combined unit and integration coverage
- **Same Threshold**: 80% minimum requirement
- **Same Focus**: Controllers & Business logic only

### 3. Development Workflow
- **Pre-Push**: Run before pushing to catch issues early
- **Pre-PR**: Run before creating pull requests
- **Local Development**: Quick feedback during coding
- **CI Preparation**: Ensure code will pass CI checks

## Integration Points

### 1. Git Hooks
```bash
# Pre-push hook example
#!/bin/sh
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1
```

### 2. IDE Integration
- **Visual Studio**: External tool configuration
- **VS Code**: Task runner integration
- **JetBrains**: External tool setup

### 3. Team Workflows
- **Pre-Push**: Automatic quality checks
- **Pre-PR**: Manual validation before pull requests
- **Local Development**: Quick feedback during coding
- **CI Preparation**: Ensure code will pass CI checks

## Exit Codes & Integration

### Exit Code Meanings
- **0**: All checks passed successfully
- **1**: One or more checks failed
- **2**: Script execution error (missing files, tools, etc.)

### Integration Benefits
- **Consistent Quality**: Same standards as CI pipeline
- **Fast Feedback**: Immediate validation during development
- **Reduced Churn**: Fewer failed CI runs
- **Team Efficiency**: Uniform development experience

## Troubleshooting & Maintenance

### 1. Common Issues

#### Coverage Files Not Found
**Issue**: `Coverage files not found. Make sure tests ran successfully.`
**Solutions**:
- Verify tests completed successfully
- Check for test failures in previous steps
- Ensure Debug mode is used for tests
- Verify `coverlet.runsettings` exists

#### Build Failures
**Issue**: `Build failed - fix compilation errors`
**Solutions**:
- Run `dotnet format` to fix formatting issues
- Resolve compilation errors
- Check for missing dependencies
- Verify solution path is correct

#### Test Failures
**Issue**: `Unit tests failed` or `Integration tests failed`
**Solutions**:
- Fix failing tests
- Check test data setup
- Verify test dependencies
- Review test isolation issues

#### Coverage Below Threshold
**Issue**: `Coverage below threshold: X% < 80%`
**Solutions**:
- Add unit tests for Business logic classes
- Add controller unit tests
- Improve integration test coverage
- Focus on Create, Update, Delete operations

### 2. Performance Optimization
- **Use Quick-Only**: For fast feedback during development
- **Skip Security**: For faster execution when security tools not needed
- **Verbose Output**: For detailed debugging information
- **Regular Cleanup**: Remove old coverage files periodically

### 3. Maintenance Tasks
- **Tool Updates**: Keep .NET tools updated
- **Security Tools**: Update Semgrep and Gitleaks regularly
- **Coverage Analysis**: Review coverage trends over time
- **Script Updates**: Keep script aligned with CI workflow changes

## Best Practices

### 1. Development Workflow
- **Pre-Commit**: Run script before committing changes
- **Pre-Push**: Run script before pushing to remote
- **Pre-PR**: Run script before creating pull requests
- **Regular Checks**: Run during development for quick feedback

### 2. Team Collaboration
- **Consistent Usage**: All team members use same script
- **Shared Configuration**: Use same `coverlet.runsettings`
- **Documentation**: Keep script usage documented
- **Training**: Ensure team knows how to use script effectively

### 3. Continuous Improvement
- **Monitor Performance**: Track execution times
- **Coverage Trends**: Monitor coverage improvements
- **Error Patterns**: Track common failure causes
- **Script Evolution**: Update script to match CI improvements

## Conclusion

The local code review script provides developers with the same quality gates as the CI pipeline, enabling fast feedback and consistent code quality standards across the team. Through its flexible execution modes, comprehensive error handling, and focused coverage measurement, it serves as an essential tool for maintaining code quality in a fast-paced development environment.
