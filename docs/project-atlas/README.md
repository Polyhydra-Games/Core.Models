# Project Atlas

## Purpose

`Core.Models` publishes shared DTOs, identifiers, media contracts, API envelopes, and small utility helpers for PolyhydraGames projects.

## Public Surfaces

- `Models/PolyhydraGames.Core.Models.csproj` packages `PolyhydraGames.Core.Models` for NuGet consumers.
- `Tests/PolyhydraGames.Core.Models.Test.csproj` verifies serialization, helper behavior, media contracts, identifiers, queues, scan progress, and API utility contracts.
- `.github/workflows/publish-nuget.yml` restores, tests, packs, and publishes package artifacts.

## Test Ownership Map

- Identifier contracts: `Tests/StronglyTypedIdentifierTests.cs`
- Media contracts and scan roots: `Tests/MediaModelsTests.cs`
- Queue request contracts: `Tests/QueueRequestContractsTests.cs`
- API utility contracts: `Tests/ApiUtilityContractsTests.cs`
- Scan progress contracts: `Tests/ScanProgressContractsTests.cs`
- Serialization helpers: `Tests/JsonHelperTests.cs`
- Utility helpers: `Tests/CacheTests.cs`, `Tests/ContentTypeTests.cs`, `Tests/DelayTest.cs`, `Tests/OneTimeUseBoolTests.cs`

## Compatibility Expectations

Package consumers should expect:

- `net9.0` and `net10.0` package assets.
- Backward-compatible additive model changes by default.
- Release notes for model member removals, renames, type changes, required-field changes, enum semantic shifts, or serialization behavior changes.

Project-reference consumers should expect:

- The same public API compatibility rules as package consumers.
- Local builds to require sibling `Core.Interfaces` when using the checked-out source tree.
- Downstream validation when shared media contracts change.

## Common Consumer Example

```csharp
using PolyhydraGames.Core.Models.Media;

var file = new LocalMediaFile
{
    Id = "movie-001",
    FileName = "movie.mkv",
    MediaType = LocalMediaType.Video
};
```

## Naming Review

- Repository: `Core.Models`.
- Package ID: `PolyhydraGames.Core.Models`.
- Root namespace family: `PolyhydraGames.Core.Models`.
- Related package dependency: `PolyhydraGames.Core.Interfaces`.

Keep reusable contracts in this repo. Product-specific catalog behavior, storage indexing, and operator workflows belong in consuming repos.

## Validation

Run from the real repo path:

```bash
dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj --configuration Release
dotnet pack Models/PolyhydraGames.Core.Models.csproj --configuration Release --no-restore --output artifacts/packages
dotnet list Models/PolyhydraGames.Core.Models.csproj package --include-transitive --vulnerable
```

For media-contract changes, also run:

```bash
cd /mnt/data/lancer1977/code/core.nas
dotnet test src/NasDb.Tests/NasDb.Tests.csproj --configuration Release
```
