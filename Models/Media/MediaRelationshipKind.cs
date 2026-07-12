namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Broad directional relationship label between two shared media entities.
/// </summary>
public enum MediaRelationshipKind
{
    Unknown = 0,
    Related = 1,
    Contains = 2,
    DerivedFrom = 3,
    EpisodeOf = 4,
    PosterFor = 5,
    TrailerFor = 6,
    SubtitleFor = 7,
    CompanionFor = 8,
    BoxArtFor = 9,
    PreviewOf = 10,
    VariantOf = 11,
    CanonicalFor = 12
}
