# Core.Models 2.0.1 hardening

This patch release hardens existing utility behavior without changing method or property signatures.

- Type-based JSON, CSV, and XML deserialization now rethrows failures when `continueOnError` is `false`.
- Resetting a delayed timer cancels and disposes its pending delay.
- Analytics `TryRunAction` methods log individual service failures, continue to remaining services, and return `false` if any service failed. Service enumeration uses a synchronized snapshot.
- Async observable operators send failures downstream by default. Passing `onError` handles and drops failures.
- CSV serialization provider access is synchronized.
- Deserialization helpers now declare nullable results where serializers can return `null`, while preserving their existing null behavior.

Validation: `dotnet build PolyhydraGames.Core.Models.sln --configuration Release` and `dotnet test Tests/PolyhydraGames.Core.Models.Test.csproj --configuration Release`.
