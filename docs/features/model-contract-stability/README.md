---
title: Model Contract Stability
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-07-06
tags: [feature, documentation, Core.Models, contracts]
---

# Model Contract Stability

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
- [x] Add explicit release notes for any future breaking shared-model change.

## Release Note Path

Use `docs/release-notes/shared-contract-change-template.md` for any public model,
identifier, enum, serialization helper, API envelope, or media contract change.
The release note should be committed with the source change when any of these
are true:

- a public member is added, removed, renamed, or changes type
- a nullable member becomes required, or a required member becomes nullable
- JSON property names, converter behavior, or default values change
- enum values are added, removed, renamed, reordered, or given new semantics
- helper behavior changes in a way a consumer could observe

The note must name the local validation command, package command, advisory
command, and at least one downstream consumer command when a media contract is
in scope.

## Version Policy

- Patch: documentation, tests, analyzer cleanup, or implementation changes that
  preserve public API and serialized JSON shape.
- Minor: additive public contracts such as new optional members, new DTOs, new
  helper APIs, or new enum values that existing consumers can safely ignore.
- Major or intentionally coordinated breaking release: removed or renamed public
  members, changed member types, required-field changes, incompatible JSON
  shape changes, enum semantic changes that alter existing behavior, or helper
  behavior changes that require consumer code changes.

When a change is not clearly patch-safe, treat it as minor until a consumer
validation proves otherwise. When a change is not clearly minor-safe, open a
consumer migration issue before merging it.

## Breaking Change Rules

A shared model change is breaking when an existing consumer must change source
code, stored data, serialized JSON, generated clients, or release configuration
to keep working.

Treat these as breaking by default:

- removing or renaming a public type, member, enum value, or helper
- changing a member type or collection shape
- making an optional member required
- changing JSON property names, defaults, enum values, or converter behavior
- moving a contract to a new namespace or package
- changing helper behavior in a way that alters existing serialized output

Breaking changes require a coordinated release plan before merge:

- a release note using the shared-contract change template
- an explicit versioning decision
- validation against every known consumer touched by the contract family
- one migration issue per affected consumer repo when consumer code or data must
  change

Consumer migration issues should be opened before merging whenever the shared
change is not additive, when a consumer package reference must be bumped in the
same rollout, or when a consumer needs data migration, fixture updates, or API
contract changes to adopt the new shape.

## Required Validation

All shared model changes need:

```bash
dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj --configuration Release
dotnet pack Models/PolyhydraGames.Core.Models.csproj --configuration Release --no-restore --output artifacts/packages
dotnet list Models/PolyhydraGames.Core.Models.csproj package --include-transitive --vulnerable
```

Media-contract changes also need the closest consumer proof, starting with:

```bash
cd /mnt/data/lancer1977/code/core.nas
dotnet test src/NasDb.Tests/NasDb.Tests.csproj --configuration Release
```

## Next Questions

- [ ] Which future family should land next after the current media, provider,
  relationship, and queue/request contracts?
- [ ] Should any consumer-specific migration examples be promoted into
  reusable release note examples?

Tracking issue: https://github.com/lancer1977/Core.Models/issues/17.
