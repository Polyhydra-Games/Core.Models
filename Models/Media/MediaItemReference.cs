using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Portable cross-system reference to a media item.
/// </summary>
public sealed record MediaItemReference
{
    /// <summary>
    /// Optional stable reference identifier assigned by the caller or owning workflow.
    /// </summary>
    public string? ReferenceId { get; init; }

    /// <summary>
    /// Stable storage identifier supplied by the storage owner, such as core.nas.
    /// </summary>
    public string? StorageId { get; init; }

    /// <summary>
    /// Optional content hash used for dedupe or import correlation.
    /// </summary>
    public string? ContentHash { get; init; }

    /// <summary>
    /// Optional CDN or served-asset identifier supplied by the delivery owner.
    /// </summary>
    public string? CdnId { get; init; }

    /// <summary>
    /// Optional public or CDN URL supplied by the delivery owner.
    /// </summary>
    public string? CdnUrl { get; init; }

    /// <summary>
    /// Broad media category used by shared consumers.
    /// </summary>
    public MediaKind Kind { get; init; } = MediaKind.Unknown;

    /// <summary>
    /// Optional display title suitable for lightweight UI surfaces.
    /// </summary>
    public string? DisplayTitle { get; init; }

    /// <summary>
    /// Optional secondary display text, such as artist, episode, source, or provenance text.
    /// </summary>
    public string? DisplaySubtitle { get; init; }

    /// <summary>
    /// Optional provider/catalog references attached by metadata or provider adapters.
    /// </summary>
    public IReadOnlyList<MediaProviderReference> Providers { get; init; } = [];

    /// <summary>
    /// Optional assets associated with the item, such as public media, poster art, or generated derivatives.
    /// </summary>
    public IReadOnlyList<MediaAssetReference> Assets { get; init; } = [];

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
