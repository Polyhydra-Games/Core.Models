using System;
using System.Collections.Generic;
using PolyhydraGames.Core.Models.Media;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Portable reference to one playable media item inside a playlist.
/// </summary>
public sealed record MediaPlaylistItem
{
    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? PlaylistItemId { get; init; }

    /// <summary>
    /// Display title for UI and now-playing surfaces.
    /// </summary>
    public string Title { get; init; } = "";

    /// <summary>
    /// Optional portable media-file contract for this item.
    /// </summary>
    public LocalMediaFile? Media { get; init; }

    /// <summary>
    /// Optional media identifier when the full media object is not embedded.
    /// </summary>
    public string? MediaFileId { get; init; }

    /// <summary>
    /// Consumer-relative file path, such as a Busey-Box manifest path.
    /// </summary>
    public string? RelativePath { get; init; }

    /// <summary>
    /// Optional absolute or mounted path for local execution.
    /// </summary>
    public string? FullPath { get; init; }

    /// <summary>
    /// Optional source collection label.
    /// </summary>
    public string? SourceCollection { get; init; }

    /// <summary>
    /// Optional item-specific overlay or visual preset hint.
    /// </summary>
    public string? OverlayPreset { get; init; }

    /// <summary>
    /// Optional text to show in now-playing overlays. Consumers can fall back to Title.
    /// </summary>
    public string? NowPlayingText { get; init; }

    /// <summary>
    /// Optional starting point for segment playback.
    /// </summary>
    public TimeSpan? StartOffset { get; init; }

    /// <summary>
    /// Optional duration for segment playback.
    /// </summary>
    public TimeSpan? Duration { get; init; }

    /// <summary>
    /// Optional weighted-selection value for random playlist consumers.
    /// </summary>
    public int? Weight { get; init; }

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
