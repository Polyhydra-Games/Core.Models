using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Portable reference to an asset that may be local, CDN-backed, provider-backed, or otherwise external.
/// </summary>
public sealed record MediaAssetReference
{
    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? AssetId { get; init; }

    /// <summary>
    /// Optional display label for UI surfaces.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Optional human-readable description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Asset classification used by shared consumers.
    /// </summary>
    public MediaAssetReferenceKind Kind { get; init; } = MediaAssetReferenceKind.External;

    /// <summary>
    /// Optional role label used by the owning app or importer.
    /// </summary>
    public string? Role { get; init; }

    /// <summary>
    /// Optional portable local media contract when the asset is backed by a local/NAS file.
    /// </summary>
    public LocalMediaFile? LocalFile { get; init; }

    /// <summary>
    /// Optional storage location when the reference can be expressed without embedding the full local file contract.
    /// </summary>
    public MediaFileLocation? Location { get; init; }

    /// <summary>
    /// Optional externally reachable URL.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// Optional source identifier from the owning application or provider.
    /// </summary>
    public string? SourceId { get; init; }

    /// <summary>
    /// Optional source name for display or traceability.
    /// </summary>
    public string? SourceName { get; init; }

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
