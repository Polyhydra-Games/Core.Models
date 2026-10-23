# Core.Models health disposition, 2026-10 (epic Polyhydra-Games/Core.Models#5)

Facts only. Every claim cites a command or run URL. Anything not checked is marked **unverified**.
No publish, push to a shared branch, PR merge, issue edit, or deployment was performed.

## Refs and base

| Item | Value | Command |
| --- | --- | --- |
| Remote for this repo | `origin` = `git@github.com:Polyhydra-Games/Core.Models.git` | `git remote -v` |
| `origin/dev` | **Does not exist** on Polyhydra-Games/Core.Models | `git rev-parse --verify origin/dev` (fatal), `git ls-remote --heads origin dev` (empty) |
| `origin/main` | `3ef4bcba9ab5904158d950c984eb40308da4b54b` | `git rev-parse origin/main` |
| GitHub default branch | `import/lancer1977-main` (not `main`) | `gh repo view Polyhydra-Games/Core.Models --json defaultBranchRef` |
| `origin/import/lancer1977-main` | `a39b7fe`, one commit behind `origin/main` | `git rev-list --left-right --count origin/main...origin/import/lancer1977-main` returns `1 0` |

Base used for this worktree: `origin/main` (3ef4bcb). The brief named `origin/dev`, which does not exist. Using `main` as the base is a workaround for a missing integration branch, and is flagged as an open decision in section 6.

## (a) Default-head build and focused test

Host: .NET SDKs 8.0.131, 9.0.121, 10.0.112, 6.0.136 installed (`dotnet --list-sdks`).

| Step | Command | Result |
| --- | --- | --- |
| Restore | `dotnet restore Tests/PolyhydraGames.Core.Models.Test.csproj` | exit 0 |
| Build test project | `dotnet build Tests/PolyhydraGames.Core.Models.Test.csproj -c Release --no-restore` | exit 0, 0 Warning(s), 0 Error(s) |
| Build solution (all library TFMs) | `dotnet build PolyhydraGames.Core.Models.sln -c Release --no-restore` | exit 0, 0 Warning(s), 0 Error(s); library built for net8.0 and net10.0 |
| Test | `dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj -c Release --no-build --results-directory <scratch>/cov --collect:"XPlat Code Coverage"` | exit 0, `Passed! - Failed: 0, Passed: 194, Skipped: 0, Total: 194, Duration: 1 m 1 s` (net10.0) |
| Coverage gate (as in ci.yml) | `python3 scripts/summarize-coverage.py <scratch>/cov --min-line 77` | exit 0, line 77.33% (839/1085), branch 67.66% (226/334) |

Required services and environment: none observed. The test project targets net10.0 only and needs the .NET SDK. `Tests/TimeStampAndObservableFileMonitorTests.cs` uses the local file system and timers; no database, container, or network endpoint was found by grep for `Environment`, `GetEnvironmentVariable`, `Testcontainers`, `localhost:` (only that one file matched). Whether those tests need a writable temp directory was not checked (unverified).

Target frameworks on origin/main (`grep TargetFramework`): library `Models/PolyhydraGames.Core.Models.csproj` = `net10.0;net8.0`; test project = `net10.0`.

## (b) CI status on the current head

Default branch for CI: `ci.yml` triggers on `push` to `main`, `pull_request` to `main`, and `workflow_dispatch`. There is no `dev` trigger.

Latest runs on `main` (`gh run list -R Polyhydra-Games/Core.Models --branch main`):

