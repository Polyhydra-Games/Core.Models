using System;
using System.Collections.Generic;
using System.Linq;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Pure helpers for portable playlist contracts.
/// </summary>
public static class MediaPlaylistHelpers
{
    /// <summary>
    /// Validates the basic portable playlist shape without checking filesystem availability.
    /// </summary>
    public static MediaPlaylistValidationResult Validate(MediaPlaylist? playlist)
    {
        var errors = new List<string>();

        if (playlist is null)
        {
            errors.Add("Playlist is required.");
            return new MediaPlaylistValidationResult { Errors = errors };
        }

        if (string.IsNullOrWhiteSpace(playlist.Name))
        {
            errors.Add("Playlist name is required.");
        }

        if (playlist.Items.Count == 0)
        {
            errors.Add("Playlist must contain at least one item.");
        }

        if (playlist.Playback.StartIndex.HasValue &&
            (playlist.Playback.StartIndex.Value < 0 || playlist.Playback.StartIndex.Value >= playlist.Items.Count))
        {
            errors.Add("Playback start index must point to an existing playlist item.");
        }

        for (var i = 0; i < playlist.Items.Count; i++)
        {
            var item = playlist.Items[i];
            var hasTitle = !string.IsNullOrWhiteSpace(item.Title);
            var hasMedia = item.Media is not null;
            var hasPath = !string.IsNullOrWhiteSpace(item.RelativePath) || !string.IsNullOrWhiteSpace(item.FullPath);
            var hasMediaId = !string.IsNullOrWhiteSpace(item.MediaFileId);

            if (!hasTitle)
            {
                errors.Add($"Playlist item {i} must have a title.");
            }

            if (!hasMedia && !hasPath && !hasMediaId)
            {
                errors.Add($"Playlist item {i} must reference media, a media id, or a path.");
            }

            if (item.Weight.HasValue && item.Weight.Value <= 0)
            {
                errors.Add($"Playlist item {i} weight must be greater than zero.");
            }

            if (item.Duration.HasValue && item.Duration.Value <= TimeSpan.Zero)
            {
                errors.Add($"Playlist item {i} duration must be greater than zero.");
            }

            if (item.StartOffset.HasValue && item.StartOffset.Value < TimeSpan.Zero)
            {
                errors.Add($"Playlist item {i} start offset cannot be negative.");
            }
        }

        return new MediaPlaylistValidationResult { Errors = errors };
    }

    /// <summary>
    /// Returns the item title formatted for now-playing surfaces.
    /// </summary>
    public static string GetNowPlayingText(MediaPlaylistItem item)
    {
        if (!string.IsNullOrWhiteSpace(item.NowPlayingText))
        {
            return item.NowPlayingText;
        }

        if (!string.IsNullOrWhiteSpace(item.Title))
        {
            return item.Title;
        }

        return item.Media?.FileName ?? item.RelativePath ?? item.FullPath ?? "";
    }

    /// <summary>
    /// Returns playlist items that have enough information for a consumer to try playback.
    /// </summary>
    public static IReadOnlyList<MediaPlaylistItem> GetPlayableItems(MediaPlaylist playlist)
    {
        return playlist.Items
            .Where(item =>
                item.Media is not null ||
                !string.IsNullOrWhiteSpace(item.MediaFileId) ||
                !string.IsNullOrWhiteSpace(item.RelativePath) ||
                !string.IsNullOrWhiteSpace(item.FullPath))
            .ToList();
    }
}
