using System.Linq;
using PolyhydraGames.Core.Models.Attributes;

namespace PolyhydraGames.Core.Models;

public static class ContentTypeExtensions
{
    public static string ToContentTypeString(this ContentType contentType)
    {
        var stringValueAttribute = contentType.GetType()
            .GetField(contentType.ToString())
            ?.GetCustomAttributes(typeof(StringValueAttribute), false)
            .FirstOrDefault() as StringValueAttribute;

        return stringValueAttribute?.Value ?? contentType.ToString();
    }
}
