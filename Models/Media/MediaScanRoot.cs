using System;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// A storage root that can be scanned for media files.
/// </summary>
public sealed record MediaScanRoot
{
    /// <summary>
    /// Stable root identifier assigned by the owning application.
    /// </summary>
    public string RootId { get; init; } = "";

    /// <summary>
    /// Human-readable root name.
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// Hostname or device name for network-backed roots, when known.
    /// </summary>
    public string? Hostname { get; init; }

    /// <summary>
    /// Base filesystem path for this scan root.
    /// </summary>
    public string BasePath { get; init; } = "";

    /// <summary>
    /// Indicates whether this root should be included in active scans.
    /// </summary>
    public bool IsActive { get; init; } = true;

    /// <summary>
    /// Timestamp when the root definition was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}
