# Code Health

## Current Status

- Native test gate: `dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj --configuration Release`.
- Package gate: `dotnet pack Models/PolyhydraGames.Core.Models.csproj --configuration Release --no-restore --output artifacts/packages`.
- Advisory gate: `dotnet list Models/PolyhydraGames.Core.Models.csproj package --include-transitive --vulnerable`.
- Downstream compatibility gate: run the closest consumer test, with `core.nas` as the default media-contract consumer.

## Dependency Policy

- Keep direct package references local to the project files until this repo adopts central package management across the Core family in a dedicated pass.
- Treat direct package drift as actionable in this repo.
- Treat transitive drift from `PolyhydraGames.Extensions` or `PolyhydraGames.Core.Interfaces` as upstream unless it creates a vulnerability or runtime failure here.
- No vulnerable packages were reported for the model project or test project in the current validation pass.

## Generated Artifacts

- `artifacts/packages/` contains local NuGet outputs and is not source.
- `bin/` and `obj/` are SDK outputs.
- Release evidence belongs in GitHub Actions logs or issue comments, not checked-in generated package files.

## Public Contract Review

- Public DTOs, identifiers, and helper APIs are package surface.
- Serialization shape, nullable tolerance, and enum value semantics are compatibility-sensitive.
- New shared contracts need tests and at least one consumer validation before release.

## Accepted Follow-ups

- Existing nullability and obsolete warnings are legacy compatibility debt. Keep them visible during pack until a focused breaking-change review decides which public shapes can change.
