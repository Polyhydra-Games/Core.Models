using System;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Playback behavior shared by playlist consumers.
/// </summary>
public sealed record MediaPlaylistPlaybackOptions
{
    /// <summary>
    /// Whether playback should return to the first item after the final item.
    /// </summary>
    public bool Loop { get; init; } = true;

    /// <summary>
    /// Whether consumers may shuffle the item order.
    /// </summary>
    public bool Shuffle { get; init; }

    /// <summary>
    /// Whether consumers may choose items randomly instead of sequentially.
    /// </summary>
    public bool RandomOrder { get; init; }

    /// <summary>
    /// Optional default duration for segment-style playback.
    /// </summary>
    public TimeSpan? ClipLength { get; init; }

    /// <summary>
    /// Optional crossfade duration between items.
    /// </summary>
    public TimeSpan? CrossfadeDuration { get; init; }

    /// <summary>
    /// Optional pause between items.
    /// </summary>
    public TimeSpan? PauseBetweenItems { get; init; }

    /// <summary>
    /// Optional item index where playback should start.
    /// </summary>
    public int? StartIndex { get; init; }
}
