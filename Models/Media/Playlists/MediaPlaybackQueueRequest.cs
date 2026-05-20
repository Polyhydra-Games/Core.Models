using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Portable request for assembling or extending a playback queue.
/// </summary>
public sealed record MediaPlaybackQueueRequest
{
    /// <summary>
    /// Optional stable identifier from the owning application.
    /// </summary>
    public string? QueueRequestId { get; init; }

    /// <summary>
    /// Optional queue identifier supplied by the owning application.
    /// </summary>
    public string? QueueId { get; init; }

    /// <summary>
    /// Display name for the queue request.
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// Optional source playlist identifier for the queued items.
    /// </summary>
    public string? SourcePlaylistId { get; init; }

    /// <summary>
    /// Optional source playlist display name.
    /// </summary>
    public string? SourcePlaylistName { get; init; }

    /// <summary>
    /// Optional requester label or user id.
    /// </summary>
    public string? RequestedBy { get; init; }

    /// <summary>
    /// Optional timestamp when the queue request was created.
    /// </summary>
    public DateTimeOffset? RequestedAtUtc { get; init; }

    /// <summary>
    /// Whether the requested items should append to the existing queue.
    /// </summary>
    public bool Append { get; init; } = true;

    /// <summary>
    /// Optional insertion index when appending is disabled.
    /// </summary>
    public int? InsertAtIndex { get; init; }

    /// <summary>
    /// Portable queue request items.
    /// </summary>
    public IReadOnlyList<MediaPlaybackQueueRequestItem> Items { get; init; } = Array.Empty<MediaPlaybackQueueRequestItem>();

    /// <summary>
    /// Optional string metadata for app-specific hints that should not become first-class contract fields yet.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}
