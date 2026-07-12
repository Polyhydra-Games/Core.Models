namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// ROM-oriented metadata for emulation libraries.
/// </summary>
public sealed record RomMediaMetadata
{
    /// <summary>
    /// Platform or system name supplied by the owning application.
    /// </summary>
    public string? Platform { get; init; }

    /// <summary>
    /// Region value supplied by the owning application.
    /// </summary>
    public string? Region { get; init; }
}
