using System.Text.Json;
using NUnit.Framework;
using PolyhydraGames.Core.Models.Media;

namespace PolyhydraGames.Core.Models.Tests;

public class MediaModelsTests
{
    [TestCase("mp4", ".mp4")]
    [TestCase(".MKV", ".mkv")]
    [TestCase("  FlAc  ", ".flac")]
    [TestCase("", "")]
    [TestCase(null, "")]
    public void NormalizeExtension_ReturnsLowercaseDotExtension(string? extension, string expected)
    {
        Assert.That(MediaPathHelpers.NormalizeExtension(extension), Is.EqualTo(expected));
    }

    [TestCase("Movies\\Svengoolie\\clip.mp4", "Movies/Svengoolie/clip.mp4")]
    [TestCase("/Movies/Svengoolie/clip.mp4", "Movies/Svengoolie/clip.mp4")]
    [TestCase("  TV Shows/Svengoolie/file.mkv  ", "TV Shows/Svengoolie/file.mkv")]
    [TestCase(null, "")]
    public void NormalizeRelativePath_ReturnsPortableRelativePath(string? path, string expected)
    {
        Assert.That(MediaPathHelpers.NormalizeRelativePath(path), Is.EqualTo(expected));
    }

    [TestCase(".mp4", MediaKind.Video)]
    [TestCase(".m4a", MediaKind.Audio)]
    [TestCase(".png", MediaKind.Image)]
    [TestCase(".sfc", MediaKind.Rom)]
    [TestCase(".srt", MediaKind.Document)]
    [TestCase(".unknown", MediaKind.Other)]
    [TestCase("", MediaKind.Unknown)]
    public void ClassifyByExtension_ReturnsExpectedMediaKind(string extension, MediaKind expected)
    {
        Assert.That(MediaPathHelpers.ClassifyByExtension(extension), Is.EqualTo(expected));
    }

    [Test]
    public void LocalMediaFile_SerializesAndDeserializesSharedShape()
    {
        var file = new LocalMediaFile
        {
            MediaFileId = "svengoolie:001",
            FileName = "Svengoolie - The Killer Shrews [S02E20].mp4",
            FileSizeBytes = 123456789,
            Extension = ".mp4",
            Kind = MediaKind.Video,
            Availability = MediaAvailability.Available,
            Location = new MediaFileLocation
            {
                RootId = "syn2-video",
                RootName = "Synology Video",
                RelativePath = "TV Shows/Svengoolie/Svengoolie - The Killer Shrews [S02E20].mp4",
                FullPath = "/mnt/syn2/video/TV Shows/Svengoolie/Svengoolie - The Killer Shrews [S02E20].mp4",
                PublicUrl = "https://svengooliedb.polyhydragames.com/media/Svengoolie%20-%20The%20Killer%20Shrews%20%5BS02E20%5D.mp4"
            },
            TechnicalMetadata = new MediaTechnicalMetadata
            {
                Duration = TimeSpan.FromMinutes(97),
                Video = new VideoMediaMetadata
                {
                    Codec = "h264",
                    Container = "mp4",
                    Width = 1920,
                    Height = 1080,
                    Framerate = 29.97,
                    BitrateBitsPerSecond = 5_000_000
                },
                Audio = new AudioMediaMetadata
                {
                    Codec = "aac",
                    Channels = 2,
                    SampleRateHz = 48000
                }
            }
        };

        var json = JsonSerializer.Serialize(file);
        var roundTrip = JsonSerializer.Deserialize<LocalMediaFile>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.FileName, Is.EqualTo(file.FileName));
        Assert.That(roundTrip.Kind, Is.EqualTo(MediaKind.Video));
        Assert.That(roundTrip.Availability, Is.EqualTo(MediaAvailability.Available));
        Assert.That(roundTrip.Location.RelativePath, Is.EqualTo(file.Location.RelativePath));
        Assert.That(roundTrip.TechnicalMetadata!.Video!.Width, Is.EqualTo(1920));
        Assert.That(roundTrip.TechnicalMetadata.Audio!.Codec, Is.EqualTo("aac"));
    }
}
