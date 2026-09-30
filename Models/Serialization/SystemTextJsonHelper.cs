using System.Text.Json;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models.Serialization;

using System;

public class SystemTextJsonHelper : ISerializationProvider
{
    public SystemTextJsonHelper() : this(new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) { }

    public SystemTextJsonHelper(JsonSerializerOptions options)
    {
        Options = options;
    }
    public JsonSerializerOptions Options { get; }

    public T DeserializeObject<T>(string value, bool continueOnError = true)
    {
        return JsonSerializer.Deserialize<T>(value, Options)!;
    }

    public object DeserializeObject(string value, Type type, bool continueOnError = true) =>
        JsonSerializer.Deserialize(value, type, Options)!;

    public string SerializeObject(object value) => JsonSerializer.Serialize(value);

    public (bool Success, string? Value) TrySerializeObject(object value)
    {
        try
        {
            return (true, SerializeObject(value));
        }
        catch
        {
            return (false, null);
        }
    }
}
