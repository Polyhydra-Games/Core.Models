using PolyhydraGames.Core.Models.Attributes;

namespace PolyhydraGames.Core.Models;

public enum ContentType
{
    [StringValue("application/java-archive")]
    JavaArchive,

    [StringValue("application/EDI-X12")]
    EdiX12,

    [StringValue("application/EDIFACT")]
    Edifact,

    [StringValue("application/javascript")]
    JavascriptObsolete,

    [StringValue("application/octet-stream")]
    OctetStream,

    [StringValue("audio/ogg")]
    AudioOgg,
    [StringValue("video/ogg")]
    VideoOgg,

    [StringValue("application/pdf")]
    Pdf,

    [StringValue("application/xhtml+xml")]
    XhtmlXml,

    [StringValue("application/x-shockwave-flash")]
    ShockwaveFlash,

    [StringValue("application/json")]
    Json,

    [StringValue("application/ld+json")]
    LdJson,

    [StringValue("application/xml")]
    Xml,

    [StringValue("application/zip")]
    Zip,

    [StringValue("application/x-www-form-urlencoded")]
    WwwFormUrlencoded,

    // Type audio
    [StringValue("audio/mpeg")]
    AudioMpeg,

    [StringValue("audio/x-ms-wma")]
    AudioWma,

    [StringValue("audio/vnd.rn-realaudio")]
    AudioRealAudio,

    [StringValue("audio/x-wav")]
    AudioWav,

    // Type image
    [StringValue("image/gif")]
    ImageGif,

    [StringValue("image/jpeg")]
    ImageJpeg,

    [StringValue("image/png")]
    ImagePng,

    [StringValue("image/tiff")]
    ImageTiff,

    [StringValue("image/vnd.microsoft.icon")]
    ImageMicrosoftIcon,

    [StringValue("image/x-icon")]
    ImageIcon,

    [StringValue("image/vnd.djvu")]
    ImageDjvu,

    [StringValue("image/svg+xml")]
    ImageSvgXml,

    // Type multipart
    [StringValue("multipart/mixed")]
    MultipartMixed,

    [StringValue("multipart/alternative")]
    MultipartAlternative,

    [StringValue("multipart/related")]
    MultipartRelated,

    [StringValue("multipart/form-data")]
    MultipartFormData,

    // Type text
    [StringValue("text/css")]
    TextCss,

    [StringValue("text/csv")]
    TextCsv,

    [StringValue("text/html")]
    TextHtml,

    [StringValue("text/javascript")]
    TextJavascript,

    [StringValue("text/plain")]
    TextPlain,

    [StringValue("text/xml")]
    TextXml,

    // Type video
    [StringValue("video/mpeg")]
    VideoMpeg,

    [StringValue("video/mp4")]
    VideoMp4,

    [StringValue("video/quicktime")]
    VideoQuicktime,

    [StringValue("video/x-ms-wmv")]
    VideoWmv,

    [StringValue("video/x-msvideo")]
    VideoXmsvideo,

    [StringValue("video/x-flv")]
    VideoFlv,

    [StringValue("video/webm")]
    VideoWebm,

    // Type vnd
    [StringValue("application/vnd.android.package-archive")]
    VndAndroidPackageArchive,

    [StringValue("application/vnd.oasis.opendocument.text")]
    VndOasisOpendocumentText,

    [StringValue("application/vnd.oasis.opendocument.spreadsheet")]
    VndOasisOpendocumentSpreadsheet,

    [StringValue("application/vnd.oasis.opendocument.presentation")]
    VndOasisOpendocumentPresentation,

    [StringValue("application/vnd.oasis.opendocument.graphics")]
    VndOasisOpendocumentGraphics,

    [StringValue("application/vnd.ms-excel")]
    VndMsExcel,

    [StringValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
    VndOpenxmlformatsSpreadsheetmlSheet,

    [StringValue("application/vnd.ms-powerpoint")]
    VndMsPowerpoint,

    [StringValue("application/vnd.openxmlformats-officedocument.presentationml.presentation")]
    VndOpenxmlformatsPresentationmlPresentation,

    [StringValue("application/msword")]
    VndMsword,

    [StringValue("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
    VndOpenxmlformatsWordprocessingmlDocument,

    [StringValue("application/vnd.mozilla.xul+xml")]
    VndMozillaXulXml,
}
