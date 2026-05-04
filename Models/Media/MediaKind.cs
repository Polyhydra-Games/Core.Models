namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Broad media categories used when indexing local files.
/// </summary>
public enum MediaKind
{
    /// <summary>
    /// File type is not known.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Video file.
    /// </summary>
    Video = 1,

    /// <summary>
    /// Audio file.
    /// </summary>
    Audio = 2,

    /// <summary>
    /// Image file.
    /// </summary>
    Image = 3,

    /// <summary>
    /// Game ROM or disc image file.
    /// </summary>
    Rom = 4,

    /// <summary>
    /// Document, subtitle, or text companion file.
    /// </summary>
    Document = 5,

    /// <summary>
    /// Known extension that does not fit a supported category.
    /// </summary>
    Other = 99
}
