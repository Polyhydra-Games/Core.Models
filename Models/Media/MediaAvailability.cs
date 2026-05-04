namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Availability state for media discovered from a local or network-backed root.
/// </summary>
public enum MediaAvailability
{
    /// <summary>
    /// Availability has not been checked or cannot be represented.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// The file is present and readable from the indexing context.
    /// </summary>
    Available = 1,

    /// <summary>
    /// The file was previously indexed but is no longer present.
    /// </summary>
    Missing = 2,

    /// <summary>
    /// The file or root exists but cannot currently be accessed.
    /// </summary>
    Inaccessible = 3,

    /// <summary>
    /// The file is intentionally archived outside the active library.
    /// </summary>
    Archived = 4
}
