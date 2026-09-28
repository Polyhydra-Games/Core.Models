using System;
using System.Diagnostics;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Models.Serialization;

namespace PolyhydraGames.Core.Models;

public static class JsonHelper
{
    public static ISerializationProvider Provider { get; } = new SystemTextJsonHelper();

    public static object? FromJson(this string? data, Type type, bool continueOnError = true)
    {
        try
        {
            return string.IsNullOrEmpty(data)
                ? Activator.CreateInstance(type)
                : Provider.DeserializeObject(data, type, continueOnError);
        }
        catch (Exception ex)
        {
            if (!continueOnError) throw;
            Debug.WriteLine(ex.Message);
            return Activator.CreateInstance(type);
        }
    }
    public static T? FromJson<T>(this string? value, bool continueOnError = true)
    {
        if (Provider == null) throw new NullReferenceException("ISerialization provider was not set. Please initialize this before use");
        if (string.IsNullOrEmpty(value))
            return CreateDefault<T>();

        try
        {
            return Provider.DeserializeObject<T>(value, continueOnError);
        }
        catch (Exception ex)
        {
            if (!continueOnError) throw;
            Debug.WriteLine(ex.Message);
            return CreateDefault<T>();
        }
    }

    private static T? CreateDefault<T>()
    {
        var value = default(T);
        if (value == null)
        {
            return (T?)Activator.CreateInstance(typeof(T));
        }
        return value;
    }
    public static string ToJson(this object value)
    {
        if (Provider == null) throw new NullReferenceException("ISerialization provider was not set. Please initialize this before use");
        return Provider.SerializeObject(value);
    }
}
