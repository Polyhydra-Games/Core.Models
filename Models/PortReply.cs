using System;
using System.Collections.Generic;
using System.Linq;

namespace PolyhydraGames.Core.Models;

public class PortReply : PortReply<string>
{
}

public class PortReply<T>
{
    public string Method { get; set; } = string.Empty;
    public Dictionary<string, string> Headers { get; set; } = new();
    public Dictionary<string, string> Query { get; set; } = new();
    public T? Content { get; set; }

    public override string ToString()
    {
        var headerNames = Headers?.Keys.OrderBy(key => key, StringComparer.OrdinalIgnoreCase) ?? Enumerable.Empty<string>();
        var queryNames = Query?.Keys.OrderBy(key => key, StringComparer.OrdinalIgnoreCase) ?? Enumerable.Empty<string>();

        return $"Method: {Method} \nHeaders: {string.Join(", ", headerNames)} \nQuery: {string.Join(", ", queryNames)} ";
    }

    public T? Data { get; set; }
}
