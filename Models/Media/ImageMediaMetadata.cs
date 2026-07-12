using System;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Image metadata extracted from a local media file.
/// </summary>
public sealed record ImageMediaMetadata
{
    /// <summary>
    /// Image width in pixels, when known.
    /// </summary>
    public int? Width { get; init; }

    /// <summary>
    /// Image height in pixels, when known.
    /// </summary>
    public int? Height { get; init; }

    /// <summary>
    /// Camera model captured from image metadata, when available.
    /// </summary>
    public string? CameraModel { get; init; }

    /// <summary>
    /// Original capture timestamp, when available.
    /// </summary>
    public DateTimeOffset? DateTaken { get; init; }
}
