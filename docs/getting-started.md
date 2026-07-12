# Getting started

Install the package with `dotnet add package PolyhydraGames.Core.Models` and import only the namespaces your application uses.

The library is organized by API family. For example, `PolyhydraGames.Core.Models.Api` contains API result and paging contracts, `Media` contains media shapes, and `Identifiers` contains strongly typed identifier utilities.

For a generic JSON converter registration:

```csharp
using System.Text.Json;
using PolyhydraGames.Core.Models.Identifiers;

var options = new JsonSerializerOptions();
options.Converters.Add(new StronglyTypedIdJsonConverterFactory());
```

The converter factory supports identifier types implementing `IStronglyTypedId<T>` where `T` is `Guid`, `string`, `int`, or `long`. Use a specific converter when a type needs a dedicated JSON policy.
