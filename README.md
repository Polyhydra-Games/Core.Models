# PolyhydraGames.Core.Models

`PolyhydraGames.Core.Models` is a .NET library of reusable contracts and small utilities for applications that exchange media, API, settings, and identifier data.

## Install

```bash
dotnet add package PolyhydraGames.Core.Models
```

The package targets .NET 8 and .NET 9. It is licensed under [MIT-0](LICENSE).

## What is included

- API result, validation, readiness, paging, sorting, and filtering contracts.
- Media, playlist, playback-queue, relationship, and scan-progress contracts.
- Strongly typed `Guid`, `string`, nullable-string, `int`, and `long` identifiers with `System.Text.Json` converters.
- Content-type helpers, settings models, cache and delay helpers, and Reactive Extensions-based observables.

## Quick example

Create a strongly typed identifier and serialize it as a JSON string:

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;
using PolyhydraGames.Core.Models.Identifiers;

[JsonConverter(typeof(StronglyTypedGuidConverter<OrderId>))]
public readonly struct OrderId : IEquatable<OrderId>, IStronglyTypedId<Guid>
{
    public OrderId(Guid value)
    {
        if (value == Guid.Empty)
            throw new FormatException("An order identifier cannot be empty.");

        Value = value;
    }

    public Guid Value { get; }
    public static implicit operator Guid(OrderId value) => value.Value;
    public bool Equals(OrderId other) => Value.Equals(other.Value);
    public override bool Equals(object? obj) => obj is OrderId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString("D");
}

var id = StronglyTypedId.NewGuid<OrderId>();
var json = JsonSerializer.Serialize(id); // JSON string
```

For APIs with several identifier backing types, add `StronglyTypedIdJsonConverterFactory` to `JsonSerializerOptions`; see [identifier guidance](docs/api-families/identifiers.md).

## Scope and non-goals

This project provides in-process models and helpers. It does not provide a database, transport protocol, web server, authentication system, deployment configuration, media scanner, or application-specific runtime. Public record properties are intended as interoperable data shapes; consumers own persistence, validation policy, and transport concerns.

## Documentation

- [Getting started](docs/getting-started.md)
- [Scope](docs/scope.md)
- [Compatibility](docs/compatibility.md)
- [API families](docs/api-families/README.md)

## Development

```bash
dotnet restore PolyhydraGames.Core.Models.sln
dotnet build PolyhydraGames.Core.Models.sln --configuration Release --no-restore
dotnet test PolyhydraGames.Core.Models.sln --configuration Release --no-build
dotnet pack Models/PolyhydraGames.Core.Models.csproj --configuration Release --no-build --output artifacts/packages
```

See [CONTRIBUTING.md](CONTRIBUTING.md) for contribution and validation expectations.
