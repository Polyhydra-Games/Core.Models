---
title: Model Contract Stability
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-06-03
tags: [feature, documentation, Core.Models, contracts]
---

# Model Contract Stability

## Hermes Status

Hermes Kanban: pending promotion — canonical family anchor; checklist and sibling docs point here.

## Summary

This feature defines the contract boundary for `PolyhydraGames.Core.Models`.
It describes which data shapes belong in the shared package, which shapes must stay downstream, and how public model changes should be reviewed before release.

## Current Scope

- Stable shared DTOs, enums, and pure helpers belong in `Core.Models`.
- Execution logic, provider APIs, playback orchestration, database persistence, scheduler behavior, and CDN delivery stay downstream.
- Shared model families should be introduced only after at least one real downstream consumer can map or serialize them.

## Boundary Rules

- `Core.Models` may own:
  - serialization-friendly DTOs
  - pure normalization helpers
  - cross-project enum/value contracts
  - portable playlist or asset references
- `Core.Models` should not own:
  - scanners or indexers
  - playback/FFmpeg execution
  - provider API clients
  - scheduler or cron logic
  - database persistence layers
  - CDN upload or delivery workflows

## Compatibility Checklist

- [x] Add a downstream consumer before treating a new contract family as stable.
- [x] Add serialization tests for every public contract family.
- [x] Keep implementation behavior in downstream repos.
- [x] Preserve JSON shape unless a breaking change is intentionally versioned and documented.
- [ ] Add explicit release notes for any future breaking shared-model change.

## Next Questions

- [ ] Which future family should land next: media asset references, provider references, relationships, or queue/request contracts?
- [ ] Which consumers should validate the next family first?
- [ ] Should `Core.Models` eventually expose a dedicated compatibility checklist template for all future families?
