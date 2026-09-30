# Core.Models build standard (#41)

Adopts the Polyhydra .NET library build standard v1 (analyzers as errors in CI, Makefile, hang-safe
tests, Stryker ratchet). No method, property, or field signatures change.

Observable behavior changes:

- `DelayedTimer` concurrent-use guard now throws `InvalidOperationException` instead of a bare
  `Exception`. It is a subclass, so `catch (Exception)` still works; code that asserts the exact
  type must update.
- `SubjectObservable<T>` now declares `IDisposable`. Its existing `Dispose()` method is unchanged.
- `ObservableFileMonitor.Dispose()` calls `GC.SuppressFinalize`.
- `StringSetting` now states `StringComparison.Ordinal` explicitly (same comparison as before).

Unchanged by design: the JSON, CSV, and XML entry points still throw `NullReferenceException` when no
`ISerialization` provider is configured.
