using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Scanning;

/// <summary>
/// Portable scan summary for a completed or checkpointed scan job.
/// </summary>
public sealed record MediaScanSummary
{
    public string? ScanId { get; init; }
    public string? RootId { get; init; }
    public string? RootName { get; init; }
    public DateTimeOffset? StartedAtUtc { get; init; }
    public DateTimeOffset? CompletedAtUtc { get; init; }
    public long TotalFiles { get; init; }
    public long IndexedFiles { get; init; }
    public long SkippedFiles { get; init; }
    public long FailedFiles { get; init; }
    public IReadOnlyList<string> Warnings { get; init; } = [];
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
