---
title: Public NuGet Publishing Checklist
status: active
owner: @DreadBreadcrumb
priority: high
complexity: 2
created: 2026-05-12
updated: 2026-05-12
tags: [checklist, nuget, publishing, core-models]
---

# Public NuGet Publishing Checklist

## Discovery

- [x] Verify `PolyhydraGames.Core.Models` already exists on nuget.org.
- [x] Verify package dependencies are also public on nuget.org.
- [x] Confirm the repo did not have a GitHub Actions publish workflow.

## Implementation

- [x] Add NuGet package metadata to the project file.
- [x] Add README packaging metadata.
- [x] Add a GitHub Actions workflow for restore, test, pack, and publish.
- [x] Use `1.0.2.<run_number>` for the next automated public package line.
- [x] Install .NET 8, .NET 9, and .NET 10 in CI for the package target frameworks and tests.

## Release

- [ ] Add `NUGET_API_KEY` to GitHub repository secrets if missing.
- [ ] Push to `main`.
- [ ] Confirm the package appears on nuget.org.

## Downstream

- [ ] Update `chop-it` to use `PackageReference` after the new package is published.
- [ ] Remove `Core.Models` checkout/build-context handling from downstream deployment workflows.
