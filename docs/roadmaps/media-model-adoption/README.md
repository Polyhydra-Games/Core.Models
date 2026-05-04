---
title: Media Model Adoption Roadmap
status: planned
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-04
tags: [roadmap, media, Core.Models, adoption]
---

# Media Model Adoption Roadmap

## Purpose

Track the phased rollout of `PolyhydraGames.Core.Models.Media` across downstream media, NAS, catalog, CDN, and clipping projects.

## Current State

- [x] Shared contracts exist in `Core.Models`.
- [x] Helper and serialization tests exist in `Core.Models`.
- [ ] Downstream repositories still need explicit mapping/adoption work.
- [ ] Contract stability should be confirmed after the first real consumer maps to the shared shape.

## Target Consumers

- [ ] `core.nas`
- [ ] `SvengoolieDB`
- [ ] `chop-it`
- [ ] CDN/media normalization tooling
- [ ] `Media-Metadata`, only where external provider metadata needs to reference local file records

## Success Criteria

- [ ] At least one downstream scanner maps into `LocalMediaFile`.
- [ ] At least one downstream catalog reads the shared shape without app-specific glue in `Core.Models`.
- [ ] Serialization behavior is verified with realistic downstream payloads.
- [ ] Open naming questions are resolved before broad adoption.
