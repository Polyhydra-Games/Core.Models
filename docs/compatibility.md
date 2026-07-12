# Compatibility

The library and its test project target .NET 8 and .NET 9. The test project is a development-only project and is not a package target.

`System.Text.Json` is used by the identifier converters and is exercised by the test suite. Consumers should treat contract property names, defaults, and serialization behavior as compatibility-sensitive.

The package depends on `PolyhydraGames.Core.Interfaces`, `PolyhydraGames.Extensions`, and `System.Reactive`; NuGet restores these dependencies from nuget.org through the included portable `NuGet.config`.
