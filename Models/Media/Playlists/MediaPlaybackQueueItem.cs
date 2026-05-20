using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Portable queue entry for a media item that has already been accepted into a queue.
/// </summary>
public sealed record MediaPlaybackQueueItem
{
    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? QueueItemId { get; init; }

    /// <summary>
    /// Optional portable playlist item backing the queue entry.
    /// </summary>
    public MediaPlaylistItem? PlaylistItem { get; init; }

    /// <summary>
    /// Current queue state for the item.
    /// </summary>
    public MediaQueueItemState State { get; init; } = MediaQueueItemState.Queued;

    /// <summary>
    /// Optional zero-based position in the queue.
    /// </summary>
    public int? QueuePosition { get; init; }

    /// <summary>
    /// Optional timestamp when the item was queued.
    /// </summary>
    public DateTimeOffset? QueuedAtUtc { get; init; }

    /// <summary>
    /// Optional timestamp when the item started playing.
    /// </summary>
    public DateTimeOffset? StartedAtUtc { get; init; }

    /// <summary>
    /// Optional timestamp when the item completed playback.
    /// </summary>
    public DateTimeOffset? CompletedAtUtc { get; init; }

    /// <summary>
    /// Optional error or skip message for failed or skipped entries.
    /// </summary>
    public string? Message { get; init; }

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
