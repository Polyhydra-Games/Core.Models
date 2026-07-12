# Identifiers

`IStronglyTypedId<T>` identifies a value object backed by `Guid`, `string`, nullable `string`, `int`, or `long`. `StronglyTypedId` provides creation and parse helpers for these backing types.

The dedicated converters serialize GUID and string-backed identifiers as JSON strings and integer-backed identifiers as JSON numbers. `StronglyTypedIdJsonConverterFactory` can register support for all supported backing types in one `JsonSerializerOptions` instance.

Identifier implementations define their own invalid-value policy. The library's test examples reject empty GUIDs and zero numeric identifiers; consumers may choose different domain rules.
