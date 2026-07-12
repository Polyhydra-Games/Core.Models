using System;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Technical metadata for a local media file.
/// </summary>
public sealed record MediaTechnicalMetadata
{
    /// <summary>
    /// Media duration when available.
    /// </summary>
    public TimeSpan? Duration { get; init; }

    /// <summary>
    /// Video stream metadata, when available.
    /// </summary>
    public VideoMediaMetadata? Video { get; init; }

    /// <summary>
    /// Audio stream metadata, when available.
    /// </summary>
    public AudioMediaMetadata? Audio { get; init; }

    /// <summary>
    /// Image metadata, when available.
    /// </summary>
    public ImageMediaMetadata? Image { get; init; }

    /// <summary>
    /// ROM metadata, when available.
    /// </summary>
    public RomMediaMetadata? Rom { get; init; }
}
