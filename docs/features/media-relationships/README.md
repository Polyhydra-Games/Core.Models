---
title: Media Relationships
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-05-20
tags: [feature, Core.Models, media, relationships, contracts]
---

# Media Relationships

## Summary

`MediaRelationship` is the shared contract family for directional links between catalog items, media files, media assets, provider records, and other media-adjacent entities.
It gives downstream repos a typed way to express episode-to-video, show-to-poster, media-to-trailer, ROM-to-box-art, and derived-asset relationships without moving graph traversal or persistence logic into `Core.Models`.

## Current Shape

- `MediaRelationshipKind` captures the broad directional relationship label.
- `MediaRelationshipEndpointKind` captures the broad entity category at each endpoint.
- `MediaRelationship.Source` and `MediaRelationship.Target` carry typed endpoints.
- `MediaRelationshipEndpoint.EntityId` is the stable downstream identifier for the endpoint.
- `MediaRelationshipEndpoint.RelativePath`, `FullPath`, and `Url` provide lightweight file or delivery hints where needed.
- `MediaRelationship.Metadata` and `MediaRelationshipEndpoint.Metadata` are escape hatches for consumer hints that should not become first-class fields yet.

## Examples

- Episode to video file:
  - `Kind = EpisodeOf`
  - `Source.Kind = CatalogItem`
  - `Target.Kind = MediaFile`
- Show to poster:
  - `Kind = PosterFor`
  - `Source.Kind = CatalogItem`
  - `Target.Kind = MediaAsset`
- Media to trailer:
  - `Kind = TrailerFor`
  - `Source.Kind = CatalogItem`
  - `Target.Kind = MediaAsset`
- ROM to box art:
  - `Kind = BoxArtFor`
  - `Source.Kind = MediaFile`
  - `Target.Kind = MediaAsset`
- Derived asset:
  - `Kind = DerivedFrom`
  - `Source.Kind = MediaAsset`
  - `Target.Kind = MediaAsset`

## Boundary

- `Core.Models` owns the portable relationship shape.
- Downstream repos own matching, indexing, fetching, graph traversal, and persistence behavior.
- Consumers should map into this contract rather than introducing ad hoc graph DTOs on the first pass.

## Validation

- Serialization coverage exists in the `Core.Models` test project.
- The first adopter can now link catalog and asset entities without adding another shared relationship shape.

## Next Steps

- [x] Add a downstream adoption example in a real consumer repo.
- [ ] Decide whether future relationship kinds need a separate role enum.