| Run | Head | Conclusion | Event | Date |
| --- | --- | --- | --- | --- |
| [39465146362](https://github.com/Polyhydra-Games/Core.Models/actions/runs/39465146362) (run_number 10) | `3ef4bcb` = origin/main head | success (Build/test/coverage, Pack, Publish to nuget.org all success) | push | 2026-10-20T17:20:01Z |
| [38832125389](https://github.com/Polyhydra-Games/Core.Models/actions/runs/38832125389) | `a39b7fe` | success | push | 2026-10-16 |

Triage:
- Current head `3ef4bcb` is green (run 39465146362). Its publish job ran on push to `main`, so this head is already on nuget.org (see (c)).
- Run [38651806751](https://github.com/Polyhydra-Games/Core.Models/actions/runs/38651806751) (feat/interfaces-3, 2026-10-14) **failed** with: `##[error]PolyhydraGames.Core.Models line coverage 77.11% is below the 77.11% floor`. The 189 tests passed in that run. This is a floor-equality threshold failure, not a test failure. It was fixed on the `import` branch by `cd82338` ("ci: coverage floor 77 (measured 832/1079 = 77.108% fails a 77.11 floor)"), which is in `main` history. Its rerun, [38652070531](https://github.com/Polyhydra-Games/Core.Models/actions/runs/38652070531) at `cd82338`, was green.
- Mutation testing: scheduled run [39277391675](https://github.com/Polyhydra-Games/Core.Models/actions/runs/39277391675) on `import/lancer1977-main` was **cancelled** after 24h1s (self-hosted runner `core-models-ci`). No successful mutation run on `main` appears in `gh run list --workflow "Mutation testing"`. Runner health: **unverified**.
- Coverage margin: measured 77.33% vs floor 77. Headroom is 0.33 points. The earlier failure shows the floor is sensitive to small changes.

## (c) Deployability

Core.Models is a NuGet library, not a service. It has no health or smoke endpoint.

- Authoritative publish path: `.github/workflows/ci.yml`, job `publish` (`Publish to nuget.org`). It runs only on `push` to `main` and `needs: pack`. Steps: download artifacts, `NuGet/login` (trusted publishing, environment `nuget`), `dotnet nuget push ... --source https://api.nuget.org/v3/index.json --skip-duplicate`.
- Consequence: every push to `main` publishes a new package version. Run 39465146362 pushed `PolyhydraGames.Core.Models.3.0.0.10.nupkg` (confirmed in run log: "Your package was pushed.").
- nuget.org flat-container index (`curl https://api.nuget.org/v3-flatcontainer/polyhydragames.core.models/index.json`): 68 versions, highest `3.0.0.10`.
- `deploy/` on origin/main is a dev-forge template (`deploy/README.md`: "This folder captures reusable default-deploy baselines for new project repos"). `deploy/portainer-stack.yml` uses placeholder image `ghcr.io/lancer1977/example-app:main`. It is not a deployment of this library. No Portainer stack for Core.Models was checked (unverified, and out of scope for a library).
- No mirror publish job exists in `ci.yml` on origin/main.

## (d) Open PRs

`gh pr list -R Polyhydra-Games/Core.Models --state open` returned **no open PRs** at 2026-10-23.

Recent merged PRs (`gh pr list --state all --limit 10`): #9 (fix8-race-hardening, merged 2026-10-19), #7 (feat/interfaces-3, merged 2026-10-14), #3 and #2 (2026-07-12). No PR dispositions required. Nothing was merged or closed by this run.

The lancer1977 fork (`lancer1977/Core.Models`) has no open PRs (`gh pr list -R lancer1977/Core.Models --state open` empty). The previous `docs/health-2026-10.md` on main lists PRs #56 and #48, which are not present on the Polyhydra-Games remote (stale, see section 5).

## (e) P0/P1/P2 follow-up candidates

| Priority | Candidate | Evidence |
| --- | --- | --- |
| P1 | Decide the integration base and fix the GitHub default branch. `dev` does not exist, and the default branch is the stale `import/lancer1977-main`. | `gh repo view ... defaultBranchRef`; `git ls-remote --heads origin dev` empty |
| P1 | Every push to `main` publishes to nuget.org with no human gate beyond the tests. The publish job has no approval step. Confirm this is intended, or add a protected environment review. | `ci.yml` job `publish`; run 39465146362 log "Your package was pushed." |
| P2 | Coverage floor is 77 with measured 77.33%. A 0.33-point margin already caused a red run (38651806751). | `summarize-coverage.py --min-line 77` output; run 38651806751 error line |
| P2 | Mutation testing has no successful run on `main`. Last scheduled run was cancelled. Runner `core-models-ci` health is unverified. | `gh run list --workflow "Mutation testing"`; run 39277391675 |
| P2 | Stale `docs/health-2026-10.md` on main describes the lancer1977 fork snapshot (refs `origin/dev` 5f7ba28, `origin/main` 15e6356, PRs #56 and #48, `publish-nuget.yml`). `publish-nuget.yml` does not exist on origin/main (`git cat-file -e origin/main:.github/workflows/publish-nuget.yml` fails). The doc is misleading for this remote. | `git show origin/main:docs/health-2026-10.md`; `git ls-tree origin/main .github/workflows/` |
| P3 | Version pinned in two places: `Models/PolyhydraGames.Core.Models.csproj` `VersionPrefix` 3.0.0 and `ci.yml` `PACKAGE_VERSION: 3.0.0.${{ github.run_number }}`. | grep on csproj and ci.yml |
| P3 | `net8.0` is still a library target (`net10.0;net8.0`), and CI installs `8.0.x` SDK. Confirm whether net8.0 support is intended. | `grep TargetFramework Models/*.csproj` |

No P0 finding. Build, the 194-test run, the coverage gate, and the latest `main` CI run are green at `3ef4bcb`.

## 5. Stale-doc note

`docs/health-2026-10.md` (on origin/main) should be reconciled or removed by the owner. It is not authoritative for `Polyhydra-Games/Core.Models`.

## 6. Open decisions for the owner (not decided here)

1. Is `dev` wanted as the integration base? If yes, create it. If no, change the GitHub default branch from `import/lancer1977-main` to `main`. This is an architecture/process call and was not made here.
2. Confirm that auto-publish on every push to `main` is intended.

## 7. Commands not run

Not run, by design: publish, push to shared branches, PR creation or merge, issue edits or closes, mutation workflow dispatch, Stryker run locally.
