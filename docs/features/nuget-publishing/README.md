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

- Trigger: every push to `main` runs restore, build, tests, pack, version guard, and publish. Pull requests retain validation and never publish. Manual dispatch is also available on `main`.
- Version: `2.0.1.<github.run_number>`. The base was raised from `2.0.0` because `2.0.0.1` would be lower than the existing `2.0.0.53`. Before authentication, the workflow reads the nuget.org flat-container index and fails unless the computed version is strictly greater than the highest published version.
- Authentication uses NuGet Trusted Publishing through the pinned `NuGet/login` action and GitHub OIDC. No NuGet API key secret is used.
- One-time operator step: configure a nuget.org Trusted Publishing policy for owner `lancer1977`, repository `lancer1977/Core.Models`, workflow file `publish-nuget.yml`.

## Validation

- [x] Confirm the workflow publishes a `2.0.1.x` package from `main`.
- [x] Switch downstream apps away from sibling `ProjectReference` usage once the new package is available.
