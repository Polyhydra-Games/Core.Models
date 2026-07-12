namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Video stream metadata extracted from a local media file.
/// </summary>
public sealed record VideoMediaMetadata
{
    /// <summary>
    /// Codec name reported for the video stream.
    /// </summary>
    public string? Codec { get; init; }

    /// <summary>
    /// Container format reported for the file.
    /// </summary>
    public string? Container { get; init; }

    /// <summary>
    /// Video width in pixels, when known.
    /// </summary>
    public int? Width { get; init; }

    /// <summary>
    /// Video height in pixels, when known.
    /// </summary>
    public int? Height { get; init; }

    /// <summary>
    /// Video frame rate, when known.
    /// </summary>
    public double? Framerate { get; init; }

    /// <summary>
    /// Encoded video bitrate in bits per second, when known.
    /// </summary>
    public long? BitrateBitsPerSecond { get; init; }
}
