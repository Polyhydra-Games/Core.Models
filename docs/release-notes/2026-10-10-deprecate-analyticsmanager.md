# AnalyticsManager deprecated (#47)

## Summary

- Contract changed: AnalyticsManager is marked [Obsolete]
- Change type: deprecation
- Package version: next 2.0.1.x

## Compatibility

- Package consumers: Callers of AnalyticsManager (rpg-pf-assistant, SFCharacter.Core) get a CS0618 warning.
- Project-reference consumers: same as package consumers
- Serialization impact: none
- Nullability or required-field impact: none
- Enum or identifier impact: none

## Migration Notes

- Required consumer change: Migrate to ILogger / System.Diagnostics.Metrics.
- Safe rollout order: n/a
- Deprecated member removal target: next major version

## Validation

- `dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj`: passes on this branch.
