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
            RootId = "syn2-video",
            RootName = "Synology Video",
            CurrentPath = "TV Shows/Svengoolie",
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

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.ProcessedFiles, Is.EqualTo(42));
        Assert.That(roundTrip.CurrentPath, Is.EqualTo("TV Shows/Svengoolie"));
        Assert.That(roundTrip.Metadata["phase"], Is.EqualTo("indexing"));
    }

    [Test]
    public void MediaScanSummary_SerializesAndDeserializesSharedShape()
    {
        var summary = new MediaScanSummary
        {
            ScanId = "scan:001",
            RootId = "syn2-video",
            RootName = "Synology Video",
            StartedAtUtc = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
            CompletedAtUtc = DateTimeOffset.Parse("2026-05-20T12:20:00Z"),
            TotalFiles = 100,
            IndexedFiles = 95,
            SkippedFiles = 4,
            FailedFiles = 1,
            Warnings = ["1 file could not be probed."],
            Metadata = new Dictionary<string, string>
            {
                ["server"] = "nas-1"
            }
        };

        var json = JsonSerializer.Serialize(summary);
        var roundTrip = JsonSerializer.Deserialize<MediaScanSummary>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.IndexedFiles, Is.EqualTo(95));
        Assert.That(roundTrip.Warnings[0], Is.EqualTo("1 file could not be probed."));
        Assert.That(roundTrip.Metadata["server"], Is.EqualTo("nas-1"));
    }

    [Test]
    public void IndexedMediaFileSummary_SerializesAndDeserializesSharedShape()
    {
        var file = new IndexedMediaFileSummary
        {
            MediaFileId = "nas:123",
            FileName = "The Killer Shrews.mp4",
            RootId = "syn2-video",
            RelativePath = "TV Shows/Svengoolie/The Killer Shrews.mp4",
            FullPath = "/mnt/syn2/video/TV Shows/Svengoolie/The Killer Shrews.mp4",
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

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.Kind, Is.EqualTo(MediaKind.Video));
        Assert.That(roundTrip.Availability, Is.EqualTo(MediaAvailability.Available));
        Assert.That(roundTrip.Metadata["source"], Is.EqualTo("scan"));
    }
}
