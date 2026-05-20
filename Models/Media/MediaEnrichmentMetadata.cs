using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Optional descriptive metadata used to enrich a local media file from provider or catalog sources.
/// </summary>
public sealed record MediaEnrichmentMetadata
{
    /// <summary>
    /// Preferred display title when a provider has a canonical name for the file.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Human-readable summary or synopsis.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Release year when available.
    /// </summary>
    public int? ReleaseYear { get; init; }

    /// <summary>
    /// Free-form genres string or provider-normalized genre list encoded by the caller.
    /// </summary>
    public string? Genres { get; init; }

    /// <summary>
    /// Runtime in minutes when available.
    /// </summary>
    public int? RuntimeMinutes { get; init; }

    /// <summary>
    /// Provider name supplying the enrichment.
    /// </summary>
    public string? Provider { get; init; }

    /// <summary>
    /// Provider key or external identifier used by the enrichment source.
    /// </summary>
    public string? ProviderKey { get; init; }

    /// <summary>
    /// Provider-specific external link when available.
    /// </summary>
    public string? ImdbLink { get; init; }

    /// <summary>
    /// Alternate provider-specific link when available.
    /// </summary>
    public string? TvMazeLink { get; init; }

    /// <summary>
    /// Poster art URL when available.
    /// </summary>
    public string? PosterUrl { get; init; }

    /// <summary>
    /// Backdrop art URL when available.
    /// </summary>
    public string? BackdropUrl { get; init; }

    /// <summary>
    /// Optional portable references to external provider records for the media item.
    /// </summary>
    public IReadOnlyList<MediaProviderReference> Providers { get; init; } = [];
}
