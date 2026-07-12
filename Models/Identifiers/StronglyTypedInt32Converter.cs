using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Core.Models.Identifiers;

/// <summary>
/// JSON converter for strongly typed identifiers backed by a 32-bit integer.
/// </summary>
/// <typeparam name="TId">The strongly typed identifier struct.</typeparam>
public sealed class StronglyTypedInt32Converter<TId> : JsonConverter<TId>
    where TId : struct
{
    /// <inheritdoc />
    public override TId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int value;

        if (reader.TokenType == JsonTokenType.Number)
        {
            if (!reader.TryGetInt32(out value))
                throw new JsonException($"{typeof(TId).Name} must fit in a 32-bit integer.");
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            var text = reader.GetString();
            if (!int.TryParse(text, out value))
                throw new JsonException($"Invalid {typeof(TId).Name}: '{text}' is not a valid integer.");
        }
        else
        {
            throw new JsonException($"{typeof(TId).Name} must be represented as a JSON number or integer string.");
        }

        return StronglyTypedIdFactory.CreateForJson<TId, int>(value);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TId value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(StronglyTypedIdFactory.GetInt32Value(value));
    }
}
