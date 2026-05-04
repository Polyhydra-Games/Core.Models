using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Core.Models.Identifiers;

/// <summary>
/// JSON converter for optional string-backed identifiers where JSON null and whitespace mean an empty identifier.
/// </summary>
/// <typeparam name="TId">The strongly typed optional string identifier struct.</typeparam>
public sealed class StronglyTypedOptionalStringConverter<TId> : JsonConverter<TId>
    where TId : struct
{
    /// <inheritdoc />
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return StronglyTypedIdFactory.CreateForJson<TId, string?>(string.Empty);

        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"{typeof(TId).Name} must be represented as a JSON string or null.");

        return StronglyTypedIdFactory.CreateForJson<TId, string?>(StringIdHelpers.NormalizeOptional(reader.GetString()));
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

