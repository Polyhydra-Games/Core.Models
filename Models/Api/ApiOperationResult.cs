using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Api;

/// <summary>
/// Portable result envelope for simple success/failure operations.
/// </summary>
public sealed record ApiOperationResult
{
    /// <summary>
    /// True when the operation completed successfully.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// Optional human-readable summary.
    /// </summary>
    public string Message { get; init; } = "";

    /// <summary>
    /// Validation or domain errors produced by the operation.
    /// </summary>
    public IReadOnlyList<ApiValidationIssue> Errors { get; init; } = [];

    /// <summary>
    /// Non-fatal warnings.
    /// </summary>
    public IReadOnlyList<string> Warnings { get; init; } = [];

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
