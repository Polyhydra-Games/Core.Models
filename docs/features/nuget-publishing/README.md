---
title: Public NuGet Publishing
status: active
owner: @DreadBreadcrumb
priority: high
complexity: 2
created: 2026-05-12
updated: 2026-05-12
tags: [feature, nuget, publishing, core-models]
---

# Public NuGet Publishing

## Summary

`PolyhydraGames.Core.Models` is intended to be consumed as a public nuget.org package rather than through private sibling checkouts.

The package is already public on nuget.org. The repository now owns repeatable package metadata and a GitHub Actions workflow that tests, packs, and publishes new versions from `main`.

## Current State

- [x] Package ID is `PolyhydraGames.Core.Models`.
- [x] Current public nuget.org package exists.
- [x] Existing public versions include `1.0.1.20`.
- [x] Package dependencies are public on nuget.org:
  - `PolyhydraGames.Core.Interfaces`
  - `PolyhydraGames.Extensions`
  - `System.Reactive`
- [x] Project file includes package metadata for nuget.org.
- [x] GitHub Actions workflow publishes `1.0.2.<run_number>` packages.

## Required GitHub Secret

- [ ] `NUGET_API_KEY` must exist in the `lancer1977/Core.Models` repository secrets.

## Validation

- [ ] Confirm the workflow publishes a `1.0.2.x` package from `main`.
- [ ] Switch downstream apps away from sibling `ProjectReference` usage once the new package is available.
