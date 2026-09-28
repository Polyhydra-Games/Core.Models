using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using PolyhydraGames.Core.Interfaces.Enum;

namespace PolyhydraGames.Core.Models.Serialization;

public static class WebHelpers
{
    /// <summary>
    /// Takes a URL and grabs a raw JSON file and converts it into a T object list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="client"></param>
    /// <param name="url"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static async Task<IEnumerable<T>?> GetItemsAsync<T>(this HttpClient client, string url, Serialize encoding)
    {
        try
        {
            var response = await client.GetStringAsync(url);
            switch (encoding)
            {
                case Serialize.Json: return response.FromJson<List<T>>();
                case Serialize.XML: return response.FromXML<List<T>>();
                case Serialize.CSV: return response.FromCSV<List<T>>();
                default: throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new List<T>();
        }

    }

    public static async Task<T?> GetItemAsync<T>(this HttpClient client, string url, Serialize encoding) where T : class
    {
        try
        {
            var response = await client.GetStringAsync(url);
            switch (encoding)
            {
                case Serialize.Json: return response.FromJson<T>();
                case Serialize.XML: return response.FromXML<T>();
                case Serialize.CSV: return response.FromCSV<T>();
                default: throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }

    }
}
