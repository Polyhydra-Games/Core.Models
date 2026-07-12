using System;
using System.Diagnostics;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models;

public static class CSVExtensions
{
    public static ISerializationProvider? Provider { get; set; }
    public static object FromCSV(this string data, Type type, bool continueOnError = true)
    {
        if (Provider == null) throw new NullReferenceException("ISerialization provider was not set. Please initialize this before use");
        try
        {
            return string.IsNullOrEmpty(data)
                ? Activator.CreateInstance(type)!
                : Provider.DeserializeObject(data, type, continueOnError);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Activator.CreateInstance(type)!;
        }
    }
    public static T FromCSV<T>(this string value, bool continueOnError = true)
    {
        if (Provider == null) throw new NullReferenceException("ISerialization provider was not set. Please initialize this before use");
        return string.IsNullOrEmpty(value) ? default! : Provider.DeserializeObject<T>(value, continueOnError);
    }
    public static string ToCSV(this object value)
    {
        if (Provider == null) throw new NullReferenceException("ISerialization provider was not set. Please initialize this before use");
        return Provider.SerializeObject(value);
    }
}
