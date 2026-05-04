using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Core.Models.Identifiers;

/// <summary>
/// JSON converter factory for structs implementing <see cref="IStronglyTypedId{TValue}" />.
/// </summary>
public sealed class StronglyTypedIdJsonConverterFactory : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsValueType && GetBackingType(typeToConvert) is not null;
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var backingType = GetBackingType(typeToConvert);

        if (backingType == typeof(Guid))
            return CreateConverter(typeof(StronglyTypedGuidConverter<>), typeToConvert);

        if (typeof(IOptionalStringId).IsAssignableFrom(typeToConvert))
            return CreateConverter(typeof(StronglyTypedOptionalStringConverter<>), typeToConvert);

        if (backingType == typeof(string))
            return CreateConverter(typeof(StronglyTypedStringConverter<>), typeToConvert);

        if (backingType == typeof(int))
            return CreateConverter(typeof(StronglyTypedInt32Converter<>), typeToConvert);

        if (backingType == typeof(long))
            return CreateConverter(typeof(StronglyTypedInt64Converter<>), typeToConvert);

        throw new NotSupportedException($"{typeToConvert.Name} uses unsupported strongly typed ID backing type {backingType?.Name}.");
    }

    private static JsonConverter CreateConverter(Type converterType, Type idType)
    {
        return (JsonConverter)Activator.CreateInstance(converterType.MakeGenericType(idType))!;
    }

    private static Type? GetBackingType(Type typeToConvert)
    {
        return typeToConvert
            .GetInterfaces()
            .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IStronglyTypedId<>))
            .Select(x => x.GetGenericArguments()[0])
            .SingleOrDefault();
    }
}
