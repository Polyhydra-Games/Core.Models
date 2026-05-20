using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Portable reference to one endpoint in a shared media relationship.
/// </summary>
public sealed record MediaRelationshipEndpoint
{
    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? EntityId { get; init; }

    /// <summary>
    /// Broad entity category for the endpoint.
    /// </summary>
    public MediaRelationshipEndpointKind Kind { get; init; } = MediaRelationshipEndpointKind.Unknown;

    /// <summary>
    /// Optional display label for the endpoint.
    /// </summary>
    public string? Label { get; init; }

    /// <summary>
    /// Optional portable relative path for file-backed endpoints.
    /// </summary>
    public string? RelativePath { get; init; }

    /// <summary>
    /// Optional absolute or mounted path for local execution.
    /// </summary>
    public string? FullPath { get; init; }

    /// <summary>
    /// Optional externally reachable URL.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
