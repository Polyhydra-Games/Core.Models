using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace PolyhydraGames.Core.Models.Identifiers;

internal static class StronglyTypedIdFactory
{
    public static TId Create<TId, TValue>(TValue value)
        where TId : struct
    {
        try
        {
            return (TId)Activator.CreateInstance(typeof(TId), value!)!;
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }
    }

    public static TId CreateForJson<TId, TValue>(TValue value)
        where TId : struct
    {
        try
        {
            return Create<TId, TValue>(value);
        }
        catch (MissingMethodException ex)
        {
            throw new System.Text.Json.JsonException($"{typeof(TId).Name} must expose a constructor that accepts {typeof(TValue).Name}.", ex);
        }
        catch (Exception ex) when (ex is not System.Text.Json.JsonException)
        {
            throw new System.Text.Json.JsonException(ex.Message, ex);
        }
    }

    public static int GetInt32Value<TId>(TId value)
        where TId : struct
    {
        if (value is IStronglyTypedId<int> typed)
            return typed.Value;

        if (int.TryParse(value.ToString(), out var parsed))
            return parsed;

        throw new System.Text.Json.JsonException($"{typeof(TId).Name} must expose an int Value or return an int from ToString().");
    }

    public static long GetInt64Value<TId>(TId value)
        where TId : struct
    {
        if (value is IStronglyTypedId<long> typed)
            return typed.Value;

        if (long.TryParse(value.ToString(), out var parsed))
            return parsed;

        throw new System.Text.Json.JsonException($"{typeof(TId).Name} must expose a long Value or return a long from ToString().");
    }
}
