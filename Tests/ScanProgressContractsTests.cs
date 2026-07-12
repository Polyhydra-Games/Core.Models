using System;
using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;
using PolyhydraGames.Core.Models.Media;
using PolyhydraGames.Core.Models.Media.Scanning;

namespace PolyhydraGames.Core.Models.Tests;

public class ScanProgressContractsTests
{
    [Test]
    public void MediaScanProgress_SerializesAndDeserializesSharedShape()
    {
        var progress = new MediaScanProgress
        {
            ScanId = "scan:001",
            RootId = "library-video",
            RootName = "Video Library",
            CurrentPath = "TV Shows/Sample Show",
            DiscoveredFiles = 100,
            ProcessedFiles = 42,
            IndexedFiles = 40,
            SkippedFiles = 2,
            FailedFiles = 0,
            StartedAtUtc = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
            UpdatedAtUtc = DateTimeOffset.Parse("2026-05-20T12:10:00Z"),
            Complete = false,
            Metadata = new Dictionary<string, string>
            {
                ["phase"] = "indexing"
            }
        };

        var json = JsonSerializer.Serialize(progress);
        var roundTrip = JsonSerializer.Deserialize<MediaScanProgress>(json);

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.ProcessedFiles, Is.EqualTo(42));
        Assert.That(() => roundTrip.CurrentPath, Is.EqualTo("TV Shows/Sample Show"));
        Assert.That(() => roundTrip.Metadata["phase"], Is.EqualTo("indexing"));
    }

    [Test]
    public void MediaScanSummary_SerializesAndDeserializesSharedShape()
    {
        var summary = new MediaScanSummary
        {
            ScanId = "scan:001",
            RootId = "library-video",
            RootName = "Video Library",
            StartedAtUtc = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
            CompletedAtUtc = DateTimeOffset.Parse("2026-05-20T12:20:00Z"),
            TotalFiles = 100,
            IndexedFiles = 95,
            SkippedFiles = 4,
            FailedFiles = 1,
            Warnings = ["1 file could not be probed."],
            Metadata = new Dictionary<string, string>
            {
                ["source"] = "scanner"
            }
        };

        var json = JsonSerializer.Serialize(summary);
        var roundTrip = JsonSerializer.Deserialize<MediaScanSummary>(json);

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.IndexedFiles, Is.EqualTo(95));
        Assert.That(() => roundTrip.Warnings[0], Is.EqualTo("1 file could not be probed."));
        Assert.That(() => roundTrip.Metadata["source"], Is.EqualTo("scanner"));
    }

    [Test]
    public void IndexedMediaFileSummary_SerializesAndDeserializesSharedShape()
    {
        var file = new IndexedMediaFileSummary
        {
            MediaFileId = "library:123",
            FileName = "Sample Episode.mp4",
            RootId = "library-video",
            RelativePath = "shows/example/example.mp4",
            FullPath = "/media/library/shows/example/example.mp4",
            Kind = MediaKind.Video,
            Availability = MediaAvailability.Available,
            IndexedAtUtc = DateTimeOffset.Parse("2026-05-20T12:10:00Z"),
            Checksum = "sha256:abc123",
            Metadata = new Dictionary<string, string>
            {
                ["source"] = "scan"
            }
        };

        var json = JsonSerializer.Serialize(file);
        var roundTrip = JsonSerializer.Deserialize<IndexedMediaFileSummary>(json);

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.Kind, Is.EqualTo(MediaKind.Video));
        Assert.That(() => roundTrip.Availability, Is.EqualTo(MediaAvailability.Available));
        Assert.That(() => roundTrip.Metadata["source"], Is.EqualTo("scan"));
    }
}
