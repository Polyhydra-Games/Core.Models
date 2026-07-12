using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Portable reference to an external media provider record for an asset or catalog item.
/// </summary>
public sealed record MediaProviderReference
{
    /// <summary>
    /// Stable provider key such as tmdb, tvmaze, omdb, or thetvdb.
    /// </summary>
    public string ProviderKey { get; init; } = string.Empty;

    /// <summary>
    /// External identifier within the provider namespace.
    /// </summary>
    public string ExternalId { get; init; } = string.Empty;

    /// <summary>
    /// Optional display name supplied by the provider or owning application.
    /// </summary>
    public string? DisplayName { get; init; }

    /// <summary>
    /// Optional provider URL or canonical page.
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// Optional timestamp when the reference was last refreshed or observed.
    /// </summary>
    public DateTimeOffset? LastSeenAt { get; init; }

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
