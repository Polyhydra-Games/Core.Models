---
title: Media Models Feature Roadmap
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-07-06
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

- [ ] Add optional chapter/subtitle/stream-list models only when a real
      downstream consumer needs to serialize or map those shapes.
      Tracking issue: https://github.com/lancer1977/Core.Models/issues/18.
- [x] Add compatibility tests for downstream JSON payloads.
- [x] Document breaking-change rules for shared model evolution.
      Tracking issue: https://github.com/lancer1977/Core.Models/issues/17.
- [ ] Add richer playlist transition/cue models only after a real downstream consumer needs them.

## Optional Model Admission Rules

Optional chapter, subtitle, and stream-list contracts stay out of the package
until a consuming repo can point to a concrete serialized payload, API response,
or persisted shape that needs a shared model. Until then, downstream apps should
keep those details local.

When a consumer does justify one of these families, the Core.Models change must:

- add the smallest optional DTO family needed by that consumer
- include JSON round-trip tests for sparse and populated payloads
- link the consumer issue or PR that proves the shared need
- avoid introducing playback, CDN, catalog, storage, or product behavior
