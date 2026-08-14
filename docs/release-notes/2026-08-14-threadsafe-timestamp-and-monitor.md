# Shared Contract Change Notes

## Summary

- Contract changed: `TimeStamp` and `ObservableFileMonitor` runtime behavior.
- Change type: behavioral, with additive `IDisposable` support on `ObservableFileMonitor`.
- Package version: next `2.0.0.*` build.

## Compatibility

- Package and project-reference consumers remain source compatible.
- `ObservableFileMonitor` can now be disposed by consumers that control its lifetime.
- No serialization, nullability, enum, or identifier changes.

## Migration Notes

- Required consumer change: none.
- Safe rollout order: publish the package, then adopt it in consumers normally.
- Deprecated member removal target: none.

## Validation

- Local test command: `dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj --configuration Release --filter FullyQualifiedName~TimeStampAndObservableFileMonitorTests`.
- Package, advisory, and downstream validation: recorded on the linked pull request.
- Evidence link or issue comment: ChannelCheevos issue #1486.

## Version Classification

- Patch.
- Reason: fixes concurrent mutation and watcher lifetime without removing a public member.
