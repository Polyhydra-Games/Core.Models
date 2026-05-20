using PolyhydraGames.Core.Models.Media;
using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Scanning;

/// <summary>
/// Portable summary for one indexed media file.
/// </summary>
public sealed record IndexedMediaFileSummary
{
    public string? MediaFileId { get; init; }
    public string FileName { get; init; } = "";
    public string? RootId { get; init; }
    public string? RelativePath { get; init; }
    public string? FullPath { get; init; }
    public MediaKind Kind { get; init; } = MediaKind.Unknown;
    public MediaAvailability Availability { get; init; } = MediaAvailability.Unknown;
    public DateTimeOffset? IndexedAtUtc { get; init; }
    public string? Checksum { get; init; }
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
