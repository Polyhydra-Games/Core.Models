namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Broad category for a shared media asset reference.
/// </summary>
public enum MediaAssetReferenceKind
{
    /// <summary>
    /// Asset is backed by a local or NAS-discovered media file.
    /// </summary>
    LocalFile = 1,

    /// <summary>
    /// Asset is delivered from a CDN or similar public delivery layer.
    /// </summary>
    CdnAsset = 2,

    /// <summary>
    /// Asset refers to an image or poster supplied by a provider.
    /// </summary>
    ProviderImage = 3,

    /// <summary>
    /// Asset is a poster or box-art style image.
    /// </summary>
    Poster = 4,

    /// <summary>
    /// Asset is a trailer or preview clip.
    /// </summary>
    Trailer = 5,

    /// <summary>
    /// Asset is a subtitle or caption sidecar.
    /// </summary>
    Subtitle = 6,

    /// <summary>
    /// Asset is a companion file that does not fit a narrower role.
    /// </summary>
    Companion = 7,

    /// <summary>
    /// Asset is a generic external URL reference.
    /// </summary>
    External = 99
}
