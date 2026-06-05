---
title: Media Models Checklist
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-06-03
tags: [feature, checklist, media, Core.Models]
---

# Media Models Checklist

## Canonical Anchor

Canonical doc: [README](./README.md). This checklist mirrors the canonical family status and does not carry a separate Hermes marker.

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
- [x] Add `MediaEnrichmentMetadata`.
- [x] Add `VideoMediaMetadata`.
- [x] Add `AudioMediaMetadata`.
- [x] Add `ImageMediaMetadata`.
- [x] Add `RomMediaMetadata`.
- [x] Add `MediaPlaylist`.
- [x] Add `MediaPlaylistSource`.
- [x] Add `MediaPlaylistPlaybackOptions`.
- [x] Add `MediaPlaylistItem`.
- [x] Add playlist validation helpers.

## Helpers

- [x] Add extension normalization.
- [x] Add relative-path normalization.
- [x] Add extension-based media-kind classification.
- [x] Revisit extension lists after first downstream adoption pass.

## Testing

- [x] Cover extension normalization.
- [x] Cover relative-path normalization.
- [x] Cover extension classification.
- [x] Cover JSON serialization round trip.
- [x] Cover playlist manifest serialization and validation.
- [x] Cover optional enrichment metadata round trip.
- [x] Add downstream mapping tests in consuming repos.

## Release Readiness

- [x] Repository tests pass.
- [x] New public media API members have XML documentation.
- [x] Validate playlist shape in `chop-it` and `app-busey-box` before broad expansion.
- [x] Confirm package version/release notes before publishing.
- [x] Validate adoption in at least one downstream consumer before treating the contract as stable.
