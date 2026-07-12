using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PolyhydraGames.Extensions;

namespace PolyhydraGames.Core.Models;

public static class PortReplyExtensions
{
    private const string RedactedHeaderValue = "[REDACTED]";

    public static Task<PortReply> ToPortReply(this HttpListenerRequest context)
    {
        try
        {
            var reply = new PortReply
            {
                Method = context.Method(),
                Headers = CaptureHeaders(context.Headers),
                Query = context.QueryString.ToDictionary(),
            };

            return Task.FromResult(reply);
        }
        catch (System.Exception exception)
        {
            return Task.FromException<PortReply>(exception);
        }
    }

    public static PortReply ToPortReply(this string context, NameValueCollection? headers = null)
    {
        var parts = context.Split("?");
        return new PortReply
        {
            Method = parts[0][1..],
            Headers = CaptureHeaders(headers),
            Query = parts.Length == 2 ? parts[1].ToDictionary() : new()
        };
    }

    public static string Method(this HttpListenerRequest context)
    {
        return context?.RawUrl?.Split("?")[0][1..] ?? "";
    }

    public static Task<PortReply<T>> ToPortReply<T>(this HttpListenerRequest context)
    {
        try
        {
            var reply = new PortReply<T>
            {
                Method = context.Method(),
                Headers = CaptureHeaders(context.Headers),
                Query = context.QueryString.ToDictionary()
            };

            return Task.FromResult(reply);
        }
        catch (System.Exception exception)
        {
            return Task.FromException<PortReply<T>>(exception);
        }
    }

    private static Dictionary<string, string> CaptureHeaders(NameValueCollection? headers)
    {
        if (headers is null)
        {
            return new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
        }

        return headers.AllKeys
            .Where(key => !string.IsNullOrWhiteSpace(key))
            .ToDictionary(
                key => key!,
                key => IsSensitiveHeader(key!) ? RedactedHeaderValue : headers[key] ?? string.Empty,
                System.StringComparer.OrdinalIgnoreCase);
    }

    private static bool IsSensitiveHeader(string name)
    {
        var normalizedName = name.Replace("-", string.Empty).Replace("_", string.Empty).ToUpperInvariant();

        return normalizedName.Contains("AUTHORIZATION", System.StringComparison.Ordinal) ||
               normalizedName.Contains("COOKIE", System.StringComparison.Ordinal) ||
               normalizedName.Contains("APIKEY", System.StringComparison.Ordinal) ||
               normalizedName.Contains("TOKEN", System.StringComparison.Ordinal) ||
               normalizedName.Contains("SECRET", System.StringComparison.Ordinal) ||
               normalizedName.Contains("PASSWORD", System.StringComparison.Ordinal);
    }
}
