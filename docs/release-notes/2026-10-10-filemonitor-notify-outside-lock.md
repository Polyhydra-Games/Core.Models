# ObservableFileMonitor notifications raised outside the lock (#47)

## Summary

- Contract changed: ObservableFileMonitor.OnFileChanged
- Change type: behavioral
- Package version: next 2.0.1.x

## Compatibility

- Package consumers: Subscribers are now called without the monitor's internal lock held; a notification racing Dispose() is dropped instead of throwing.
- Project-reference consumers: same as package consumers
- Serialization impact: none
- Nullability or required-field impact: none
- Enum or identifier impact: none

## Migration Notes

- Required consumer change: None expected.
- Safe rollout order: n/a
- Deprecated member removal target: n/a

## Validation

- `dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj`: passes on this branch.
