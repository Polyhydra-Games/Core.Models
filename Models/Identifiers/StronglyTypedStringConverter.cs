using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Core.Models.Identifiers;

/// <summary>
/// JSON converter for strongly typed identifiers backed by a string.
/// </summary>
/// <typeparam name="TId">The strongly typed identifier struct.</typeparam>
public sealed class StronglyTypedStringConverter<TId> : JsonConverter<TId>
    where TId : struct
{
    /// <inheritdoc />
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
            throw new JsonException($"{typeof(TId).Name} must be represented as a JSON string.");

        return StronglyTypedIdFactory.CreateForJson<TId, string?>(reader.GetString());
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
