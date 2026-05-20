using System.Text.Json;
using NUnit.Framework;
using PolyhydraGames.Core.Models.Media;
using PolyhydraGames.Core.Models.Media.Playlists;

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
            },
            Enrichment = new MediaEnrichmentMetadata
            {
                Title = "Svengoolie - The Killer Shrews",
                Description = "A hosted horror broadcast.",
                ReleaseYear = 1959,
                Genres = "Horror, Sci-Fi",
                RuntimeMinutes = 97,
                Provider = "tmdb",
                ProviderKey = "12345",
                ImdbLink = "https://www.imdb.com/title/tt0051730/",
                TvMazeLink = "https://www.tvmaze.com/",
                PosterUrl = "https://example.com/poster.jpg",
                BackdropUrl = "https://example.com/backdrop.jpg",
                Providers =
                [
                    new MediaProviderReference
                    {
                        ProviderKey = "tmdb",
                        ExternalId = "12345",
                        DisplayName = "The Movie Database",
                        Url = "https://www.themoviedb.org/movie/12345",
                        LastSeenAt = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
                    },
                    new MediaProviderReference
                    {
                        ProviderKey = "tvmaze",
                        ExternalId = "67890",
                        DisplayName = "TV Maze",
                        Url = "https://www.tvmaze.com/shows/67890",
                    }
                ]
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
        Assert.That(roundTrip.Enrichment!.Title, Is.EqualTo("Svengoolie - The Killer Shrews"));
        Assert.That(roundTrip.Enrichment.Provider, Is.EqualTo("tmdb"));
        Assert.That(roundTrip.Enrichment.Providers, Has.Count.EqualTo(2));
        Assert.That(roundTrip.Enrichment.Providers[0].ProviderKey, Is.EqualTo("tmdb"));
        Assert.That(roundTrip.Enrichment.Providers[1].ExternalId, Is.EqualTo("67890"));
    }

    [Test]
    public void MediaAppLaunchRequest_SerializesPortableLaunchShape()
    {
        var request = new MediaAppLaunchRequest
        {
            AppId = "vlc",
            TargetSlug = "media://svengoolie-live/Movies/The%20Blob%20%281958%29.mkv",
            DeviceId = "stream-box",
            Target = new MediaLaunchTarget
            {
                RootId = "svengoolie-live",
                RelativePath = "Movies/The Blob (1958).mkv",
                MediaFileId = "catalog:the-blob",
                FileName = "The Blob (1958).mkv",
                Kind = MediaKind.Video,
                Title = "The Blob",
                PosterUrl = "https://cdn.example/poster.jpg",
                SourceCatalogId = "9b8fdf31-0c88-41bc-b879-8d4b1a8d6f50"
            }
        };

        var json = JsonSerializer.Serialize(request);
        var roundTrip = JsonSerializer.Deserialize<MediaAppLaunchRequest>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.AppId, Is.EqualTo("vlc"));
        Assert.That(roundTrip.DeviceId, Is.EqualTo("stream-box"));
        Assert.That(roundTrip.TargetSlug, Is.EqualTo(request.TargetSlug));
        Assert.That(roundTrip.Target.RootId, Is.EqualTo("svengoolie-live"));
        Assert.That(roundTrip.Target.RelativePath, Is.EqualTo("Movies/The Blob (1958).mkv"));
        Assert.That(roundTrip.Target.Kind, Is.EqualTo(MediaKind.Video));
        Assert.That(roundTrip.Target.Title, Is.EqualTo("The Blob"));
    }

    [Test]
    public void MediaPlaylist_SerializesBuseyStyleManifestShape()
    {
        var playlist = new MediaPlaylist
        {
            PlaylistId = "busey:my-show-01",
            Name = "my-show-01",
            OverlayPreset = "default",
            Playback = new MediaPlaylistPlaybackOptions
            {
                Loop = true,
                Shuffle = false,
                StartIndex = 0
            },
            Items =
            [
                new MediaPlaylistItem
                {
                    Title = "Intro",
                    RelativePath = "video/01-intro.mp4",
                    NowPlayingText = "Now Playing: Intro"
                },
                new MediaPlaylistItem
                {
                    Title = "Episode 1",
                    RelativePath = "video/02-episode.mp4"
                }
            ]
        };

        var json = JsonSerializer.Serialize(playlist);
        var roundTrip = JsonSerializer.Deserialize<MediaPlaylist>(json);
        var validation = MediaPlaylistHelpers.Validate(roundTrip);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.Name, Is.EqualTo("my-show-01"));
        Assert.That(roundTrip.OverlayPreset, Is.EqualTo("default"));
        Assert.That(roundTrip.Playback.Loop, Is.True);
        Assert.That(roundTrip.Items, Has.Count.EqualTo(2));
        Assert.That(roundTrip.Items[0].RelativePath, Is.EqualTo("video/01-intro.mp4"));
        Assert.That(MediaPlaylistHelpers.GetNowPlayingText(roundTrip.Items[0]), Is.EqualTo("Now Playing: Intro"));
        Assert.That(MediaPlaylistHelpers.GetNowPlayingText(roundTrip.Items[1]), Is.EqualTo("Episode 1"));
        Assert.That(validation.IsValid, Is.True, string.Join("; ", validation.Errors));
    }

    [Test]
    public void MediaPlaylist_SupportsChopItSegmentOptionsAndEmbeddedMedia()
    {
        var playlist = new MediaPlaylist
        {
            PlaylistId = "chop-it:svengoolie",
            Name = "Svengoolie Segments",
            SourceCollectionName = "Svengoolie",
            Source = new MediaPlaylistSource
            {
                Name = "Svengoolie",
                RootPath = "/mnt/syn2/video/TV Shows/Svengoolie",
                Recursive = true,
                FilePatterns = [ "*.mp4", "*.mkv" ]
            },
            Tags = ["svengoolie", "segments"],
            Playback = new MediaPlaylistPlaybackOptions
            {
                Loop = true,
                RandomOrder = true,
                ClipLength = TimeSpan.FromSeconds(30),
                CrossfadeDuration = TimeSpan.FromSeconds(2)
            },
            Items =
            [
                new MediaPlaylistItem
                {
                    PlaylistItemId = "clip-1",
                    Title = "The Killer Shrews",
                    SourceCollection = "Svengoolie",
                    StartOffset = TimeSpan.FromMinutes(10),
                    Duration = TimeSpan.FromSeconds(30),
                    Weight = 2,
                    Media = new LocalMediaFile
                    {
                        MediaFileId = "nas:123",
                        FileName = "Svengoolie - The Killer Shrews [S02E20].mp4",
                        Location = new MediaFileLocation
                        {
                            RelativePath = "TV Shows/Svengoolie/Svengoolie - The Killer Shrews [S02E20].mp4"
                        }
                    }
                }
            ]
        };

        var json = JsonSerializer.Serialize(playlist);
        var roundTrip = JsonSerializer.Deserialize<MediaPlaylist>(json);
        var playable = MediaPlaylistHelpers.GetPlayableItems(roundTrip!);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.Playback.RandomOrder, Is.True);
        Assert.That(roundTrip.Playback.ClipLength, Is.EqualTo(TimeSpan.FromSeconds(30)));
        Assert.That(roundTrip.Source!.RootPath, Is.EqualTo("/mnt/syn2/video/TV Shows/Svengoolie"));
        Assert.That(roundTrip.Source.FilePatterns, Has.Count.EqualTo(2));
        Assert.That(roundTrip.Items[0].Media!.MediaFileId, Is.EqualTo("nas:123"));
        Assert.That(playable, Has.Count.EqualTo(1));
        Assert.That(MediaPlaylistHelpers.Validate(roundTrip).IsValid, Is.True);
    }

    [Test]
    public void MediaAssetReference_SerializesAndDeserializesSharedShape()
    {
        var reference = new MediaAssetReference
        {
            AssetId = "poster:the-blob",
            Title = "The Blob Poster",
            Description = "Poster art for the 1958 release",
            Kind = MediaAssetReferenceKind.Poster,
            Role = "poster",
            Url = "https://cdn.example.com/posters/the-blob.jpg",
            SourceId = "tmdb:12345",
            SourceName = "TMDb",
            Location = new MediaFileLocation
            {
                RootId = "cdn",
                RootName = "CDN",
                RelativePath = "posters/the-blob.jpg",
                PublicUrl = "https://cdn.example.com/posters/the-blob.jpg"
            },
            Metadata = new Dictionary<string, string>
            {
                ["license"] = "public-domain",
                ["provider"] = "tmdb",
            }
        };

        var json = JsonSerializer.Serialize(reference);
        var roundTrip = JsonSerializer.Deserialize<MediaAssetReference>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.AssetId, Is.EqualTo("poster:the-blob"));
        Assert.That(roundTrip.Kind, Is.EqualTo(MediaAssetReferenceKind.Poster));
        Assert.That(roundTrip.Role, Is.EqualTo("poster"));
        Assert.That(roundTrip.Location!.RelativePath, Is.EqualTo("posters/the-blob.jpg"));
        Assert.That(roundTrip.Metadata["license"], Is.EqualTo("public-domain"));
        Assert.That(roundTrip.Metadata["provider"], Is.EqualTo("tmdb"));
    }

    [Test]
    public void MediaProviderReference_SerializesAndDeserializesSharedShape()
    {
        var reference = new MediaProviderReference
        {
            ProviderKey = "tmdb",
            ExternalId = "12345",
            DisplayName = "The Movie Database",
            Url = "https://www.themoviedb.org/movie/12345",
            LastSeenAt = DateTimeOffset.Parse("2026-05-20T12:00:00Z"),
            Metadata = new Dictionary<string, string>
            {
                ["region"] = "US",
                ["priority"] = "primary",
            }
        };

        var json = JsonSerializer.Serialize(reference);
        var roundTrip = JsonSerializer.Deserialize<MediaProviderReference>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip!.ProviderKey, Is.EqualTo("tmdb"));
        Assert.That(roundTrip.ExternalId, Is.EqualTo("12345"));
        Assert.That(roundTrip.DisplayName, Is.EqualTo("The Movie Database"));
        Assert.That(roundTrip.Metadata["region"], Is.EqualTo("US"));
        Assert.That(roundTrip.Metadata["priority"], Is.EqualTo("primary"));
    }

    [Test]
    public void MediaRelationship_SerializesAndDeserializesSharedShape()
    {
        var relationships = new[]
        {
            new MediaRelationship
            {
                RelationshipId = "rel:episode-video",
                Kind = MediaRelationshipKind.EpisodeOf,
                Role = "episode-video",
                Source = new MediaRelationshipEndpoint
                {
                    EntityId = "catalog:svengoolie-episode-20",
                    Kind = MediaRelationshipEndpointKind.CatalogItem,
                    Label = "The Killer Shrews"
                },
                Target = new MediaRelationshipEndpoint
                {
                    EntityId = "media:nas:123",
                    Kind = MediaRelationshipEndpointKind.MediaFile,
                    Label = "Svengoolie - The Killer Shrews [S02E20].mp4",
                    RelativePath = "TV Shows/Svengoolie/Svengoolie - The Killer Shrews [S02E20].mp4"
                },
                Metadata = new Dictionary<string, string>
                {
                    ["source"] = "svengoolie",
                }
            },
            new MediaRelationship
            {
                RelationshipId = "rel:show-poster",
                Kind = MediaRelationshipKind.PosterFor,
                Source = new MediaRelationshipEndpoint
                {
                    EntityId = "catalog:svengoolie",
                    Kind = MediaRelationshipEndpointKind.CatalogItem,
                    Label = "Svengoolie"
                },
                Target = new MediaRelationshipEndpoint
                {
                    EntityId = "asset:poster:the-killer-shrews",
                    Kind = MediaRelationshipEndpointKind.MediaAsset,
                    Label = "The Killer Shrews Poster",
                    Url = "https://cdn.example.com/posters/the-killer-shrews.jpg"
                }
            },
            new MediaRelationship
            {
                RelationshipId = "rel:rom-box-art",
                Kind = MediaRelationshipKind.BoxArtFor,
                Source = new MediaRelationshipEndpoint
                {
                    EntityId = "rom:metroid",
                    Kind = MediaRelationshipEndpointKind.MediaFile,
                    Label = "Metroid.sfc"
                },
                Target = new MediaRelationshipEndpoint
                {
                    EntityId = "asset:box-art:metroid",
                    Kind = MediaRelationshipEndpointKind.MediaAsset,
                    Label = "Metroid Box Art",
                    Url = "https://cdn.example.com/box-art/metroid.png"
                }
            }
        };

        var json = JsonSerializer.Serialize(relationships);
        var roundTrip = JsonSerializer.Deserialize<MediaRelationship[]>(json);

        Assert.That(roundTrip, Is.Not.Null);
        Assert.That(roundTrip, Has.Length.EqualTo(3));
        Assert.That(roundTrip![0].Kind, Is.EqualTo(MediaRelationshipKind.EpisodeOf));
        Assert.That(roundTrip[0].Source.Kind, Is.EqualTo(MediaRelationshipEndpointKind.CatalogItem));
        Assert.That(roundTrip[0].Target.RelativePath, Is.EqualTo("TV Shows/Svengoolie/Svengoolie - The Killer Shrews [S02E20].mp4"));
        Assert.That(roundTrip[1].Kind, Is.EqualTo(MediaRelationshipKind.PosterFor));
        Assert.That(roundTrip[1].Target.Kind, Is.EqualTo(MediaRelationshipEndpointKind.MediaAsset));
        Assert.That(roundTrip[2].Kind, Is.EqualTo(MediaRelationshipKind.BoxArtFor));
        Assert.That(roundTrip[2].Target.Url, Is.EqualTo("https://cdn.example.com/box-art/metroid.png"));
    }

    [Test]
    public void MediaPlaylistHelpers_ReportsShapeErrors()
    {
        var playlist = new MediaPlaylist
        {
            Name = "",
            Playback = new MediaPlaylistPlaybackOptions { StartIndex = 2 },
            Items =
            [
                new MediaPlaylistItem
                {
                    Title = "",
                    Weight = 0,
                    Duration = TimeSpan.Zero,
                    StartOffset = TimeSpan.FromSeconds(-1)
                }
            ]
        };

        var validation = MediaPlaylistHelpers.Validate(playlist);

        Assert.That(validation.IsValid, Is.False);
        Assert.That(validation.Errors, Has.Some.EqualTo("Playlist name is required."));
        Assert.That(validation.Errors, Has.Some.EqualTo("Playback start index must point to an existing playlist item."));
        Assert.That(validation.Errors, Has.Some.EqualTo("Playlist item 0 must have a title."));
        Assert.That(validation.Errors, Has.Some.EqualTo("Playlist item 0 must reference media, a media id, or a path."));
        Assert.That(validation.Errors, Has.Some.EqualTo("Playlist item 0 weight must be greater than zero."));
        Assert.That(validation.Errors, Has.Some.EqualTo("Playlist item 0 duration must be greater than zero."));
        Assert.That(validation.Errors, Has.Some.EqualTo("Playlist item 0 start offset cannot be negative."));
    }
}
