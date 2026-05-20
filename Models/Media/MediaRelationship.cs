using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Portable directional relationship between two media-adjacent entities.
/// </summary>
public sealed record MediaRelationship
{
    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? RelationshipId { get; init; }

    /// <summary>
    /// Broad relationship label used by shared consumers.
    /// </summary>
    public MediaRelationshipKind Kind { get; init; } = MediaRelationshipKind.Related;

    /// <summary>
    /// Optional app-specific role or qualifier for the relationship.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// Relationship origin endpoint.
    /// </summary>
    public MediaRelationshipEndpoint Source { get; init; } = new();

    /// <summary>
    /// Relationship destination endpoint.
    /// </summary>
    public MediaRelationshipEndpoint Target { get; init; } = new();

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
