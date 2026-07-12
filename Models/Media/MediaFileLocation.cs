namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Storage and addressing information for a discovered media file.
/// </summary>
public sealed record MediaFileLocation
{
    /// <summary>
    /// Stable identifier for the scan root that owns this location.
    /// </summary>
    public string? RootId { get; init; }

    /// <summary>
    /// Human-readable root name.
    /// </summary>
    public string? RootName { get; init; }

    /// <summary>
    /// Slash-normalized path relative to the scan root.
    /// </summary>
    public string RelativePath { get; init; } = "";

    /// <summary>
    /// Absolute local path when it is safe and useful to expose.
    /// </summary>
    public string? FullPath { get; init; }

    /// <summary>
    /// Optional externally reachable URL supplied by a delivery layer.
    /// </summary>
    public string? PublicUrl { get; init; }
}
