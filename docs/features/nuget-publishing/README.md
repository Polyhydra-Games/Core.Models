---
title: Public NuGet Publishing
status: active
owner: @DreadBreadcrumb
priority: high
complexity: 2
created: 2026-05-12
updated: 2026-05-18
tags: [feature, nuget, publishing, core-models]
---

# Public NuGet Publishing

## Hermes Status

Hermes Kanban: pending promotion — canonical family anchor; checklist and sibling docs point here.

## Summary

`PolyhydraGames.Core.Models` is intended to be consumed as a public nuget.org package rather than through private sibling checkouts.

The package is public on nuget.org. The enabled GitHub Actions workflow tests, packs, and publishes new versions from `main`.

## Current State

- [x] Package ID is `PolyhydraGames.Core.Models`.
- [x] Current public nuget.org package exists.
- [x] Existing public versions include `1.0.1.20`, `1.0.2.6`, `1.0.2.7`, `1.0.2.8`, and `1.0.2.10`.
- [x] Package dependencies are public on nuget.org:
  - `PolyhydraGames.Core.Interfaces`
  - `PolyhydraGames.Extensions` (v2.1.1.78)
  - `System.Reactive`
- [x] Project file includes package metadata for nuget.org.
- [x] GitHub Actions workflow publishes `2.0.1.<run_number>` packages.

## Publishing

- Trigger: pushes to `main` and manual dispatch on `main` run validation, packaging, and publishing. Pull requests retain validation and never publish. Concurrency may coalesce queued default-branch runs; the newest green commit of a burst is published.
- The `package` job checks out the repository, restores, builds, tests, computes `2.0.1.<github.run_number>`, and packs exactly one package into `publish-out/`. It hands the package to downstream jobs through `actions/cache`.
- Two runner roles are used. `package` runs on `[self-hosted, Linux, X64, core-models-ci]` and executes checked-out build and test code. `publish-nuget-org` and `publish-mirrors` run only on `[self-hosted, nuget-core-models-publish]`, a dedicated runner that never executes build, test, or pull request code. Keeping publishing credentials on a separate host prevents compromised build code from persisting on the host that receives the nuget.org OIDC token. The publish runner is provisioned by [lancer1977/homelab-gitops#1028](https://github.com/lancer1977/homelab-gitops/issues/1028) and [#1029](https://github.com/lancer1977/homelab-gitops/issues/1029). No other workflow job may use the publish label.
- Both downstream publishing jobs first clear the retained runner workspace, install the same .NET 8, 9, and 10 SDKs inside their own job, then restore the run-scoped package cache. Neither job checks out repository code or changes into a repository path.
- The `publish-nuget-org` job verifies the exact package and version, runs the Python-backed nuget.org version guard with isolated `python3 -I`, authenticates through the pinned `NuGet/login` action and GitHub OIDC, and pushes to nuget.org. It has no package publishing secret; the OIDC action supplies the push credential.
- BaGet and GitHub Packages pushes run in the independent `publish-mirrors` non-OIDC job. Each push is non-blocking, and the job is not a dependency of `publish-nuget-org`.
- One-time operator step: configure a nuget.org Trusted Publishing policy for owner `lancer1977`, repository `lancer1977/Core.Models`, workflow file `publish-nuget.yml`.

## Validation

- [ ] Confirm the workflow publishes a `2.0.1.<run>` package from a green `main` push or dispatch.
- [x] Keep OIDC permission only on the package-only `publish-nuget-org` job; pass packages through the run-scoped cache.
- [x] Switch downstream apps away from sibling `ProjectReference` usage once the new package is available.
