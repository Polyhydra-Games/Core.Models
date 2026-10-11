# Core.Models

**Location:** `~/code/Core.Models`

**Purpose:** Core shared models and utilities — common DTOs, helpers used across the ecosystem.

**Assembly:** `PolyhydraGames.Core.Models`

**NuGet Dependencies:**
- `PolyhydraGames.Core.Interfaces`
- `PolyhydraGames.Extensions`
- `System.Reactive`

**Target Frameworks:** net9.0 and net10.0

## Key Types

| Type | Purpose |
|------|---------|
| `Cache<T>` | Simple in-memory cache with expiration |
| `CacheHelper` | Cache utilities |
| `CSVExtensions` | CSV parsing/generation |
| `JsonHelper` | JSON serialization helpers |
| `IStronglyTypedId<TValue>` | Marker contract for primitive-backed identifier value objects |
| `IGuidId` | Common contract for GUID-backed identifiers |
| `IStringId` | Common contract for string-backed identifiers |
| `IOptionalStringId` | Common contract for string-backed identifiers where empty means missing |
| `StringIdHelpers` | Shared normalization helpers for optional string identifiers |
| `StronglyTypedId` | Factory and parse helpers for primitive-backed identifier value objects |
| `StronglyTypedGuidConverter<TId>` | JSON converter for GUID-backed identifier structs |
| `StronglyTypedStringConverter<TId>` | JSON converter for string-backed identifier structs |
| `StronglyTypedOptionalStringConverter<TId>` | JSON converter for optional string-backed identifier structs |
| `StronglyTypedInt32Converter<TId>` | JSON converter for int-backed identifier structs |
| `StronglyTypedInt64Converter<TId>` | JSON converter for long-backed identifier structs |
| `StronglyTypedIdJsonConverterFactory` | JSON converter factory for supported strongly typed ID backing types |
| `LocalMediaFile` | Shared local/NAS-backed media file model |
| `MediaScanRoot` | Shared scan-root model for local and network media libraries |
| `MediaTechnicalMetadata` | Shared technical metadata model for video/audio/image/ROM details |
| `MediaRelationshipKind` | Shared directional relationship kind enum |
| `MediaRelationshipEndpointKind` | Shared entity kind enum for relationship endpoints |
| `MediaRelationship` | Shared directional relationship model between catalog and asset entities |
| `MediaRelationshipEndpoint` | Shared entity reference used by media relationships |
| `MediaQueueItemState` | Shared lifecycle state for playback queue items |
| `MediaPlaybackQueueRequest` | Shared request for queueing media playback items |
| `MediaPlaybackQueue` | Shared playback queue snapshot |
| `ApiOperationResult` | Shared success/failure result envelope |
| `ApiOperationResult<T>` | Shared typed success/failure result envelope |
| `ApiReadinessSummary` | Shared readiness summary for operator surfaces |
| `PagedRequest` | Shared page-based query request contract |
| `PagedResult<T>` | Shared page-based response contract |
| `MediaScanProgress` | Shared scan-progress snapshot for media indexing jobs |
| `MediaScanSummary` | Shared scan summary for media indexing jobs |
| `MediaAssetReference` | Shared portable media asset reference model |
| `MediaProviderReference` | Shared portable external provider reference model |
| `MediaPlaylist` | Shared playlist/playout manifest model under `PolyhydraGames.Core.Models.Media.Playlists` |
| `MediaPathHelpers` | Pure path and extension normalization helpers |
| `ObservableExtensions` | Reactive extensions |
| `ObservableFileMonitor` | File system watcher wrapper |
| `OneTimeUseBool` | Bool that flips from true to false once |
| `ToggleBoolean` | State toggle helper |
| `TimeStamp` | Timestamp wrapper |
| `EnumDescription` | Enum metadata + display helpers |
| `InjectableSave` | Save mechanism for injectable services |

## Dependencies From

- OAuth.Core
- All API projects
- Spotabot
- Channel Cheevos
- core.nas
- SvengoolieDB
- chop-it

## Status

✅ **Working** — Stable utility library.

## Feature Docs

- [Media Models](features/media-models/README.md)
- [Strongly Typed Identifiers](features/strongly-typed-identifiers/README.md)
