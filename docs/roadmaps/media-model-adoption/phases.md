---
title: Media Model Adoption Phases
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-06
tags: [roadmap, phases, media, Core.Models]
---

# Media Model Adoption Phases

## Phase 1: Contract Stabilization

- [x] Add shared contracts to `Core.Models`.
- [x] Add focused helper and serialization tests.
- [x] Document the feature boundary.
- [x] Add first portable playlist/playout contracts.
- [x] Resolve `Rom` versus `GameRom` naming. Keep `Rom` for compatibility with current downstream consumers.
- [x] Decide whether checksum algorithm belongs in the model. Keep the checksum as a string field for now.

## Phase 2: First Consumer

- [ ] Pick first downstream repo.
- [ ] Add mapping extensions in the downstream repo.
- [x] Map `chop-it` playlist output into `MediaPlaylist`.
- [x] Map `app-busey-box` manifests into `MediaPlaylist`.
- [x] Add mapping tests with realistic data.
- [ ] Capture any model gaps back in `docs/features/media-models/roadmap.md`.

## Phase 3: Catalog And CDN Adoption

- [ ] Map catalog media assets to shared location fields.
- [x] Audit CDN records for reusable location/path concepts. Current CDN file descriptors stay delivery-specific; no shared location shape was promoted yet.
- [ ] Verify URL/path boundaries are still safe.
- [ ] Add documentation for app-specific ownership.

## Phase 4: Compatibility Gate

- [ ] Add compatibility examples or snapshot tests.
- [ ] Document versioning expectations for shared media contracts.
- [ ] Confirm release notes before publishing a package with these contracts.
