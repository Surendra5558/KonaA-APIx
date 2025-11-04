# KonaAI-API CI: Staged + Parallel Workflow Redesign

## Goals

- Faster feedback with granular jobs; strong gates preserved
- Deterministic sequencing where needed, parallelization elsewhere
- Clear artifacts and logs per concern

## Job Topology

1) bootstrap (ubuntu)

- Checkout, setup .NET, restore cache key context
- Output commit metadata to be reused
- Produces no artifacts; short timeout

2) lint-and-analyzers (ubuntu) [needs: bootstrap]

- dotnet format (whitespace)
- dotnet format analyzers (unused usings/namespaces, code style)
- Artifacts: none; fail-fast

3) dependency-hygiene (ubuntu) [needs: bootstrap]

- Restore
- dotnet list package: vulnerable (blocking)
- dotnet list package: deprecated (blocking)
- dotnet list package: outdated (non-blocking PR comment)
- Dependency Review action on PRs

4) build (windows) [needs: lint-and-analyzers, dependency-hygiene]

- Restore with cache
- Build with analyzers + warnings-as-errors
- Artifacts: build logs if needed

5) test (windows) [needs: build]

- dotnet test with TRX + XPlat coverage
- Artifacts: TRX, coverage.cobertura.xml (per project)

6) coverage-aggregate (ubuntu) [needs: test]

- ReportGenerator to merge coverage
- Upload coverage-report artifact
- Enforce Cobertura line coverage ≥ 80% (blocking)

7) security-scan (ubuntu) [needs: bootstrap]

- CodeQL init/autobuild/analyze
- Runs in parallel with build/test chain

8) notify (ubuntu) [needs: coverage-aggregate, security-scan]

- Summarize key results to $GITHUB_STEP_SUMMARY
- Optional PR comment links to artifacts

## Triggers & Controls

- on: pull_request (opened, synchronize, reopened, ready_for_review)
- on: merge_group (main) for merge queue
- schedule: weekly scan
- Top-level permissions: contents: read, actions: read, security-events: write
- Timeouts: bootstrap(5), lint/deps(10), build(20), test(25), coverage(10), security(15), notify(5)
- Concurrency: group by workflow+ref; cancel in-progress on PR

## Key Implementation Notes

- Reuse existing steps; move into new jobs with needs: chaining
- Keep caches in build and test jobs only (where they help)
- Use shell: bash and set -euo pipefail for script steps
- Outdated report remains advisory via github-script PR comment
- Matrix not required now; add later for multi-OS if needed

## Files to Update

- .github/workflows/code-review.yml: replace jobs section with the staged+parallel layout
- CodeReviewAgent/docs/CODE_REVIEW_AGENT.md: update workflow diagram and job descriptions
- CodeReviewAgent/docs/AGENTS_OVERVIEW.md: reflect staged/parallel topology

## Acceptance

- PRs run: bootstrap → (lint, deps) → build → test → coverage; security runs parallel; notify last
- Merge queue validates final commit via merge_group
- Required checks updated to: lint-and-analyzers, dependency-hygiene, build, test, coverage-aggregate, security-scan
