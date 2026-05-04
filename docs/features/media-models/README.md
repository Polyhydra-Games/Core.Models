---
title: Media Models
status: in-progress
owner: @DreadBreadcrumb
priority: high
complexity: 2
created: 2026-05-04
updated: 2026-05-04
tags: [feature, Core.Models, media, local-files, shared-models]
---

# Media Models

## Summary

`PolyhydraGames.Core.Models.Media` defines shared model contracts for local and NAS-backed media files. The feature gives `core.nas`, `SvengoolieDB`, `chop-it`, and CDN-related tooling one common language for file identity, storage location, availability, broad media kind, and technical metadata.

This feature intentionally does not own scanning, database indexing, ffprobe execution, playback, playlist orchestration, or external title lookup. Those remain in application or service projects.

## Current Status

- [x] Shared media model namespace exists at `PolyhydraGames.Core.Models.Media`.
- [x] Broad file classification is represented by `MediaKind`.
- [x] Local and NAS file state is represented by `MediaAvailability`.
- [x] Mounted or local scan roots are represented by `MediaScanRoot`.
- [x] File storage details are represented by `MediaFileLocation`.
- [x] The canonical shared file DTO is `LocalMediaFile`.
- [x] Technical metadata is modeled for video, audio, image, and ROM files.
- [x] Path and extension helpers are dependency-free.
- [x] Serialization and helper behavior is covered by tests.
- [ ] Downstream repositories have not yet adopted the shared contracts.

## Ownership

`Core.Models` owns:

- [x] Shared DTOs and enums.
- [x] Pure normalization and classification helpers.
- [x] Stable cross-project contract names.
- [ ] Contract compatibility notes for broad downstream adoption.

Downstream owners:

- [ ] `core.nas` owns filesystem scanning, NAS indexing, ffmpeg/ffprobe metadata extraction, persistence, and availability refresh.
- [ ] `SvengoolieDB` owns catalog-specific title, episode, image, and playback mapping.
- [ ] `Media-Metadata` owns external provider metadata such as TVMaze, TMDb, OMDb, TheTVDB, and Watchmode.
- [ ] `chop-it` owns playback, clipping, playlists, and operator flow.

## Model Notes

- `LocalMediaFile.Location.RelativePath` should be portable and slash-normalized.
- `FullPath` is optional so the model can safely cross process or machine boundaries.
- `PublicUrl` is optional and should be set only by a delivery layer.
- `TechnicalMetadata` is optional because not every caller will run ffprobe or image metadata extraction.
- `MediaFileId` is a string so apps can use database IDs, provider keys, or stable synthetic IDs without forcing a shared persistence shape.

## Validation

- [x] `dotnet test` passes for the repository test project.
- [x] `MediaPathHelpers.NormalizeExtension` covers casing, missing dots, whitespace, empty input, and null input.
- [x] `MediaPathHelpers.NormalizeRelativePath` covers Windows separators, leading slashes, whitespace, and null input.
- [x] `MediaPathHelpers.ClassifyByExtension` covers video, audio, image, ROM, document, unknown, and other extensions.
- [x] `LocalMediaFile` JSON round-trips the shared model shape.

## Next Steps

- [ ] Add mapping extension methods in `core.nas`.
- [ ] Add mapping extension methods in `SvengoolieDB`.
- [ ] Audit CDN file records for overlap with `MediaFileLocation`.
- [ ] Decide whether `MediaKind.Rom` should become `GameRom` before broad adoption.
- [ ] Decide whether playlist contracts belong here or in a later `Core.Models.Media.Playlists` namespace.
