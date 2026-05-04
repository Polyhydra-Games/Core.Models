---
title: Media Models Checklist
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-04
tags: [feature, checklist, media, Core.Models]
---

# Media Models Checklist

## Discovery

- [x] Identify shared consumers: `core.nas`, `SvengoolieDB`, `chop-it`, CDN tooling.
- [x] Keep scanner, persistence, playback, and provider lookup outside `Core.Models`.
- [x] Define a minimal contract that can cross project boundaries.

## Documentation

- [x] Create feature documentation under `docs/features/media-models/`.
- [x] Update the feature index.
- [x] Update the root README key type list.
- [x] Update `docs/core-models.md`.
- [x] Add adoption roadmap documentation.

## Models

- [x] Add `MediaKind`.
- [x] Add `MediaAvailability`.
- [x] Add `MediaScanRoot`.
- [x] Add `MediaFileLocation`.
- [x] Add `LocalMediaFile`.
- [x] Add `MediaTechnicalMetadata`.
- [x] Add `VideoMediaMetadata`.
- [x] Add `AudioMediaMetadata`.
- [x] Add `ImageMediaMetadata`.
- [x] Add `RomMediaMetadata`.

## Helpers

- [x] Add extension normalization.
- [x] Add relative-path normalization.
- [x] Add extension-based media-kind classification.
- [ ] Revisit extension lists after first downstream adoption pass.

## Testing

- [x] Cover extension normalization.
- [x] Cover relative-path normalization.
- [x] Cover extension classification.
- [x] Cover JSON serialization round trip.
- [ ] Add downstream mapping tests in consuming repos.

## Release Readiness

- [x] Repository tests pass.
- [x] New public media API members have XML documentation.
- [ ] Confirm package version/release notes before publishing.
- [ ] Validate adoption in at least one downstream consumer before treating the contract as stable.
