using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Portable snapshot of a playback queue.
/// </summary>
public sealed record MediaPlaybackQueue
{
    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? QueueId { get; init; }

    /// <summary>
    /// Display name for the queue.
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// Optional source playlist identifier.
    /// </summary>
    public string? SourcePlaylistId { get; init; }

    /// <summary>
    /// Optional source playlist display name.
    /// </summary>
    public string? SourcePlaylistName { get; init; }

    /// <summary>
    /// Optional requester label or user id associated with the queue.
    /// </summary>
    public string? RequestedBy { get; init; }

    /// <summary>
    /// Optional timestamp when the queue was created.
    /// </summary>
    public DateTimeOffset? CreatedAtUtc { get; init; }

    /// <summary>
    /// Optional timestamp when the queue was last updated.
    /// </summary>
    public DateTimeOffset? UpdatedAtUtc { get; init; }

    /// <summary>
    /// Optional active queue item identifier.
    /// </summary>
    public string? ActiveQueueItemId { get; init; }

    /// <summary>
    /// Queue entries in the intended order.
    /// </summary>
    public IReadOnlyList<MediaPlaybackQueueItem> Items { get; init; } = Array.Empty<MediaPlaybackQueueItem>();

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
