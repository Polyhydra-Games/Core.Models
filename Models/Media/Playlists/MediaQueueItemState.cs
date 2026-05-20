namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Broad lifecycle state for an item inside a playback queue.
/// </summary>
public enum MediaQueueItemState
{
    Unknown = 0,
    Queued = 1,
    Ready = 2,
    Playing = 3,
    Completed = 4,
    Skipped = 5,
    Failed = 6,
    Deferred = 7
}
