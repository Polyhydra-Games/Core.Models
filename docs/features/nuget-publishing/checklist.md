---
title: Public NuGet Publishing Checklist
status: active
owner: @DreadBreadcrumb
priority: high
complexity: 2
created: 2026-05-12
updated: 2026-05-18
tags: [checklist, nuget, publishing, core-models]
---

# Public NuGet Publishing Checklist

## Canonical Anchor

Canonical doc: [README](./README.md). This checklist mirrors the canonical family status and does not carry a separate Hermes marker.

## Discovery

- [x] Verify `PolyhydraGames.Core.Models` already exists on nuget.org.
- [x] Verify package dependencies are also public on nuget.org.
- [x] Confirm the repo did not have a GitHub Actions publish workflow.

## Implementation

- [x] Add NuGet package metadata to the project file.
- [x] Add README packaging metadata.
- [x] Add a GitHub Actions workflow for restore, test, pack, and publish.
- [x] Use `2.0.1.<run_number>` for automated public package versions.
- [x] Install .NET 8, .NET 9, and .NET 10 in CI for the package target frameworks and tests.
- [x] Split repository execution and OIDC publishing: the `package` job validates and packs, then hands off `publish-out/` through `actions/cache`; only `publish-nuget-org` receives `id-token: write`.
- [x] Isolate publishing credentials on `[self-hosted, nuget-core-models-publish]`; keep build and test execution on `[self-hosted, Linux, X64, core-models-ci]`. The publish runner never executes build, test, or pull request code.
- [x] Verify package generation in pull request validation with `dotnet pack` into runner temp storage, separate from the release cache handoff.
- [x] Use nuget.org Trusted Publishing through the pinned `NuGet/login` action. No NuGet API key secret is configured.

## Release

- [x] Confirm GitHub Actions restore, test, and pack succeed.
- [ ] Configure a nuget.org Trusted Publishing policy for owner `lancer1977`, repository `lancer1977/Core.Models`, workflow file `publish-nuget.yml`.
- [ ] Push or dispatch on `main` after the workflow change is merged.
- [ ] Confirm the GitHub Actions publish step succeeds through OIDC.
- [x] Confirm the package appears in the nuget.org public flat-container index.

## Downstream

- [x] Update `chop-it` to use `PackageReference` after the new package is published.
- [ ] Remove `Core.Models` checkout/build-context handling from downstream deployment workflows.
