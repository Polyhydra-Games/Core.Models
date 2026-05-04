namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Audio stream metadata extracted from a local media file.
/// </summary>
public sealed record AudioMediaMetadata
{
    /// <summary>
    /// Codec name reported for the audio stream, such as aac, flac, or mp3.
    /// </summary>
    public string? Codec { get; init; }

    /// <summary>
    /// Encoded audio bitrate in bits per second, when known.
    /// </summary>
    public long? BitrateBitsPerSecond { get; init; }

    /// <summary>
    /// Number of audio channels, when known.
    /// </summary>
    public int? Channels { get; init; }

    /// <summary>
    /// Audio sample rate in hertz, when known.
    /// </summary>
    public int? SampleRateHz { get; init; }
}
