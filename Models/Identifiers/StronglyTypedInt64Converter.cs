using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Core.Models.Identifiers;

/// <summary>
/// JSON converter for strongly typed identifiers backed by a 64-bit integer.
/// </summary>
/// <typeparam name="TId">The strongly typed identifier struct.</typeparam>
public sealed class StronglyTypedInt64Converter<TId> : JsonConverter<TId>
    where TId : struct
{
    /// <inheritdoc />
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        long value;

        if (reader.TokenType == JsonTokenType.Number)
        {
            if (!reader.TryGetInt64(out value))
                throw new JsonException($"{typeof(TId).Name} must fit in a 64-bit integer.");
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            var text = reader.GetString();
            if (!long.TryParse(text, out value))
                throw new JsonException($"Invalid {typeof(TId).Name}: '{text}' is not a valid long integer.");
        }
        else
        {
            throw new JsonException($"{typeof(TId).Name} must be represented as a JSON number or integer string.");
        }

        return StronglyTypedIdFactory.CreateForJson<TId, long>(value);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(StronglyTypedIdFactory.GetInt64Value(value));
    }
}
