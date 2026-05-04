---
title: Media Model Adoption Phases
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-04
tags: [roadmap, phases, media, Core.Models]
---

# Media Model Adoption Phases

## Phase 1: Contract Stabilization

- [x] Add shared contracts to `Core.Models`.
- [x] Add focused helper and serialization tests.
- [x] Document the feature boundary.
- [ ] Resolve `Rom` versus `GameRom` naming.
- [ ] Decide whether checksum algorithm belongs in the model.

## Phase 2: First Consumer

- [ ] Pick first downstream repo.
- [ ] Add mapping extensions in the downstream repo.
- [ ] Add mapping tests with realistic data.
- [ ] Capture any model gaps back in `docs/features/media-models/roadmap.md`.

## Phase 3: Catalog And CDN Adoption

- [ ] Map catalog media assets to shared location fields.
- [ ] Audit CDN records for reusable location/path concepts.
- [ ] Verify URL/path boundaries are still safe.
- [ ] Add documentation for app-specific ownership.

## Phase 4: Compatibility Gate

- [ ] Add compatibility examples or snapshot tests.
- [ ] Document versioning expectations for shared media contracts.
- [ ] Confirm release notes before publishing a package with these contracts.
