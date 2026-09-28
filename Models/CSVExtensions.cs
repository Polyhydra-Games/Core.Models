using System;
using System.Diagnostics;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models;

public static class CSVExtensions
{
    private static ISerializationProvider? _provider;
    private static readonly object ProviderLock = new();

    public static ISerializationProvider? Provider
    {
        get
        {
            lock (ProviderLock) return _provider;
        }
        set
        {
            lock (ProviderLock) _provider = value;
        }
    }

    public static object? FromCSV(this string data, Type type, bool continueOnError = true)
    {
        var provider = GetProvider();
        try
        {
            return string.IsNullOrEmpty(data)
                ? Activator.CreateInstance(type)
                : provider.DeserializeObject(data, type, continueOnError);
        }
        catch (Exception ex)
        {
            if (!continueOnError) throw;
            Debug.WriteLine(ex.Message);
            return Activator.CreateInstance(type);
        }
    }
    public static T? FromCSV<T>(this string value, bool continueOnError = true)
    {
        var provider = GetProvider();
        if (string.IsNullOrEmpty(value))
        {
            return default;
        }
        return provider.DeserializeObject<T>(value, continueOnError);
    }
    public static string ToCSV(this object value)
    {
        return GetProvider().SerializeObject(value);
    }

    private static ISerializationProvider GetProvider() => Provider
        ?? throw new NullReferenceException("ISerialization provider was not set. Please initialize this before use");

}
