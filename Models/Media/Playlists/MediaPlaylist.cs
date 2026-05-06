using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Portable playlist or playout manifest shared across media, catalog, operator, and broadcast apps.
/// </summary>
public sealed record MediaPlaylist
{
    /// <summary>
    /// Contract version for serialized playlist manifests.
    /// </summary>
    public int Version { get; init; } = 1;

    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? PlaylistId { get; init; }

    /// <summary>
    /// Display name for the playlist or show.
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// Optional human-readable description.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Optional source collection identifier.
    /// </summary>
    public string? SourceCollectionId { get; init; }

    /// <summary>
    /// Optional source collection display name.
    /// </summary>
    public string? SourceCollectionName { get; init; }

    /// <summary>
    /// Optional source definition used to generate or resolve this playlist.
    /// </summary>
    public MediaPlaylistSource? Source { get; init; }

    /// <summary>
    /// Optional overlay or visual preset hint for consumers that support overlays.
    /// </summary>
    public string? OverlayPreset { get; init; }

    /// <summary>
    /// Playback options that describe ordering and item timing.
    /// </summary>
    public MediaPlaylistPlaybackOptions Playback { get; init; } = new();

    /// <summary>
    /// Playlist items in their intended order.
    /// </summary>
    public IReadOnlyList<MediaPlaylistItem> Items { get; init; } = Array.Empty<MediaPlaylistItem>();

    /// <summary>
    /// Timestamp when the playlist was generated or exported, when known.
    /// </summary>
    public DateTimeOffset? GeneratedAt { get; init; }

    /// <summary>
    /// Optional labels for broad grouping or filtering.
    /// </summary>
    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
