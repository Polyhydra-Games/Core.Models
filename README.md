# Core.Models

A .NET utility library providing common models, helpers, and reactive extensions for the PolyhydraGames ecosystem.

## Tags

- dotnet
- core
- core-models
- auth
- docs
- models

## Related Repos

- [`Core`](../Core/)
- [`Core.Interfaces`](../Core.Interfaces/)
- [`Core.Extensions`](../Core.Extensions/)
- [`Core.Serialization`](../Core.Serialization/)
- [`Core.SQL`](../Core.SQL/)

## What This Library Provides

`PolyhydraGames.Core.Models` is a shared utility library containing common DTOs, caching, serialization helpers, and reactive extensions used across multiple projects. It eliminates duplication by centralizing frequently-needed model types and helpers.

### Key Types

| Category | Type | Purpose |
|----------|------|---------|
| **Caching** | `Cache<T>` | Simple in-memory cache with automatic expiration |
| | `CacheHelper` | Utility methods for cache management |
| **Data** | `CSVExtensions` | CSV parsing and generation extension methods |
| | `JsonHelper` | JSON serialization/deserialization helpers |
| **Identifiers** | `IStronglyTypedId<TValue>` | Marker contract for primitive-backed identifier value objects |
| | `IGuidId` | Common contract for GUID-backed identifiers |
| | `IStringId` | Common contract for string-backed identifiers |
| | `IOptionalStringId` | Common contract for string-backed identifiers where empty means missing |
| | `StringIdHelpers` | Shared normalization helpers for optional string identifiers |
| | `StronglyTypedId` | Factory and parse helpers for primitive-backed identifier value objects |
| | `StronglyTypedGuidConverter<TId>` | JSON converter for GUID-backed identifier structs |
| | `StronglyTypedStringConverter<TId>` | JSON converter for string-backed identifier structs |
| | `StronglyTypedOptionalStringConverter<TId>` | JSON converter for optional string-backed identifier structs |
| | `StronglyTypedInt32Converter<TId>` | JSON converter for int-backed identifier structs |
| | `StronglyTypedInt64Converter<TId>` | JSON converter for long-backed identifier structs |
| | `StronglyTypedIdJsonConverterFactory` | JSON converter factory for supported strongly typed ID backing types |
| **Media** | `LocalMediaFile` | Shared model for local/NAS-backed media files |
| | `MediaScanRoot` | Shared model for filesystem or NAS scan roots |
| | `MediaTechnicalMetadata` | Shared technical metadata container for video/audio/image/ROM details |
| | `MediaRelationshipKind` | Shared directional relationship kind enum |
| | `MediaRelationshipEndpointKind` | Shared entity kind enum for relationship endpoints |
| | `MediaRelationship` | Shared directional relationship model between catalog and asset entities |
| | `MediaRelationshipEndpoint` | Shared entity reference used by media relationships |
| | `MediaQueueItemState` | Shared lifecycle state for playback queue items |
| | `MediaPlaybackQueueRequest` | Shared request for queueing media playback items |
| | `MediaPlaybackQueue` | Shared playback queue snapshot |
| | `ApiOperationResult` | Shared success/failure result envelope |
| | `ApiOperationResult<T>` | Shared typed success/failure result envelope |
| | `ApiReadinessSummary` | Shared readiness summary for operator surfaces |
| | `PagedRequest` | Shared page-based query request contract |
| | `PagedResult<T>` | Shared page-based response contract |
| | `MediaScanProgress` | Shared scan-progress snapshot for media indexing jobs |
| | `MediaScanSummary` | Shared scan summary for media indexing jobs |
| | `MediaAssetReference` | Shared portable media asset reference model |
| | `MediaProviderReference` | Shared portable external provider reference model |
| | `MediaPlaylist` | Shared playlist/playout manifest for media, clipping, and broadcast consumers |
| | `MediaPathHelpers` | Pure path, extension, and media-kind normalization helpers |
| **Reactive** | `ObservableExtensions` | Custom reactive extensions for System.Reactive |
| | `ObservableFileMonitor` | File system watcher wrapper using observables |
| | `SubjectObservable<T>` | Subject-based observable implementation |
| **Enums** | `EnumDescription` | Enum metadata and display name resolution |
| | `EnumDescriptionExtensions` | Extension methods for enum description lookup |
| **State** | `OneTimeUseBool` | Boolean that flips from `true` to `false` after first read |
| | `ToggleBoolean` | State toggle helper |
| | `InjectableSave` | Save mechanism for dependency-injectable services |
| **Network** | `PortReply` | Network port reply model |
| | `PortReplyExtensions` | Extension methods for port reply handling |
| **Utilities** | `TimeStamp` | Timestamp wrapper with formatting helpers |
| | `DelayTimer` | Timer utility for delayed operations |
| | `IPortListener` | Interface for port listening services |

## Common Use Cases

### 1. Caching Data
```csharp
var cache = new Cache<MyData>(myData, TimeSpan.FromMinutes(5));
var result = cache.Value; // null/default if expired
```

### 2. Watching Files Reactively
```csharp
var monitor = new ObservableFileMonitor("path/to", "file.txt");
monitor.Observable.Subscribe(path => Console.WriteLine($"File changed: {path}"));
monitor.Initialize();
```

### 3. Enum Descriptions
```csharp
var description = new EnumDescription<MyEnum>(
    MyEnum.SomeValue,
    "Some Value",
    "Display description");
```

### 4. CSV Parsing
```csharp
var records = csvContent.FromCSV<MyRecord>();
var output = records.ToCSV();
```

### 5. JSON Helpers
```csharp
var json = myObject.ToJson();
var obj = json.FromJson<MyType>();
```

## Integration Instructions

### Install via NuGet
```xml
<PackageReference Include="PolyhydraGames.Core.Models" Version="1.0.2.10" />
```

The package is published publicly on nuget.org. New `main` builds publish `1.0.2.<run_number>` when the repository has a valid `NUGET_API_KEY` secret.

### Target Frameworks
- **Main package:** `net8.0` and `net9.0` (multi-target)
- **Test project:** `net10.0`

### Prerequisites
- .NET 10.0 SDK or later for the current test project
- .NET 8.0 and .NET 9.0 targeting packs for the library targets
- The following NuGet packages are automatically resolved:
  - `PolyhydraGames.Core.Interfaces` (v2.0.0.45)
  - `PolyhydraGames.Extensions` (v2.1.1.78)
  - `System.Reactive` (v6.1.0)

### Building
```bash
# Build the solution
dotnet build PolyhydraGames.Core.Models.sln

# Run tests
dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj
```

### Typical Usage
```csharp
using PolyhydraGames.Core.Models;
using PolyhydraGames.Core.Models.Identifiers;
using PolyhydraGames.Core.Models.Media;

// Now use any of the types listed above
```

## Projects That Use This Library

- OAuth.Core
- All API projects
- Spotabot
- Channel Cheevos


## 📖 Documentation
Detailed documentation can be found in the following sections:
- [Feature Index](./docs/features/README.md)
- [Core Capabilities](./docs/features/core-capabilities.md)
- [Media Models](./docs/features/media-models/README.md)
- [Strongly Typed Identifiers](./docs/features/strongly-typed-identifiers/README.md)
