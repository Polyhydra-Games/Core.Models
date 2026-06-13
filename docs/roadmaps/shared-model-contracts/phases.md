---
title: Shared Model Contracts Phases
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-20
tags: [roadmap, phases, Core.Models, shared-contracts]
---

# Shared Model Contracts Phases

## Canonical Anchor

Canonical doc: [README](./README.md). This file is supporting detail for the roadmap’s canonical Hermes anchor in README.md.

## Phase 0: Contract Rules

- [x] Create `docs/features/model-contract-stability/`.
- [x] Document what qualifies for `Core.Models`.
- [x] Document what must stay in downstream repos.
- [x] Add a compatibility checklist for public model changes.
- [x] Add release-note expectations for shared model changes.

## Phase 1: Strongly Typed Identifiers

- [x] Create `docs/features/strongly-typed-identifiers/`.
- [x] Import the reusable pattern from `OAuth.Core` and streaming-facing IDs.
- [x] Add GUID-backed ID support.
- [x] Add string-backed ID support.
- [x] Add int-backed ID support.
- [x] Add long-backed ID support.
- [x] Add JSON converter strategy.
- [x] Add equality, conversion, invalid input, and round-trip tests.
- [x] Document migration path for `OAuth.Core`, `UserId`, `WebKey`, `TwitchId`, and proposed `IdentityId`.
- [x] Document first-pass migration boundary: reusable base/converter patterns move now; concrete IDs remain downstream until ownership and consumer compatibility are clear.

## Phase 2: Media Asset References

- [x] Create `docs/features/media-asset-references/`.
- [x] Add `MediaAssetReference` model.
- [x] Add asset role/type enum or value object.
- [x] Add optional link to `LocalMediaFile` identity/location.
- [x] Add JSON serialization tests.
- [x] Add examples for poster, trailer, subtitle, CDN image, and local video references.

## Phase 3: External Provider References

- [x] Create `docs/features/external-provider-references/`.
- [x] Add provider reference model.
- [x] Add provider key/value conventions.
- [x] Add tests for multiple providers per asset/catalog item.
- [x] Document that provider API clients remain in `Media-Metadata`.

## Phase 4: Media Relationships

- [x] Create `docs/features/media-relationships/`.
- [x] Add relationship model and relationship role/type values.
- [x] Cover episode-to-video, show-to-poster, media-to-trailer, ROM-to-box-art, and derived-asset scenarios.
- [x] Add serialization tests.
- [x] Validate the shape against one downstream repo before marking stable.

## Phase 5: Playlist And Queue Contracts

- [x] Add first data-only playlist and playlist-item models under `docs/features/media-models/`.
- [x] Add data-only queue/request models.
- [x] Add examples for `chop-it` and `app-busey-box` without adding execution behavior.
- [x] Add serialization tests.

## Phase 6: Generic API Utility Contracts

- [x] Create `docs/features/api-utility-contracts/`.
- [x] Add result, validation, readiness, paging, sorting, and filtering models only where duplication is confirmed.
- [x] Keep the first pass small enough that downstream apps can adopt selectively.
- [x] Add tests for null/default/empty states.

## Phase 7: Scan Progress Contracts

- [x] Create `docs/features/scan-progress-contracts/`.
- [x] Add data-only scan status, progress, and summary contracts.
- [x] Align with `core.nas` scanner/indexing surfaces.
- [x] Avoid scheduler, retry, persistence, and filesystem walking logic.

## Phase 8: Adoption And Compatibility

- [x] Pick the first downstream repo for each model family.
- [x] Add mapping/adoption tests in the downstream repo.
- [x] Capture any gaps back into the relevant feature roadmap.
- [x] Decide whether any contracts need renaming before broad adoption.
- [x] Publish only after docs, tests, and release notes are aligned.
