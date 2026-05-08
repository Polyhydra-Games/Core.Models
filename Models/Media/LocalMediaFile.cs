using System;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Canonical shared shape for a media file discovered from local or NAS-backed storage.
/// </summary>
public sealed record LocalMediaFile
{
    /// <summary>
    /// Optional stable identifier from the owning application or index.
    /// </summary>
    public string? MediaFileId { get; init; }

    /// <summary>
    /// Leaf file name, including extension.
    /// </summary>
    public string FileName { get; init; } = "";

    /// <summary>
    /// Storage location details for the file.
    /// </summary>
    public MediaFileLocation Location { get; init; } = new();

    /// <summary>
    /// File size in bytes.
    /// </summary>
    public long FileSizeBytes { get; init; }

    /// <summary>
    /// Normalized file extension, including the leading dot when present.
    /// </summary>
    public string Extension { get; init; } = "";

    /// <summary>
    /// Broad media category inferred by the scanner or caller.
    /// </summary>
    public MediaKind Kind { get; init; } = MediaKind.Unknown;

    /// <summary>
    /// Current known availability state for the file.
    /// </summary>
    public MediaAvailability Availability { get; init; } = MediaAvailability.Unknown;

    /// <summary>
    /// Last modified timestamp from the backing filesystem, when known.
    /// </summary>
    public DateTimeOffset? LastModifiedAt { get; init; }

    /// <summary>
    /// Timestamp when the file was indexed by the owning application.
    /// </summary>
    public DateTimeOffset? IndexedAt { get; init; }

    /// <summary>
    /// Optional checksum value supplied by the owning application.
    /// </summary>
    public string? Checksum { get; init; }

    /// <summary>
    /// Optional technical metadata extracted from media-specific tooling.
    /// </summary>
    public MediaTechnicalMetadata? TechnicalMetadata { get; init; }

    /// <summary>
    /// Optional provider or catalog enrichment data for the media file.
    /// </summary>
    public MediaEnrichmentMetadata? Enrichment { get; init; }
}
