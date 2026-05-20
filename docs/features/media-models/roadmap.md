---
title: Media Models Feature Roadmap
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-06
tags: [feature, roadmap, media, Core.Models]
---

# Media Models Feature Roadmap

## Near Term

- [x] Adopt these contracts in `core.nas` by mapping scanner output to `LocalMediaFile`.
- [x] Adopt these contracts in `SvengoolieDB` where catalog media assets reference local or NAS-backed files.
- [x] Adopt playlist contracts in `chop-it` generated playlist output.
- [x] Adopt playlist contracts in `app-busey-box` manifest loading.
- [x] Audit CDN models for overlap with `MediaFileLocation`. Current CDN file descriptors stay delivery-specific; no shared location shape was promoted yet.
- [x] Add one downstream serialization sample once adoption starts.
- [x] Model optional enrichment metadata for catalog/provider overlays.

## Decisions

- [x] Decide whether `MediaKind.Rom` should stay as-is or become `GameRom`. Keep `Rom` as the compatibility name.
- [x] Decide whether playlist contracts belong in this namespace.
- [x] Decide whether checksum type or algorithm should be modeled explicitly. Keep the checksum as a plain string for now.

## Later

- [ ] Add optional chapter/subtitle/stream-list models if real consumers need them.
- [ ] Add compatibility tests for downstream JSON payloads.
- [ ] Document breaking-change rules for shared model evolution.
- [ ] Add richer playlist transition/cue models only after a real downstream consumer needs them.
