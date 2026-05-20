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

## Summary

`PolyhydraGames.Core.Models` is intended to be consumed as a public nuget.org package rather than through private sibling checkouts.

The package is already public on nuget.org. The repository now owns repeatable package metadata and a GitHub Actions workflow that tests, packs, and publishes new versions from `main`.

## Current State

- [x] Package ID is `PolyhydraGames.Core.Models`.
- [x] Current public nuget.org package exists.
- [x] Existing public versions include `1.0.1.20`, `1.0.2.6`, `1.0.2.7`, `1.0.2.8`, and `1.0.2.10`.
- [x] Package dependencies are public on nuget.org:
  - `PolyhydraGames.Core.Interfaces`
  - `PolyhydraGames.Extensions` (v2.1.1.78)
  - `System.Reactive`
- [x] Project file includes package metadata for nuget.org.
- [x] GitHub Actions workflow publishes `1.0.2.<run_number>` packages.

## Required GitHub Secret

- [x] `NUGET_API_KEY` must exist in the `lancer1977/Core.Models` repository secrets.

The workflow has been verified through restore, test, and pack. If the secret is missing on a non-PR run, the publish job now fails fast instead of silently skipping nuget.org.

## Latest Publish

- [x] `NUGET_API_KEY` is configured in GitHub repository secrets.
- [x] Workflow dispatch published `PolyhydraGames.Core.Models` version `1.0.2.10`.
- [x] GitHub Actions log showed nuget.org returned `Created` and `Your package was pushed`.
- [x] NuGet flat-container index shows `1.0.2.10`; it also shows earlier `1.0.2.6`, `1.0.2.7`, and `1.0.2.8` packages.

## Validation

- [x] Confirm the workflow publishes a `1.0.2.x` package from `main`.
- [x] Switch downstream apps away from sibling `ProjectReference` usage once the new package is available.
