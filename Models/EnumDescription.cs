#nullable enable
using System;

namespace PolyhydraGames.Core.Models;

public record EnumDescription<T>(T Value, string Title, string Description) : IEnumDescription
    where T : struct, Enum;