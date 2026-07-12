using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Api;

/// <summary>
/// Portable validation issue entry shared across request and readiness models.
/// </summary>
public sealed record ApiValidationIssue
{
    /// <summary>
    /// Optional field or path associated with the issue.
    /// </summary>
    public string? Field { get; init; }

    /// <summary>
    /// Optional stable machine-readable code.
    /// </summary>
    public string? Code { get; init; }

    /// <summary>
    /// Human-readable explanation.
    /// </summary>
    public string Message { get; init; } = "";

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
