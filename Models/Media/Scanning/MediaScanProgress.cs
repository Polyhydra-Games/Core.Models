using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Scanning;

/// <summary>
/// Portable scan-progress snapshot for catalog or index jobs.
/// </summary>
public sealed record MediaScanProgress
{
    public string? ScanId { get; init; }
    public string? RootId { get; init; }
    public string? RootName { get; init; }
    public string? CurrentPath { get; init; }
    public long DiscoveredFiles { get; init; }
    public long ProcessedFiles { get; init; }
    public long IndexedFiles { get; init; }
    public long SkippedFiles { get; init; }
    public long FailedFiles { get; init; }
    public DateTimeOffset? StartedAtUtc { get; init; }
    public DateTimeOffset? UpdatedAtUtc { get; init; }
    public bool Complete { get; init; }
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
