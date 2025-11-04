# Local Code Review Agent Setup

## Quick Start

### 1. Run the Local Agent
```powershell
# Quick check (format only) - ~30 seconds
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1 -QuickOnly

# Full check (format + build + test) - ~2-3 minutes
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1

# Skip security scans - ~2-3 minutes
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1 -SkipSecurity

# Verbose output for debugging  - use this 
powershell -ExecutionPolicy Bypass -File scripts/code-review-agent.ps1 -VerboseOutput
```

### 2. Optional: Install Security Tools

#### Semgrep (SAST Scanning)
```bash
# Install Python first, then:
pip install semgrep
```

#### Gitleaks (Secret Scanning)
```bash
# Download from: https://github.com/gitleaks/gitleaks/releases
# Or via Chocolatey:
choco install gitleaks
```

## Workflow Modes

| Mode | Duration | What it checks |
|------|----------|----------------|
| **QuickOnly** | ~30s | Format verification, tool restore |
| **Default** | ~2-3min | Format + build + test + security |
| **SkipSecurity** | ~2-3min | Format + build + test (no security) |

## Integration with GitHub Workflow

The local agent mirrors the unified GitHub workflow:

```
Local Agent                    GitHub Workflow
├─ Job 1: Quick Checks        ├─ quick-checks job (~30s)
├─ Job 2: Build & Test        ├─ build-and-test job (~2-3min)
└─ Job 3: Security Scans       └─ security-scan job (~3-5min)
```

**Execution Modes**:
- **QuickOnly**: Runs only Job 1 (fast feedback)
- **Default**: Runs Jobs 1-3 (full validation)
- **SkipSecurity**: Runs Jobs 1-2 (no security scans)

## Best Practices

1. **During Development**: Use `-QuickOnly` for fast feedback
2. **Before Committing**: Run full check without `-SkipSecurity`
3. **Before Pushing**: Run full check with security scans
4. **CI/CD**: GitHub Actions will run the same checks automatically

## Troubleshooting

### Common Issues
- **PowerShell Execution Policy**: Use `-ExecutionPolicy Bypass`
- **Solution Not Found**: Run from repository root directory
- **Format Issues**: Run `dotnet format` to fix formatting
- **Build Errors**: Fix compilation errors before proceeding
- **Test Failures**: Fix failing tests before pushing

### Exit Codes
- **0**: All checks passed
- **1**: One or more checks failed (blocks push/PR)

## File Structure
```
scripts/
├── code-review-agent.ps1      # Main local agent
└── README.md                 # This file
```
