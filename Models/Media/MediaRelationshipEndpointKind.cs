namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Broad entity category for a relationship endpoint.
/// </summary>
public enum MediaRelationshipEndpointKind
{
    Unknown = 0,
    MediaFile = 1,
    MediaAsset = 2,
    CatalogItem = 3,
    ProviderRecord = 4,
    PlaylistItem = 5,
    ScanRoot = 6,
    External = 7
}
