using System;

namespace PolyhydraGames.Core.Models.Identifiers;

/// <summary>
/// Factory and parsing helpers for strongly typed identifier structs.
/// </summary>
public static class StronglyTypedId
{
    /// <summary>
    /// Creates a strongly typed identifier from its primitive backing value.
    /// </summary>
    public static TId Create<TId, TValue>(TValue value)
        where TId : struct
    {
        return StronglyTypedIdFactory.Create<TId, TValue>(value);
    }

    /// <summary>
    /// Creates a GUID-backed strongly typed identifier with a new GUID value.
    /// </summary>
    public static TId NewGuid<TId>()
        where TId : struct
    {
        return Create<TId, Guid>(Guid.NewGuid());
    }

    /// <summary>
    /// Parses a GUID-backed strongly typed identifier from a string.
    /// </summary>
    public static TId ParseGuid<TId>(string? value)
        where TId : struct
    {
        if (!Guid.TryParse(value, out var guid))
            throw new FormatException($"Invalid {typeof(TId).Name}: '{value}' is not a valid Guid.");

        return Create<TId, Guid>(guid);
    }

    /// <summary>
    /// Attempts to parse a GUID-backed strongly typed identifier from a string.
    /// </summary>
    public static bool TryParseGuid<TId>(string? value, out TId id)
        where TId : struct
    {
        id = default;

        if (!Guid.TryParse(value, out var guid))
            return false;

        try
        {
            id = Create<TId, Guid>(guid);
            return true;
        }
        catch
        {
            id = default;
            return false;
        }
    }

    /// <summary>
    /// Creates a string-backed strongly typed identifier.
    /// </summary>
    public static TId FromString<TId>(string? value)
        where TId : struct
    {
        return Create<TId, string?>(value);
    }

    /// <summary>
    /// Attempts to create a string-backed strongly typed identifier.
    /// </summary>
    public static bool TryFromString<TId>(string? value, out TId id)
        where TId : struct
    {
        id = default;

        try
        {
            id = FromString<TId>(value);
            return true;
        }
        catch
        {
            id = default;
            return false;
        }
    }

    /// <summary>
    /// Parses a 32-bit integer-backed strongly typed identifier from a string.
    /// </summary>
    public static TId ParseInt32<TId>(string? value)
        where TId : struct
    {
        if (!int.TryParse(value, out var parsed))
            throw new FormatException($"Invalid {typeof(TId).Name}: '{value}' is not a valid integer.");

        return Create<TId, int>(parsed);
    }

    /// <summary>
    /// Attempts to parse a 32-bit integer-backed strongly typed identifier from a string.
    /// </summary>
    public static bool TryParseInt32<TId>(string? value, out TId id)
        where TId : struct
    {
        id = default;

        if (!int.TryParse(value, out var parsed))
            return false;

        try
        {
            id = Create<TId, int>(parsed);
            return true;
        }
        catch
        {
            id = default;
            return false;
        }
    }

    /// <summary>
    /// Parses a 64-bit integer-backed strongly typed identifier from a string.
    /// </summary>
    public static TId ParseInt64<TId>(string? value)
        where TId : struct
    {
        if (!long.TryParse(value, out var parsed))
            throw new FormatException($"Invalid {typeof(TId).Name}: '{value}' is not a valid long integer.");

        return Create<TId, long>(parsed);
    }

    /// <summary>
    /// Attempts to parse a 64-bit integer-backed strongly typed identifier from a string.
    /// </summary>
    public static bool TryParseInt64<TId>(string? value, out TId id)
        where TId : struct
    {
        id = default;

        if (!long.TryParse(value, out var parsed))
            return false;

        try
        {
            id = Create<TId, long>(parsed);
            return true;
        }
        catch
        {
            id = default;
            return false;
        }
    }
}
