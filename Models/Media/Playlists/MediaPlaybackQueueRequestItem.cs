using System;
using System.Collections.Generic;
using PolyhydraGames.Core.Models.Media;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Portable request item for adding media into a playback queue.
/// </summary>
public sealed record MediaPlaybackQueueRequestItem
{
    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? QueueRequestItemId { get; init; }

    /// <summary>
    /// Optional portable playlist item to add to the queue.
    /// </summary>
    public MediaPlaylistItem? PlaylistItem { get; init; }

    /// <summary>
    /// Optional embedded media file contract when queueing a raw file directly.
    /// </summary>
    public LocalMediaFile? Media { get; init; }

    /// <summary>
    /// Optional media identifier when the full media object is not embedded.
    /// </summary>
    public string? MediaFileId { get; init; }

    /// <summary>
    /// Optional slash-normalized path relative to the owning source.
    /// </summary>
    public string? RelativePath { get; init; }

    /// <summary>
    /// Optional absolute or mounted path for local execution.
    /// </summary>
    public string? FullPath { get; init; }

    /// <summary>
    /// Optional queue position hint.
    /// </summary>
    public int? QueuePosition { get; init; }

    /// <summary>
    /// Optional starting offset for segment-based queueing.
    /// </summary>
    public TimeSpan? StartOffset { get; init; }

    /// <summary>
    /// Optional duration for segment-based queueing.
    /// </summary>
    public TimeSpan? Duration { get; init; }

    /// <summary>
    /// Optional weighted-selection value for queue builders.
    /// </summary>
    public int? Weight { get; init; }

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
