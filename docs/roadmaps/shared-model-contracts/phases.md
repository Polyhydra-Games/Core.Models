---
title: Shared Model Contracts Phases
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-06
tags: [roadmap, phases, Core.Models, shared-contracts]
---

# Shared Model Contracts Phases

## Phase 0: Contract Rules

- [ ] Create `docs/features/model-contract-stability/`.
- [ ] Document what qualifies for `Core.Models`.
- [ ] Document what must stay in downstream repos.
- [ ] Add a compatibility checklist for public model changes.
- [ ] Add release-note expectations for shared model changes.

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

## Phase 2: Media Asset References

- [ ] Create `docs/features/media-asset-references/`.
- [ ] Add `MediaAssetReference` model.
- [ ] Add asset role/type enum or value object.
- [ ] Add optional link to `LocalMediaFile` identity/location.
- [ ] Add JSON serialization tests.
- [ ] Add examples for poster, trailer, subtitle, CDN image, and local video references.

## Phase 3: External Provider References

- [ ] Create `docs/features/external-provider-references/`.
- [ ] Add provider reference model.
- [ ] Add provider key/value conventions.
- [ ] Add tests for multiple providers per asset/catalog item.
- [ ] Document that provider API clients remain in `Media-Metadata`.

## Phase 4: Media Relationships

- [ ] Create `docs/features/media-relationships/`.
- [ ] Add relationship model and relationship role/type values.
- [ ] Cover episode-to-video, show-to-poster, media-to-trailer, ROM-to-box-art, and derived-asset scenarios.
- [ ] Add serialization tests.
- [ ] Validate the shape against one downstream repo before marking stable.

## Phase 5: Playlist And Queue Contracts

- [x] Add first data-only playlist and playlist-item models under `docs/features/media-models/`.
- [ ] Add data-only queue/request models.
- [x] Add examples for `chop-it` and `app-busey-box` without adding execution behavior.
- [x] Add serialization tests.

## Phase 6: Generic API Utility Contracts

- [ ] Create `docs/features/api-utility-contracts/`.
- [ ] Add result, validation, readiness, paging, sorting, and filtering models only where duplication is confirmed.
- [ ] Keep the first pass small enough that downstream apps can adopt selectively.
- [ ] Add tests for null/default/empty states.

## Phase 7: Scan Progress Contracts

- [ ] Create `docs/features/scan-progress-contracts/`.
- [ ] Add data-only scan status, progress, and summary contracts.
- [ ] Align with `core.nas` scanner/indexing surfaces.
- [ ] Avoid scheduler, retry, persistence, and filesystem walking logic.

## Phase 8: Adoption And Compatibility

- [ ] Pick the first downstream repo for each model family.
- [ ] Add mapping/adoption tests in the downstream repo.
- [ ] Capture any gaps back into the relevant feature roadmap.
- [ ] Decide whether any contracts need renaming before broad adoption.
- [ ] Publish only after docs, tests, and release notes are aligned.
