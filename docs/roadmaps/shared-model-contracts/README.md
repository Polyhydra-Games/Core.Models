---
title: Shared Model Contracts Roadmap
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-30
tags: [roadmap, Core.Models, shared-contracts, media, api-models]
---

# Shared Model Contracts Roadmap

## Purpose

Define which cross-repository data contracts should move into `PolyhydraGames.Core.Models` after the initial media model work.

This roadmap is intentionally contract-first. `Core.Models` should own stable DTOs, enums, and pure helpers. It should not absorb scanner execution, provider APIs, playback workflows, scheduling logic, CDN delivery logic, or app-specific catalog behavior.

## Current State

- [x] `Core.Models` contains general utility models and helpers.
- [x] `Core.Models` now contains base local/NAS-backed media file contracts.
- [x] `Core.Models` now contains first playlist/playout contracts.
- [x] `Core.Models` now contains optional media enrichment metadata for provider/catalog overlays.
- [x] `Core.Models` now contains shared media asset reference contracts.
- [x] `Core.Models` now contains shared external provider reference contracts.
- [x] Media model docs and adoption roadmap exist.
- [x] Strongly typed identifier contracts, converters, helpers, and tests now exist.
- [x] Shared asset references are modeled.
- [x] External provider ID contracts are modeled as a standalone family.
- [x] Media relationship contracts are modeled.
- [x] Playlist data shapes are modeled under `PolyhydraGames.Core.Models.Media.Playlists`.
- [x] Playback queue data shapes are modeled.
- [x] Generic API result, paging, filtering, and readiness contracts are modeled.

## Target Consumers

- [x] `core.nas` for scanning, indexing, and media catalog API contracts.
- [x] `SvengoolieDB` for catalog media, provider IDs, episodes, assets, and companion-boundary handoff.
- [x] `chop-it` for playlist, queue, clipping input references, and operator media selections.
- [x] CDN/media normalization tooling for static asset references and delivery URLs.
- [ ] `Media-Metadata` for provider reference handoff without moving provider logic here.
- [x] Channel Cheevos and adjacent apps only for generic result/readiness/paging contracts that are truly app-agnostic.

## Import Candidates

### Strongly Typed Identifiers

- [x] Import the reusable ID pattern from `OAuth.Core` and streaming-facing contracts.
- [x] Support GUID-backed, string-backed, int-backed, and long-backed ID types.
- [x] Include JSON converters and parse/try-parse helpers.
- [x] Keep OAuth flow, PKCE, token handling, and platform clients outside `Core.Models`.
- [x] Decide whether concrete IDs like `IdentityId`, `UserId`, `WebKey`, and `TwitchId` move here or downstream packages only consume the shared base pattern: only the shared base/converter pattern moves now; concrete IDs remain downstream until ownership, compatibility tests, and release notes are ready.

### Media Asset References

- [x] Add a shared model for references to local files, CDN assets, provider images, posters, trailers, subtitles, and companion media.
- [x] Keep binary storage, CDN upload, and URL resolution outside `Core.Models`.
- [x] Map cleanly to `LocalMediaFile` when the asset is backed by local/NAS storage.

### External Provider References

- [x] Add generic provider ID contracts such as provider key, external ID, display name, URL, and last-seen metadata.
- [x] Keep provider-specific clients, API response models, and lookup rules in `Media-Metadata`.
- [x] Support multiple IDs per domain object without forcing a single canonical provider.

### Media Relationships

- [x] Add relationship models that can connect shows, episodes, videos, posters, trailers, ROMs, box art, and generated derivatives.
- [x] Keep app-specific meaning in downstream repos.
- [x] Include relationship role/type values that are broad enough for catalog and CDN use.

### Playlist And Queue Contracts

- [x] Add data-only playlist models.
- [x] Add data-only playback queue/request models.
- [ ] Keep FFmpeg execution, playback orchestration, clipping, and operator UI in `chop-it` or app repos.

### Operation, Validation, And Readiness Contracts

- [x] Add small generic result models for operation status, validation errors, warnings, and readiness summaries.
- [x] Avoid replacing framework-native result types where the app already has a clear pattern.
- [x] Prefer simple serializable contracts over service abstractions.

### Ownership Split

- [x] `Core.Models` owns the shared contract types.
- [x] `core.nas` owns retry, caching, normalization, and intake behavior.
- [x] `SvengoolieDB` and `chop-it` consume the shared contract instead of redefining it.
- [ ] `Core.Nas.REST` or a similar transport layer can exist if the HTTP surface needs separate request/response models.

### Paging, Filtering, And Sorting Contracts

- [x] Add generic `PagedRequest`, `PagedResult<T>`, sort direction, and filter rule contracts if downstream APIs are repeating them.
- [x] Keep query execution, search scoring, and database filtering in downstream repos.
- [x] Include serialization tests before adoption.

### Scan Progress Contracts

- [x] Add generic scan progress, scan summary, and indexed-file summary models.
- [x] Keep filesystem walking, scheduling, persistence, and retry policy in `core.nas` or tooling repos.
- [x] Align with the media model adoption roadmap before implementation.

## Explicit Non-Goals

- [ ] Do not move `SvengoolieDB` schedule, trigger, stream coordination, or station-specific catalog logic here.
- [ ] Do not move `core.nas` scanner/indexer services here.
- [ ] Do not move CDN upload, delivery, or lowercase filesystem normalization here.
- [ ] Do not move `chop-it` FFmpeg execution, filter presets, or operator UI logic here.
- [ ] Do not move provider-specific API response models here unless they are intentionally normalized provider references.

## Success Criteria

- [x] Each imported model family has a feature folder with README and checklist.
- [x] Each imported model family has tests for serialization and basic invariants.
- [x] Each imported model family has at least one named downstream consumer.
- [x] Contract naming is decided before broad downstream adoption.
- [x] Release notes identify compatibility impact for every new public contract family.
