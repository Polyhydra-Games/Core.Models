---
title: Strongly Typed Identifiers Architecture
status: in-progress
owner: @DreadBreadcrumb
created: 2026-05-04
updated: 2026-05-30
tags: [feature, architecture, identifiers, Core.Models]
---

# Strongly Typed Identifiers Architecture

## Boundary

`Core.Models` owns reusable identifier contracts, converters, and helper methods. It does not own app-specific identity storage, OAuth flows, platform clients, token handling, database persistence, or routing logic.

The current migration boundary is intentionally limited to reusable base/converter patterns. Concrete IDs remain downstream until ownership is shared and consumer compatibility is proven.

## Namespace

```csharp
PolyhydraGames.Core.Models.Identifiers
```

## Contract Shape

- `IStronglyTypedId<TValue>` marks primitive-backed value-object IDs.
- `StronglyTypedId` provides creation and parse/try-parse helpers.
- `StronglyTypedGuidConverter<TId>` serializes GUID-backed IDs as JSON strings.
- `StronglyTypedStringConverter<TId>` serializes string-backed IDs as JSON strings.
- `StronglyTypedInt32Converter<TId>` serializes int-backed IDs as JSON numbers.
- `StronglyTypedInt64Converter<TId>` serializes long-backed IDs as JSON numbers.
- `StronglyTypedIdJsonConverterFactory` can be registered once in `JsonSerializerOptions` to cover supported backing types.

## Example

```csharp
using System.Diagnostics;
using System.Text.Json.Serialization;
using PolyhydraGames.Core.Models.Identifiers;

[DebuggerDisplay("{Value}")]
[JsonConverter(typeof(StronglyTypedGuidConverter<IdentityId>))]
public readonly struct IdentityId : IEquatable<IdentityId>, IStronglyTypedId<Guid>
{
    public IdentityId(Guid value)
    {
        if (value == Guid.Empty)
            throw new FormatException("Invalid IdentityId: cannot use an empty guid.");

        Value = value;
    }

    public Guid Value { get; }

    public static IdentityId New()
    {
        return StronglyTypedId.NewGuid<IdentityId>();
    }

    public override string ToString()
    {
        return Value.ToString("D");
    }

    public bool Equals(IdentityId other)
    {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is IdentityId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
```

## Empty Value Policy

Empty-value rules belong to the concrete ID type:

- [x] GUID-backed IDs that represent persisted identities should usually reject `Guid.Empty`.
- [x] GUID-backed keys that use empty as an unauthenticated or placeholder state may allow `Guid.Empty`.
- [x] String-backed platform IDs may normalize null or whitespace to empty when empty means "no ID".
- [x] String-backed IDs should throw when empty is not valid for the domain.
- [x] Int-backed and long-backed IDs should decide whether zero or negative values are valid.

The shared converters call the ID constructor and preserve those per-type rules.

## JSON Strategy

Per-type attribute:

```csharp
[JsonConverter(typeof(StronglyTypedGuidConverter<IdentityId>))]
```

Global factory registration:

```csharp
var options = new JsonSerializerOptions();
options.Converters.Add(new StronglyTypedIdJsonConverterFactory());
```

## Migration Rules

- [x] Move the reusable pattern first.
- [x] Migrate concrete IDs only when ownership is clear.
- [x] Keep downstream package compatibility until consumers have switched references.
- [x] Add downstream tests before deleting old ID implementations.
- [x] Prefer explicit casts from strings and primitives where accidental cross-type conversion would be risky.
- [x] Do not move `IdentityId`, `UserId`, `WebKey`, `TwitchId`, or `ChannelKey` in the first pass; downstream packages should wrap those IDs with the shared contracts/converters first.
