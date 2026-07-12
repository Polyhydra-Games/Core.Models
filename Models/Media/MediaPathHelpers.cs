using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Dependency-free helpers for normalizing local media paths and extensions.
/// </summary>
public static class MediaPathHelpers
{
    private static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp4", ".mkv", ".avi", ".mov", ".wmv", ".m4v", ".webm", ".mpeg", ".mpg"
    };

    private static readonly HashSet<string> AudioExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".mp3", ".flac", ".m4a", ".wav", ".ogg", ".aac", ".wma"
    };

    private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".gif", ".bmp", ".tif", ".tiff", ".svg"
    };

    private static readonly HashSet<string> RomExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".nes", ".sfc", ".smc", ".gb", ".gbc", ".gba", ".n64", ".z64", ".v64", ".iso", ".cue", ".chd", ".bin", ".md", ".gen"
    };

    private static readonly HashSet<string> DocumentExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".pdf", ".txt", ".md", ".nfo", ".srt", ".ass", ".ssa", ".sub"
    };

    /// <summary>
    /// Trims an extension, lowercases it, and ensures it starts with a dot.
    /// </summary>
    public static string NormalizeExtension(string? extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
            return "";

        var trimmed = extension.Trim();
        if (trimmed.Length == 0)
            return "";

        return trimmed.StartsWith('.')
            ? trimmed.ToLowerInvariant()
            : $".{trimmed.ToLowerInvariant()}";
    }

    /// <summary>
    /// Trims a relative path, normalizes separators, and removes leading slashes.
    /// </summary>
    public static string NormalizeRelativePath(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return "";

        return relativePath
            .Trim()
            .Replace('\\', '/')
            .TrimStart('/');
    }

    /// <summary>
    /// Classifies a file extension into a broad media kind.
    /// </summary>
    public static MediaKind ClassifyByExtension(string? extension)
    {
        var normalized = NormalizeExtension(extension);

        if (VideoExtensions.Contains(normalized))
            return MediaKind.Video;

        if (AudioExtensions.Contains(normalized))
            return MediaKind.Audio;

        if (ImageExtensions.Contains(normalized))
            return MediaKind.Image;

        if (RomExtensions.Contains(normalized))
            return MediaKind.Rom;

        if (DocumentExtensions.Contains(normalized))
            return MediaKind.Document;

        return string.IsNullOrWhiteSpace(normalized) ? MediaKind.Unknown : MediaKind.Other;
    }
}
