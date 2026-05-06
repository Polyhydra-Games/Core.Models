---
title: Media Model Adoption Roadmap
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-06
tags: [roadmap, media, Core.Models, adoption]
---

# Media Model Adoption Roadmap

## Purpose

Track the phased rollout of `PolyhydraGames.Core.Models.Media` across downstream media, NAS, catalog, clipping, broadcast playout, and CDN projects.

## Current State

- [x] Shared contracts exist in `Core.Models`.
- [x] Helper and serialization tests exist in `Core.Models`.
- [x] First playlist/playout contracts exist under `PolyhydraGames.Core.Models.Media.Playlists`.
- [ ] Downstream repositories still need explicit mapping/adoption work.
- [ ] Contract stability should be confirmed after the first real consumer maps to the shared shape.

## Target Consumers

- [ ] `core.nas`
- [ ] `SvengoolieDB`
- [ ] `chop-it`
- [ ] `app-busey-box`, only for playout input shapes that should be portable
- [ ] CDN/media normalization tooling
- [ ] `Media-Metadata`, only where external provider metadata needs to reference local file records

## Success Criteria

- [ ] At least one downstream scanner maps into `LocalMediaFile`.
- [ ] At least one downstream catalog reads the shared shape without app-specific glue in `Core.Models`.
- [ ] `chop-it` maps generated playlist output into `MediaPlaylist`.
- [ ] `app-busey-box` maps show manifests into `MediaPlaylist`.
- [ ] Serialization behavior is verified with realistic downstream payloads.
- [ ] Open naming questions are resolved before broad adoption.

## Related Roadmaps

- [Shared Model Contracts](../shared-model-contracts/README.md)
- [SvengoolieDB media metadata transition](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/README.md>)
- [One-swoop execution map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/execution-map.md>)
