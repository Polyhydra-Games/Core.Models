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

- [ ] Adopt these contracts in `core.nas` by mapping scanner output to `LocalMediaFile`.
- [ ] Adopt these contracts in `SvengoolieDB` where catalog media assets reference local or NAS-backed files.
- [ ] Adopt playlist contracts in `chop-it` generated playlist output.
- [ ] Adopt playlist contracts in `app-busey-box` manifest loading.
- [ ] Audit CDN models for overlap with `MediaFileLocation`.
- [ ] Add one downstream serialization sample once adoption starts.

## Decisions

- [ ] Decide whether `MediaKind.Rom` should stay as-is or become `GameRom`.
- [x] Decide whether playlist contracts belong in this namespace.
- [ ] Decide whether checksum type or algorithm should be modeled explicitly.

## Later

- [ ] Add optional chapter/subtitle/stream-list models if real consumers need them.
- [ ] Add compatibility tests for downstream JSON payloads.
- [ ] Document breaking-change rules for shared model evolution.
- [ ] Add richer playlist transition/cue models only after a real downstream consumer needs them.
