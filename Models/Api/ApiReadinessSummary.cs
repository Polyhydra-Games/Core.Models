using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Api;

/// <summary>
/// Portable readiness summary for operator and health surfaces.
/// </summary>
public sealed record ApiReadinessSummary
{
    /// <summary>
    /// Overall readiness label.
    /// </summary>
    public string Status { get; init; } = "unknown";

    /// <summary>
    /// Convenience flag derived from the overall status in downstream callers.
    /// </summary>
    public bool Ready => string.Equals(Status, "ready", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(Status, "ok", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Optional human-readable summary.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Timestamp when readiness was last computed.
    /// </summary>
    public DateTimeOffset? CheckedAtUtc { get; init; }

    /// <summary>
    /// Individual readiness checks.
    /// </summary>
    public IReadOnlyList<ApiReadinessCheck> Checks { get; init; } = [];

    /// <summary>
    /// Non-fatal readiness alerts.
    /// </summary>
    public IReadOnlyList<string> Alerts { get; init; } = [];

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
