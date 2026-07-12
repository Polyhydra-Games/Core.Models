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
        Assert.That(() => MediaPathHelpers.NormalizeExtension(extension), Is.EqualTo(expected));
    }

    [TestCase("Movies\\Sample Show\\clip.mp4", "Movies/Sample Show/clip.mp4")]
    [TestCase("/Movies/Sample Show/clip.mp4", "Movies/Sample Show/clip.mp4")]
    [TestCase("  TV Shows/Sample Show/file.mkv  ", "TV Shows/Sample Show/file.mkv")]
    [TestCase(null, "")]
    public void NormalizeRelativePath_ReturnsPortableRelativePath(string? path, string expected)
    {
        Assert.That(() => MediaPathHelpers.NormalizeRelativePath(path), Is.EqualTo(expected));
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
        Assert.That(() => MediaPathHelpers.ClassifyByExtension(extension), Is.EqualTo(expected));
    }

    [Test]
    public void LocalMediaFile_SerializesAndDeserializesSharedShape()
    {
        var file = new LocalMediaFile
        {
            MediaFileId = "sample-show:001",
            FileName = "Sample Show - Sample Episode [S02E20].mp4",
            FileSizeBytes = 123456789,
            Extension = ".mp4",
            Kind = MediaKind.Video,
            Availability = MediaAvailability.Available,
            Location = new MediaFileLocation
            {
                RootId = "library-video",
                RootName = "Video Library",
                RelativePath = "shows/example/example.mp4",
                FullPath = "/media/library/shows/example/example.mp4",
                PublicUrl = "https://cdn.example.test/media/example.mp4"
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
                Title = "Sample Show - Sample Episode",
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

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.FileName, Is.EqualTo(file.FileName));
        Assert.That(() => roundTrip.Kind, Is.EqualTo(MediaKind.Video));
        Assert.That(() => roundTrip.Availability, Is.EqualTo(MediaAvailability.Available));
        Assert.That(() => roundTrip.Location.RelativePath, Is.EqualTo(file.Location.RelativePath));
        Assert.That(() => roundTrip.TechnicalMetadata!.Video!.Width, Is.EqualTo(1920));
        Assert.That(() => roundTrip.TechnicalMetadata.Audio!.Codec, Is.EqualTo("aac"));
        Assert.That(() => roundTrip.Enrichment!.Title, Is.EqualTo("Sample Show - Sample Episode"));
        Assert.That(() => roundTrip.Enrichment.Provider, Is.EqualTo("tmdb"));
        Assert.That(() => roundTrip.Enrichment.Providers, Has.Count.EqualTo(2));
        Assert.That(() => roundTrip.Enrichment.Providers[0].ProviderKey, Is.EqualTo("tmdb"));
        Assert.That(() => roundTrip.Enrichment.Providers[1].ExternalId, Is.EqualTo("67890"));
    }

    [Test]
    public void MediaAppLaunchRequest_SerializesPortableLaunchShape()
    {
        var request = new MediaAppLaunchRequest
        {
            AppId = "vlc",
            TargetSlug = "media://sample-show-live/Movies/The%20Blob%20%281958%29.mkv",
            DeviceId = "stream-box",
            Target = new MediaLaunchTarget
            {
                RootId = "sample-show-live",
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

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.AppId, Is.EqualTo("vlc"));
        Assert.That(() => roundTrip.DeviceId, Is.EqualTo("stream-box"));
        Assert.That(() => roundTrip.TargetSlug, Is.EqualTo(request.TargetSlug));
        Assert.That(() => roundTrip.Target.RootId, Is.EqualTo("sample-show-live"));
        Assert.That(() => roundTrip.Target.RelativePath, Is.EqualTo("Movies/The Blob (1958).mkv"));
        Assert.That(() => roundTrip.Target.Kind, Is.EqualTo(MediaKind.Video));
        Assert.That(() => roundTrip.Target.Title, Is.EqualTo("The Blob"));
    }

    [Test]
    public void MediaPlaylist_SerializesGenericManifestShape()
    {
        var playlist = new MediaPlaylist
        {
            PlaylistId = "playlist:my-show-01",
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

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.Name, Is.EqualTo("my-show-01"));
        Assert.That(() => roundTrip.OverlayPreset, Is.EqualTo("default"));
        Assert.That(() => roundTrip.Playback.Loop, Is.True);
        Assert.That(() => roundTrip.Items, Has.Count.EqualTo(2));
        Assert.That(() => roundTrip.Items[0].RelativePath, Is.EqualTo("video/01-intro.mp4"));
        Assert.That(() => MediaPlaylistHelpers.GetNowPlayingText(roundTrip.Items[0]), Is.EqualTo("Now Playing: Intro"));
        Assert.That(() => MediaPlaylistHelpers.GetNowPlayingText(roundTrip.Items[1]), Is.EqualTo("Episode 1"));
        Assert.That(() => validation.IsValid, Is.True, string.Join("; ", validation.Errors));
    }

    [Test]
    public void MediaPlaylist_SupportsSegmentOptionsAndEmbeddedMedia()
    {
        var playlist = new MediaPlaylist
        {
            PlaylistId = "playlist:sample-show",
            Name = "Sample Show Segments",
            SourceCollectionName = "Sample Show",
            Source = new MediaPlaylistSource
            {
                Name = "Example Collection",
                RootPath = "/media/library/shows/example",
                Recursive = true,
                FilePatterns = [ "*.mp4", "*.mkv" ]
            },
            Tags = ["sample-show", "segments"],
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
                    Title = "Sample Episode",
                    SourceCollection = "Sample Show",
                    StartOffset = TimeSpan.FromMinutes(10),
                    Duration = TimeSpan.FromSeconds(30),
                    Weight = 2,
                    Media = new LocalMediaFile
                    {
                        MediaFileId = "library:123",
                        FileName = "Sample Show - Sample Episode [S02E20].mp4",
                        Location = new MediaFileLocation
                        {
                            RelativePath = "TV Shows/Sample Show/Sample Show - Sample Episode [S02E20].mp4"
                        }
                    }
                }
            ]
        };

        var json = JsonSerializer.Serialize(playlist);
        var roundTrip = JsonSerializer.Deserialize<MediaPlaylist>(json);
        var playable = MediaPlaylistHelpers.GetPlayableItems(roundTrip!);

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.Playback.RandomOrder, Is.True);
        Assert.That(() => roundTrip.Playback.ClipLength, Is.EqualTo(TimeSpan.FromSeconds(30)));
        Assert.That(() => roundTrip.Source!.RootPath, Is.EqualTo("/media/library/shows/example"));
        Assert.That(() => roundTrip.Source.FilePatterns, Has.Count.EqualTo(2));
        Assert.That(() => roundTrip.Items[0].Media!.MediaFileId, Is.EqualTo("library:123"));
        Assert.That(() => playable, Has.Count.EqualTo(1));
        Assert.That(() => MediaPlaylistHelpers.Validate(roundTrip).IsValid, Is.True);
    }

    [Test]
    public void MediaPlaylist_DeserializesGenericManifestPayload()
    {
        const string json = """
            {
              "Version": 1,
              "PlaylistId": "playlist:show-01",
              "Name": "show-01",
              "OverlayPreset": "default",
              "Playback": {
                "Loop": true,
                "Shuffle": false,
                "StartIndex": 0
              },
              "Items": [
                {
                  "Title": "Intro",
                  "RelativePath": "video/01-intro.mp4",
                  "NowPlayingText": "Now Playing: Intro"
                },
                {
                  "Title": "Episode",
                  "RelativePath": "video/02-episode.mp4"
                }
              ]
            }
            """;

        var playlist = JsonSerializer.Deserialize<MediaPlaylist>(json);
        var validation = MediaPlaylistHelpers.Validate(playlist);

        Assert.That(() => playlist, Is.Not.Null);
        Assert.That(() => playlist!.PlaylistId, Is.EqualTo("playlist:show-01"));
        Assert.That(() => playlist.OverlayPreset, Is.EqualTo("default"));
        Assert.That(() => playlist.Playback.Loop, Is.True);
        Assert.That(() => playlist.Playback.StartIndex, Is.EqualTo(0));
        Assert.That(() => playlist.Items, Has.Count.EqualTo(2));
        Assert.That(() => playlist.Items[0].RelativePath, Is.EqualTo("video/01-intro.mp4"));
        Assert.That(() => MediaPlaylistHelpers.GetNowPlayingText(playlist.Items[0]), Is.EqualTo("Now Playing: Intro"));
        Assert.That(() => MediaPlaylistHelpers.GetNowPlayingText(playlist.Items[1]), Is.EqualTo("Episode"));
        Assert.That(() => validation.IsValid, Is.True, string.Join("; ", validation.Errors));
    }

    [Test]
    public void MediaPlaylist_DeserializesSegmentPayload()
    {
        const string json = """
            {
              "Version": 1,
              "PlaylistId": "playlist:sample-show",
              "Name": "Sample Show Segments",
              "SourceCollectionName": "Sample Show",
              "Source": {
                "Name": "Example Collection",
                "RootPath": "/media/library/shows/example",
                "Recursive": true,
                "FilePatterns": [ "*.mp4", "*.mkv" ]
              },
              "Playback": {
                "Loop": true,
                "RandomOrder": true,
                "ClipLength": "00:00:30",
                "CrossfadeDuration": "00:00:02"
              },
              "Items": [
                {
                  "PlaylistItemId": "clip-1",
                  "Title": "Sample Episode",
                  "SourceCollection": "Sample Show",
                  "StartOffset": "00:10:00",
                  "Duration": "00:00:30",
                  "Weight": 2,
                  "Media": {
                    "MediaFileId": "library:123",
                    "FileName": "Sample Show - Sample Episode [S02E20].mp4",
                    "Location": {
                      "RelativePath": "TV Shows/Sample Show/Sample Show - Sample Episode [S02E20].mp4"
                    }
                  }
                }
              ]
            }
            """;

        var playlist = JsonSerializer.Deserialize<MediaPlaylist>(json);
        var playable = MediaPlaylistHelpers.GetPlayableItems(playlist!);

        Assert.That(() => playlist, Is.Not.Null);
        Assert.That(() => playlist!.SourceCollectionName, Is.EqualTo("Sample Show"));
        Assert.That(() => playlist.Source!.RootPath, Is.EqualTo("/media/library/shows/example"));
        Assert.That(() => playlist.Source.FilePatterns, Is.EquivalentTo(new[] { "*.mp4", "*.mkv" }));
        Assert.That(() => playlist.Playback.RandomOrder, Is.True);
        Assert.That(() => playlist.Playback.ClipLength, Is.EqualTo(TimeSpan.FromSeconds(30)));
        Assert.That(() => playlist.Items[0].StartOffset, Is.EqualTo(TimeSpan.FromMinutes(10)));
        Assert.That(() => playlist.Items[0].Media!.MediaFileId, Is.EqualTo("library:123"));
        Assert.That(() => playable, Has.Count.EqualTo(1));
        Assert.That(() => MediaPlaylistHelpers.Validate(playlist).IsValid, Is.True);
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

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.AssetId, Is.EqualTo("poster:the-blob"));
        Assert.That(() => roundTrip.Kind, Is.EqualTo(MediaAssetReferenceKind.Poster));
        Assert.That(() => roundTrip.Role, Is.EqualTo("poster"));
        Assert.That(() => roundTrip.Location!.RelativePath, Is.EqualTo("posters/the-blob.jpg"));
        Assert.That(() => roundTrip.Metadata["license"], Is.EqualTo("public-domain"));
        Assert.That(() => roundTrip.Metadata["provider"], Is.EqualTo("tmdb"));
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

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.ProviderKey, Is.EqualTo("tmdb"));
        Assert.That(() => roundTrip.ExternalId, Is.EqualTo("12345"));
        Assert.That(() => roundTrip.DisplayName, Is.EqualTo("The Movie Database"));
        Assert.That(() => roundTrip.Metadata["region"], Is.EqualTo("US"));
        Assert.That(() => roundTrip.Metadata["priority"], Is.EqualTo("primary"));
    }

    [Test]
    public void MediaItemReference_DefaultsToSparsePortableReference()
    {
        var reference = new MediaItemReference();

        Assert.That(() => reference.ReferenceId, Is.Null);
        Assert.That(() => reference.StorageId, Is.Null);
        Assert.That(() => reference.ContentHash, Is.Null);
        Assert.That(() => reference.CdnId, Is.Null);
        Assert.That(() => reference.CdnUrl, Is.Null);
        Assert.That(() => reference.Kind, Is.EqualTo(MediaKind.Unknown));
        Assert.That(() => reference.DisplayTitle, Is.Null);
        Assert.That(() => reference.DisplaySubtitle, Is.Null);
        Assert.That(() => reference.Providers, Is.Empty);
        Assert.That(() => reference.Assets, Is.Empty);
        Assert.That(() => reference.Metadata, Is.Empty);
    }

    [Test]
    public void MediaItemReference_SerializesAndDeserializesLayeredSharedShape()
    {
        var reference = new MediaItemReference
        {
            ReferenceId = "tracker:playlist:item:001",
            StorageId = "core.library:media:tracker:modarchive:0001",
            ContentHash = "sha256:4d9676bce22c8e6f89edc1c491d58f8b5a3a2f4f85f4d7f447553f9f4d6f3a11",
            CdnId = "cdn:tracker-modules/modarchive/0001.mod",
            CdnUrl = "https://cdn.example.test/tracker-modules/modarchive/0001.mod",
            Kind = MediaKind.Audio,
            DisplayTitle = "Crystal Hammer",
            DisplaySubtitle = "tracker module - MOD",
            Providers =
            [
                new MediaProviderReference
                {
                    ProviderKey = "modarchive",
                    ExternalId = "0001",
                    DisplayName = "The Mod Archive",
                    Url = "https://modarchive.org/index.php?request=view_by_moduleid&query=0001",
                    Metadata = new Dictionary<string, string>
                    {
                        ["confidence"] = "authoritative",
                        ["format"] = "mod"
                    }
                },
                new MediaProviderReference
                {
                    ProviderKey = "example-provider",
                    ExternalId = "artist:example",
                    DisplayName = "Example Artist"
                }
            ],
            Assets =
            [
                new MediaAssetReference
                {
                    AssetId = "cdn:tracker-module:0001",
                    Title = "Crystal Hammer module",
                    Kind = MediaAssetReferenceKind.CdnAsset,
                    Role = "playback-source",
                    Url = "https://cdn.example.test/tracker-modules/modarchive/0001.mod",
                    SourceName = "Api.CDN",
                    Metadata = new Dictionary<string, string>
                    {
                        ["storageId"] = "core.library:media:tracker:modarchive:0001"
                    }
                },
                new MediaAssetReference
                {
                    AssetId = "artwork:tracker:0001",
                    Title = "Generated tracker artwork",
                    Kind = MediaAssetReferenceKind.ProviderImage,
                    Role = "poster",
                    Url = "https://cdn.example.test/tracker-artwork/0001.png"
                }
            ],
            Metadata = new Dictionary<string, string>
            {
                ["pilot"] = "tracker-music",
                ["sourceRepo"] = "core.tracker-audio"
            }
        };

        var json = JsonSerializer.Serialize(reference);
        var roundTrip = JsonSerializer.Deserialize<MediaItemReference>(json);

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip!.ReferenceId, Is.EqualTo("tracker:playlist:item:001"));
        Assert.That(() => roundTrip.StorageId, Is.EqualTo("core.library:media:tracker:modarchive:0001"));
        Assert.That(() => roundTrip.ContentHash, Does.StartWith("sha256:"));
        Assert.That(() => roundTrip.CdnId, Is.EqualTo("cdn:tracker-modules/modarchive/0001.mod"));
        Assert.That(() => roundTrip.CdnUrl, Is.EqualTo("https://cdn.example.test/tracker-modules/modarchive/0001.mod"));
        Assert.That(() => roundTrip.Kind, Is.EqualTo(MediaKind.Audio));
        Assert.That(() => roundTrip.DisplayTitle, Is.EqualTo("Crystal Hammer"));
        Assert.That(() => roundTrip.DisplaySubtitle, Is.EqualTo("tracker module - MOD"));
        Assert.That(() => roundTrip.Providers, Has.Count.EqualTo(2));
        Assert.That(() => roundTrip.Providers[0].ProviderKey, Is.EqualTo("modarchive"));
        Assert.That(() => roundTrip.Providers[0].Metadata["format"], Is.EqualTo("mod"));
        Assert.That(() => roundTrip.Providers[1].ExternalId, Is.EqualTo("artist:example"));
        Assert.That(() => roundTrip.Assets, Has.Count.EqualTo(2));
        Assert.That(() => roundTrip.Assets[0].Kind, Is.EqualTo(MediaAssetReferenceKind.CdnAsset));
        Assert.That(() => roundTrip.Assets[0].Metadata["storageId"], Is.EqualTo("core.library:media:tracker:modarchive:0001"));
        Assert.That(() => roundTrip.Assets[1].Role, Is.EqualTo("poster"));
        Assert.That(() => roundTrip.Metadata["pilot"], Is.EqualTo("tracker-music"));
        Assert.That(() => roundTrip.Metadata["sourceRepo"], Is.EqualTo("core.tracker-audio"));
    }

    [Test]
    public void MediaItemReference_AllowsEachOwningSystemToContributeOnlyItsLayer()
    {
        var nasOnly = new MediaItemReference
        {
            StorageId = "core.library:media:sample-show:the-blob",
            ContentHash = "sha256:abc123",
            Kind = MediaKind.Video,
            DisplayTitle = "The Blob (1958).mkv"
        };

        var metadataOnly = nasOnly with
        {
            DisplayTitle = "The Blob",
            Providers =
            [
                new MediaProviderReference
                {
                    ProviderKey = "tmdb",
                    ExternalId = "10843",
                    DisplayName = "The Blob"
                }
            ]
        };

        var cdnLayer = metadataOnly with
        {
            CdnId = "cdn:sample-show/the-blob",
            CdnUrl = "https://cdn.example.test/sample-show/the-blob.mkv",
            Assets =
            [
                new MediaAssetReference
                {
                    AssetId = "cdn:sample-show/the-blob",
                    Kind = MediaAssetReferenceKind.CdnAsset,
                    Role = "public-video",
                    Url = "https://cdn.example.test/sample-show/the-blob.mkv"
                }
            ]
        };

        Assert.That(() => nasOnly.StorageId, Is.EqualTo("core.library:media:sample-show:the-blob"));
        Assert.That(() => nasOnly.Providers, Is.Empty);
        Assert.That(() => nasOnly.CdnUrl, Is.Null);
        Assert.That(() => metadataOnly.StorageId, Is.EqualTo(nasOnly.StorageId));
        Assert.That(() => metadataOnly.Providers.Single().ProviderKey, Is.EqualTo("tmdb"));
        Assert.That(() => metadataOnly.CdnUrl, Is.Null);
        Assert.That(() => cdnLayer.StorageId, Is.EqualTo(nasOnly.StorageId));
        Assert.That(() => cdnLayer.Providers.Single().ExternalId, Is.EqualTo("10843"));
        Assert.That(() => cdnLayer.CdnUrl, Is.EqualTo("https://cdn.example.test/sample-show/the-blob.mkv"));
        Assert.That(() => cdnLayer.Assets.Single().Role, Is.EqualTo("public-video"));
    }

    [Test]
    public void MediaItemReference_DoesNotRequireLocalPathsOrProductState()
    {
        var reference = new MediaItemReference
        {
            StorageId = "core.library:media:music:0001",
            ContentHash = "sha256:def456",
            CdnUrl = "https://cdn.example.test/music/0001.flac",
            Kind = MediaKind.Audio,
            DisplayTitle = "Example Track",
            Providers =
            [
                new MediaProviderReference
                {
                    ProviderKey = "example-provider",
                    ExternalId = "track:0001"
                }
            ]
        };

        var json = JsonSerializer.Serialize(reference);

        Assert.That(() => json, Does.Not.Contain("FullPath"));
        Assert.That(() => json, Does.Not.Contain("RelativePath"));
        Assert.That(() => json, Does.Not.Contain("Queue"));
        Assert.That(() => json, Does.Not.Contain("Session"));
        Assert.That(() => json, Does.Not.Contain("Schedule"));
        Assert.That(() => json, Does.Not.Contain("/private-mount/"));
    }

    [Test]
    public void MediaItemReference_DeserializesDocumentedContractShape()
    {
        const string json = """
            {
              "StorageId": "core.library:media:tracker:0001",
              "ContentHash": "sha256:789",
              "CdnUrl": "https://cdn.example.test/tracker/0001.mod",
              "Kind": 2,
              "DisplayTitle": "Tracker Demo",
              "DisplaySubtitle": "MOD",
              "Providers": [
                {
                  "ProviderKey": "modarchive",
                  "ExternalId": "0001",
                  "DisplayName": "The Mod Archive"
                }
              ],
              "Assets": [
                {
                  "AssetId": "cdn:tracker:0001",
                  "Kind": 2,
                  "Role": "playback-source",
                  "Url": "https://cdn.example.test/tracker/0001.mod"
                }
              ],
              "Metadata": {
                "pilot": "tracker-music"
              }
            }
            """;

        var reference = JsonSerializer.Deserialize<MediaItemReference>(json);

        Assert.That(() => reference, Is.Not.Null);
        Assert.That(() => reference!.StorageId, Is.EqualTo("core.library:media:tracker:0001"));
        Assert.That(() => reference.ContentHash, Is.EqualTo("sha256:789"));
        Assert.That(() => reference.CdnUrl, Is.EqualTo("https://cdn.example.test/tracker/0001.mod"));
        Assert.That(() => reference.Kind, Is.EqualTo(MediaKind.Audio));
        Assert.That(() => reference.DisplayTitle, Is.EqualTo("Tracker Demo"));
        Assert.That(() => reference.DisplaySubtitle, Is.EqualTo("MOD"));
        Assert.That(() => reference.Providers.Single().ProviderKey, Is.EqualTo("modarchive"));
        Assert.That(() => reference.Assets.Single().AssetId, Is.EqualTo("cdn:tracker:0001"));
        Assert.That(() => reference.Metadata["pilot"], Is.EqualTo("tracker-music"));
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
                    EntityId = "catalog:sample-show-episode-20",
                    Kind = MediaRelationshipEndpointKind.CatalogItem,
                    Label = "Sample Episode"
                },
                Target = new MediaRelationshipEndpoint
                {
                    EntityId = "media:library:123",
                    Kind = MediaRelationshipEndpointKind.MediaFile,
                    Label = "Sample Show - Sample Episode [S02E20].mp4",
                    RelativePath = "TV Shows/Sample Show/Sample Show - Sample Episode [S02E20].mp4"
                },
                Metadata = new Dictionary<string, string>
                {
                    ["source"] = "sample-show",
                }
            },
            new MediaRelationship
            {
                RelationshipId = "rel:show-poster",
                Kind = MediaRelationshipKind.PosterFor,
                Source = new MediaRelationshipEndpoint
                {
                    EntityId = "catalog:sample-show",
                    Kind = MediaRelationshipEndpointKind.CatalogItem,
                    Label = "Sample Show"
                },
                Target = new MediaRelationshipEndpoint
                {
                    EntityId = "asset:poster:sample-episode",
                    Kind = MediaRelationshipEndpointKind.MediaAsset,
                    Label = "Sample Episode Poster",
                    Url = "https://cdn.example.com/posters/sample-episode.jpg"
                }
            },
            new MediaRelationship
            {
                RelationshipId = "rel:rom-box-art",
                Kind = MediaRelationshipKind.BoxArtFor,
                Source = new MediaRelationshipEndpoint
                {
                    EntityId = "rom:example-game",
                    Kind = MediaRelationshipEndpointKind.MediaFile,
                    Label = "Example Game.sfc"
                },
                Target = new MediaRelationshipEndpoint
                {
                    EntityId = "asset:box-art:example-game",
                    Kind = MediaRelationshipEndpointKind.MediaAsset,
                    Label = "Example Game Box Art",
                    Url = "https://cdn.example.com/box-art/example-game.png"
                }
            }
        };

        var json = JsonSerializer.Serialize(relationships);
        var roundTrip = JsonSerializer.Deserialize<MediaRelationship[]>(json);

        Assert.That(() => roundTrip, Is.Not.Null);
        Assert.That(() => roundTrip, Has.Length.EqualTo(3));
        Assert.That(() => roundTrip![0].Kind, Is.EqualTo(MediaRelationshipKind.EpisodeOf));
        Assert.That(() => roundTrip[0].Source.Kind, Is.EqualTo(MediaRelationshipEndpointKind.CatalogItem));
        Assert.That(() => roundTrip[0].Target.RelativePath, Is.EqualTo("TV Shows/Sample Show/Sample Show - Sample Episode [S02E20].mp4"));
        Assert.That(() => roundTrip[1].Kind, Is.EqualTo(MediaRelationshipKind.PosterFor));
        Assert.That(() => roundTrip[1].Target.Kind, Is.EqualTo(MediaRelationshipEndpointKind.MediaAsset));
        Assert.That(() => roundTrip[2].Kind, Is.EqualTo(MediaRelationshipKind.BoxArtFor));
        Assert.That(() => roundTrip[2].Target.Url, Is.EqualTo("https://cdn.example.com/box-art/example-game.png"));
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

        Assert.That(() => validation.IsValid, Is.False);
        Assert.That(() => validation.Errors, Has.Some.EqualTo("Playlist name is required."));
        Assert.That(() => validation.Errors, Has.Some.EqualTo("Playback start index must point to an existing playlist item."));
        Assert.That(() => validation.Errors, Has.Some.EqualTo("Playlist item 0 must have a title."));
        Assert.That(() => validation.Errors, Has.Some.EqualTo("Playlist item 0 must reference media, a media id, or a path."));
        Assert.That(() => validation.Errors, Has.Some.EqualTo("Playlist item 0 weight must be greater than zero."));
        Assert.That(() => validation.Errors, Has.Some.EqualTo("Playlist item 0 duration must be greater than zero."));
        Assert.That(() => validation.Errors, Has.Some.EqualTo("Playlist item 0 start offset cannot be negative."));
    }
}
