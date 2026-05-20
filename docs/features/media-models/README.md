---
title: Media Models
status: in-progress
owner: @DreadBreadcrumb
priority: high
complexity: 2
created: 2026-05-04
updated: 2026-05-20
tags: [feature, Core.Models, media, local-files, shared-models]
---

# Media Models

## Summary

`PolyhydraGames.Core.Models.Media` defines shared model contracts for local and NAS-backed media files. The feature gives `core.nas`, `SvengoolieDB`, `chop-it`, `app-busey-box`, and CDN-related tooling one common language for file identity, storage location, availability, broad media kind, technical metadata, and optional catalog/provider enrichment.

This feature intentionally does not own scanning, database indexing, ffprobe execution, playback, playlist orchestration, or external title lookup. Those remain in application or service projects.

## Current Status

- [x] Shared media model namespace exists at `PolyhydraGames.Core.Models.Media`.
- [x] Broad file classification is represented by `MediaKind`.
- [x] Local and NAS file state is represented by `MediaAvailability`.
- [x] Mounted or local scan roots are represented by `MediaScanRoot`.
- [x] File storage details are represented by `MediaFileLocation`.
- [x] The canonical shared file DTO is `LocalMediaFile`.
- [x] Technical metadata is modeled for video, audio, image, and ROM files.
- [x] Optional enrichment metadata is modeled for catalog/provider overlays.
- [x] Portable media asset references are modeled under `MediaAssetReference`.
- [x] Portable external provider references are modeled under `MediaProviderReference`.
- [x] Portable relationship contracts are modeled under `MediaRelationship`.
- [x] Portable playlist/playout contracts live under `PolyhydraGames.Core.Models.Media.Playlists`.
- [x] Path and extension helpers are dependency-free.
- [x] Serialization and helper behavior is covered by tests.
- [x] Downstream repositories are now adopting the shared contracts in code, with compatibility checks in `core.nas`, `SvengoolieDB`, and `chop-it`.

## Ownership

`Core.Models` owns:

- [x] Shared DTOs and enums.
- [x] Pure normalization and classification helpers.
- [x] Stable cross-project contract names.
- [x] Portable playlist identity, source, playback, item, and validation models.
- [x] Contract compatibility notes for broad downstream adoption.

Downstream owners:

- [x] `core.nas` owns filesystem scanning, NAS indexing, ffmpeg/ffprobe metadata extraction, persistence, and availability refresh.
- [x] `SvengoolieDB` owns catalog-specific title, episode, image, and playback mapping.
- [ ] `Media-Metadata` owns external provider metadata such as TVMaze, TMDb, OMDb, TheTVDB, and Watchmode.
- [x] `chop-it` owns concrete playback, clipping, playlist generation, and operator flow.
- [ ] `core.ffmpeg` owns FFmpeg command construction, RTMP/HLS/file output, overlays, runtime text, ffprobe, and process lifecycle.
- [x] `app-busey-box` owns concrete RTMP/Twitch playout, now-playing state, and Docker/runtime broadcast control.

## Model Notes

- `LocalMediaFile.Location.RelativePath` should be portable and slash-normalized.
- `FullPath` is optional so the model can safely cross process or machine boundaries.
- `PublicUrl` is optional and should be set only by a delivery layer.
- `TechnicalMetadata` is optional because not every caller will run ffprobe or image metadata extraction.
- `MediaFileId` is a string so apps can use database IDs, provider keys, or stable synthetic IDs without forcing a shared persistence shape.
- `MediaAssetReference` is intentionally portable and does not own upload, storage, or delivery behavior.
- `MediaProviderReference` is intentionally portable and does not own provider lookup, search, or fetch behavior.
- `MediaRelationship` is intentionally portable and does not own relationship resolution, graph traversal, or persistence behavior.
- `MediaPlaylist` intentionally models portable playlist shape, not FFmpeg output, Twitch credentials, Docker runtime, or operator controls.
- `MediaPlaylist.Metadata` and `MediaPlaylistItem.Metadata` are escape hatches for consumer hints that should not force immediate NuGet churn.

## Validation

- [x] `dotnet test` passes for the repository test project.
- [x] `MediaPathHelpers.NormalizeExtension` covers casing, missing dots, whitespace, empty input, and null input.
- [x] `MediaPathHelpers.NormalizeRelativePath` covers Windows separators, leading slashes, whitespace, and null input.
- [x] `MediaPathHelpers.ClassifyByExtension` covers video, audio, image, ROM, document, unknown, and other extensions.
- [x] `LocalMediaFile` JSON round-trips the shared model shape.
- [x] `MediaPlaylist` JSON round-trips Busey-style manifests and Chop-It segment/source options.
- [x] `MediaPlaylistHelpers` reports invalid manifest shape.
- [x] `LocalMediaFile` round-trips optional enrichment metadata.

## Next Steps

- [x] Add mapping extension methods in `core.nas`.
- [x] Add mapping extension methods in `SvengoolieDB`.
- [ ] Audit CDN file records for overlap with `MediaFileLocation`.
- [x] Decide whether `MediaKind.Rom` should become `GameRom` before broad adoption. Keep `Rom` for compatibility.
- [x] Add first playlist/playout contracts under `Core.Models.Media.Playlists`.
- [x] Add downstream playlist mapping in `chop-it` before expanding fields.
- [x] Add downstream playlist mapping in `app-busey-box` before expanding fields.
- [x] Add relationship contracts for catalog and asset linking.

## Compatibility Notes Captured

- `core.nas` maps `MediaFile` into `LocalMediaFile` using `MediaFile.Id` + `NasServerId` as canonical identity fields and leaves provider IDs out of the shared contract.
- `SvengoolieDB` maps `CatalogAsset` into `LocalMediaFile` using `CatalogAssetId` + `CatalogMovieId` as local identity fields, with movie metadata carried in `MediaEnrichmentMetadata`.
- Neither adopter relies on `FullPath` in `LocalMediaFile` for routing or matching across services; `RelativePath` and `RootId` are the stable fields.

## Cross-Repo Coordination

- [SvengoolieDB media metadata transition](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/README.md>)
- [One-swoop execution map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/execution-map.md>)
- [Cross-project link map](</home/lancer1977/code/SvengoolieDB/docs/roadmaps/media-metadata-transition/link-map.md>)
