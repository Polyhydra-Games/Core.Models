# Contributing

Thanks for improving the library.

## Before opening a change

1. Keep changes focused on portable library behavior, public documentation, or tests.
2. Add or update tests for behavior changes.
3. Run the validation commands below from the repository root.
4. Do not add credentials, machine-specific paths, deployment material, or generated package artifacts.

```bash
dotnet restore PolyhydraGames.Core.Models.sln
dotnet build PolyhydraGames.Core.Models.sln --configuration Release --no-restore
dotnet test PolyhydraGames.Core.Models.sln --configuration Release --no-build
dotnet pack Models/PolyhydraGames.Core.Models.csproj --configuration Release --no-build --output artifacts/packages
```

## Compatibility

Changes to public contracts and JSON serialization are consumer-facing. Preserve existing property names, defaults, and converter behavior unless a documented breaking change is intentional.

## Pull requests

Describe the consumer impact, validation performed, and any compatibility implications. The CI workflow builds, tests, and creates local package artifacts only; it does not publish packages.
