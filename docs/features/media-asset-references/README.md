---
title: Media Asset References
status: active
owner: @DreadBreadcrumb
created: 2026-05-20
updated: 2026-05-20
tags: [feature, Core.Models, media, references, contracts]
---

# Media Asset References

## Summary

`MediaAssetReference` is the shared contract family for portable references to media-adjacent assets.
It covers local files, CDN-delivered assets, provider images, posters, trailers, subtitles, and companion media without moving storage or delivery behavior into `Core.Models`.

## Current Shape

- `MediaAssetReferenceKind` captures the broad asset category.
- `MediaAssetReference.Role` captures an app-specific or human-friendly role label.
- `MediaAssetReference.LocalFile` can embed a full `LocalMediaFile` when the asset is backed by local/NAS storage.
- `MediaAssetReference.Location` can carry a portable local storage location without embedding the full file contract.
- `MediaAssetReference.Url` can point at a CDN or other externally reachable delivery URL.
- `MediaAssetReference.Metadata` is an escape hatch for consumer hints that should not become first-class fields yet.

## Examples

- Poster art:
  - `Kind = Poster`
  - `Role = poster`
  - `Url = https://cdn.example.com/posters/the-blob.jpg`
- Trailer clip:
  - `Kind = Trailer`
  - `Role = trailer`
  - `Location.RelativePath = trailers/the-blob-trailer.mp4`
- Subtitle sidecar:
  - `Kind = Subtitle`
  - `Role = subtitles`
  - `Location.RelativePath = subs/the-blob.en.srt`
- CDN image:
  - `Kind = CdnAsset`
  - `Role = image`
  - `Url = https://cdn.example.com/images/the-blob-card.png`
- Local video asset:
  - `Kind = LocalFile`
  - `LocalFile.FileName = The Blob (1958).mkv`
  - `LocalFile.Location.RelativePath = Movies/The Blob (1958).mkv`

## Boundary

- `Core.Models` owns the portable reference shape.
- Downstream repos own fetching, storing, uploading, and delivering the underlying bytes.
- Consumers should map into this contract rather than changing their persistence models on the first pass.

## Validation

- Serialization coverage exists in the `Core.Models` test project.
- The first adopter can now be a downstream media, catalog, or CDN repo without adding a new shared contract shape.

## Next Steps

- [x] Add provider reference contracts.
- [x] Add relationship contracts.
- [x] Add downstream adoption examples in a real consumer repo.
