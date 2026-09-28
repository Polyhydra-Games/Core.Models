
using NUnit.Framework;
using PolyhydraGames.Core.Models;

namespace PolyhydraGames.Core.Models.Test;

public class ContentTypeTests
{
    [
        TestCase(ContentType.TextHtml, "text/html"),
        TestCase(ContentType.TextPlain, "text/plain"),
        TestCase(ContentType.TextXml, "text/xml"),
        TestCase(ContentType.Json, "application/json"),
        TestCase(ContentType.Xml, "application/xml"),
        TestCase(ContentType.Pdf, "application/pdf"),
        TestCase(ContentType.ImageJpeg, "image/jpeg"),
        TestCase(ContentType.ImagePng, "image/png"),
        TestCase(ContentType.AudioMpeg, "audio/mpeg"),
        TestCase(ContentType.AudioOgg, "audio/ogg"),
        TestCase(ContentType.VideoMp4, "video/mp4"),
        TestCase(ContentType.VideoWebm, "video/webm"),
        TestCase(ContentType.MultipartFormData, "multipart/form-data"),
        TestCase(ContentType.WwwFormUrlencoded, "application/x-www-form-urlencoded"),

    // Add more content types as needed

    ]
    public void ContentTypeEnumProducesrightStringType(ContentType type, string expected)
    {
        Assert.That(type.ToContentTypeString() == expected);
    }
}