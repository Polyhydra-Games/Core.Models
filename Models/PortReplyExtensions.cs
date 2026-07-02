using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading.Tasks;
using PolyhydraGames.Extensions;

namespace PolyhydraGames.Core.Models;

public static class PortReplyExtensions
{

    //public static PortReply ToPortReply(this HttpListenerRequest context)
    //{
    //    return context.RawUrl.ToPortReply(context.Headers);
    //}
    public static async Task<PortReply> ToPortReply(this HttpListenerRequest context)
    {
        try
        {
            var reply = new PortReply()
            {
                Method = context.Method(),
                Headers = context.Headers.ToDictionary(),
                Query = context.QueryString.ToDictionary(),
            };
            //if (context.Body != null)
            //{
            //    await using var content = context.Body;
            //    var reader = new StreamReader(content);
            //    var body = await reader.ReadToEndAsync();
            //    reply.Content = body;
            //}
            return reply;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    public static PortReply ToPortReply(this string context, NameValueCollection? headers = null)
    {
        try
        {
            var parts = context.Split("?");
            return new PortReply()
            {
                Method = parts[0][1..],
                Headers = headers?.ToDictionary() ?? new(),
                Query = parts.Length == 2 ? parts[1].ToDictionary() : new()
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        //}
        //}
        //string[] parts = context.Split("?");
        //return new PortReply()
        //{
        //    Method = parts[0][1..],
        //    Headers = headers?.ToDictionary() ?? new(),
        //    Query = parts.Length == 2 ? parts[1].ToDictionary() : new ()
        //};
    }

    public static string Method(this HttpListenerRequest context)
    {
        return context?.RawUrl?.Split("?")[0][1..] ?? "";
    }


    public static async Task<PortReply<T>> ToPortReply<T>(this HttpListenerRequest context)
    {
        var reply = new PortReply<T>()
        {
            Method = context.Method(),
            Headers = context.Headers.ToDictionary(),
            Query = context.QueryString.ToDictionary()
        };
        //if (context.Body != null)
        //{
        //    await using var content = context.Body;
        //    var reader = new StreamReader(content);
        //    var body = await reader.ReadToEndAsync();
        //    reply.Content = body.FromJson<T>();
        //}
        return reply;
    }

    //public static Dictionary<string, string> ToDictionary(this IHeaderDictionary dic)
    //{
    //    return dic.Keys.ToDictionary<string?, string, string>(key => key, key => dic[key]);
    //}

    //public static Dictionary<string, string> ToDictionary(this QueryString context)
    //{
    //    var raw = context.Value;
    //    return string.IsNullOrWhiteSpace(raw) ? new Dictionary<string, string>() : raw[1..].ToDictionary();
    //}

}
