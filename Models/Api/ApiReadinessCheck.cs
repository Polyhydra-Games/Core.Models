using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Api;

/// <summary>
/// Portable readiness check entry.
/// </summary>
public sealed record ApiReadinessCheck
{
    /// <summary>
    /// Human-readable check name.
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// Whether the check passed.
    /// </summary>
    public bool Ok { get; init; }

    /// <summary>
    /// Optional status label for the check.
    /// </summary>
    public string? Status { get; init; }

    /// <summary>
    /// Optional human-readable detail.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
