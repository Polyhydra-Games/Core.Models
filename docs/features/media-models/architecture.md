---
title: Media Models Architecture
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-06
tags: [feature, architecture, media, Core.Models]
---

# Media Models Architecture

## Boundary

`Core.Models` provides shared contracts only. The library should stay dependency-light and should not take ownership of filesystem scanning, ffprobe execution, provider lookups, database storage, or playback workflows.

## Namespace

All media contracts currently live under:

```csharp
PolyhydraGames.Core.Models.Media
```

Playlist and playout contracts live under:

```csharp
PolyhydraGames.Core.Models.Media.Playlists
```

## Data Flow

- [x] A downstream scanner discovers a file from a configured root.
- [x] The scanner normalizes path and extension values with `MediaPathHelpers`.
- [x] The scanner or caller assigns `MediaKind` and `MediaAvailability`.
- [x] Optional technical metadata is attached when a downstream project has extracted it.
- [x] Optional enrichment metadata is attached when a downstream project has catalog or provider data.
- [x] The resulting `LocalMediaFile` can be serialized, persisted, or mapped to app-specific records.
- [x] A downstream playlist generator can emit `MediaPlaylist` with source, playback, and item metadata.
- [x] A downstream playout app can resolve `MediaPlaylistItem` by embedded `LocalMediaFile`, media id, relative path, or full path.

## Contract Shape

- `MediaScanRoot` describes a logical storage root.
- `MediaFileLocation` describes a file's relationship to a root, optional full path, and optional public URL.
- `LocalMediaFile` is the shared file DTO.
- `MediaTechnicalMetadata` groups optional video, audio, image, and ROM metadata.
- `MediaEnrichmentMetadata` groups optional catalog/provider data such as title, description, release year, runtime, genres, provider keys, and artwork links.
- `MediaPlaylist` describes portable playlist or playout manifest identity, source, playback options, and items.
- `MediaPlaylistSource` describes the source root or generator input without owning scanner behavior.
- `MediaPlaylistItem` describes playable item identity, path/media reference, overlay hints, now-playing text, and optional segment timing.
- `MediaPlaylistHelpers` provides dependency-free validation and now-playing fallback behavior.
- `MediaPathHelpers` keeps normalization behavior available without taking a dependency on scanner services.

## Compatibility Notes

- [x] Optional fields are used where not every app can safely provide the value.
- [x] `MediaFileId` is a string to avoid forcing one persistence strategy.
- [x] `FullPath` and `PublicUrl` are optional because some callers should not expose machine-local paths or delivery URLs.
- [x] `Enrichment` is optional because not every caller has catalog or provider data to attach.
- [x] Playlist output targets, RTMP settings, FFmpeg commands, Docker runtime settings, and Twitch credentials stay out of `Core.Models`.
- [ ] Downstream adopters should map into these contracts rather than changing their persistence models in the first pass.
