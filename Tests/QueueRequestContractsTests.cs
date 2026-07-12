using System;
using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;
using PolyhydraGames.Core.Models.Media;
using PolyhydraGames.Core.Models.Media.Playlists;

namespace PolyhydraGames.Core.Models.Tests;

public class QueueRequestContractsTests
{
    [Test]
    public void MediaPlaybackQueueRequest_SerializesAndDeserializesSharedShape()
    {
        var request = new MediaPlaybackQueueRequest
        {
            QueueRequestId = "queue-request:001",
            QueueId = "queue:main",
            Name = "Friday Night Queue",
            SourcePlaylistId = "playlist:sample-show",
            SourcePlaylistName = "Sample Show",
            RequestedBy = "operator:alex",
            RequestedAtUtc = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
            Append = false,
            InsertAtIndex = 0,
            Items =
            [
                new MediaPlaybackQueueRequestItem
                {
                    QueueRequestItemId = "queue-item:1",
                    PlaylistItem = new MediaPlaylistItem
                    {
                        PlaylistItemId = "clip-1",
                        Title = "Sample Episode",
                        RelativePath = "video/01-killer-shrews.mp4",
                        StartOffset = TimeSpan.FromMinutes(10),
                        Duration = TimeSpan.FromSeconds(30)
                    },
                    Media = new LocalMediaFile
                    {
                        MediaFileId = "library:123",
                        FileName = "Sample Episode.mp4",
                        Location = new MediaFileLocation
                        {
                            RelativePath = "video/01-killer-shrews.mp4"
                        }
                    },
                    Weight = 2
                }
            ],
            Metadata = new Dictionary<string, string>
            {
                ["queue_type"] = "operator"
            }
        };

        var json = JsonSerializer.Serialize(request);
        var roundTrip = JsonSerializer.Deserialize<MediaPlaybackQueueRequest>(json);

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.Name, Is.EqualTo("Friday Night Queue"));
        Assert.That(() => roundTrip.Append, Is.False);
        Assert.That(() => roundTrip.Items, Has.Count.EqualTo(1));
        Assert.That(() => roundTrip.Items[0].PlaylistItem!.Title, Is.EqualTo("Sample Episode"));
        Assert.That(() => roundTrip.Items[0].Media!.MediaFileId, Is.EqualTo("library:123"));
        Assert.That(() => roundTrip.Metadata["queue_type"], Is.EqualTo("operator"));
    }

    [Test]
    public void MediaPlaybackQueue_SerializesAndDeserializesSharedShape()
    {
        var queue = new MediaPlaybackQueue
        {
            QueueId = "queue:main",
            Name = "Friday Night Queue",
            SourcePlaylistId = "playlist:sample-show",
            RequestedBy = "operator:alex",
            CreatedAtUtc = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
            UpdatedAtUtc = DateTimeOffset.Parse("2026-05-20T12:05:00Z"),
            ActiveQueueItemId = "queue-item:1",
            Items =
            [
                new MediaPlaybackQueueItem
                {
                    QueueItemId = "queue-item:1",
                    QueuePosition = 0,
                    State = MediaQueueItemState.Playing,
                    PlaylistItem = new MediaPlaylistItem
                    {
                        PlaylistItemId = "clip-1",
                        Title = "Sample Episode",
                        RelativePath = "video/01-killer-shrews.mp4"
                    },
                    QueuedAtUtc = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
                    StartedAtUtc = DateTimeOffset.Parse("2026-05-20T12:05:00Z")
                }
            ],
            Metadata = new Dictionary<string, string>
            {
                ["queue_type"] = "operator"
            }
        };

        var json = JsonSerializer.Serialize(queue);
        var roundTrip = JsonSerializer.Deserialize<MediaPlaybackQueue>(json);

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.Items[0].State, Is.EqualTo(MediaQueueItemState.Playing));
        Assert.That(() => roundTrip.Items[0].PlaylistItem!.RelativePath, Is.EqualTo("video/01-killer-shrews.mp4"));
        Assert.That(() => roundTrip.ActiveQueueItemId, Is.EqualTo("queue-item:1"));
    }
}
