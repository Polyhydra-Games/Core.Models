using System;
using System.Collections.Generic;
using System.Linq;

namespace PolyhydraGames.Core.Models;

public static class EnumDescriptionExtensions
{
    public static EnumDescription<T> ToDescription<T>(this T value, string title, string description)
        where T : struct, Enum =>
        new(value, title, description);

    public static List<EnumDescription<T>> ToDescriptions<T>(this T value, Func<T, string>? getTitle = null, Func<T, string>? getDescription = null) where T : struct, Enum
        => Enum.GetValues<T>().Select(item =>
            ToDescription<T>(item, getTitle?.Invoke(item) ?? item.ToString(),
                getDescription?.Invoke(item) ?? "")).ToList();

}
