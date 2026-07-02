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
        var headers = Headers.Any() ? Headers.Select(x => x.Key + ":" + x.Value).Aggregate((x, y) => x + ", " + y) : string.Empty;
        var query = Query.Any() ? Query.Select(x => x.Key + ":" + x.Value).Aggregate((x, y) => x + ", " + y) : string.Empty;
        return $"Method: {Method} \nHeaders: {headers} \nQuery: {query} ";
    }
    public T? Data { get; set; }


}
