using System;

namespace PolyhydraGames.Core.Models.Media;

/// <summary>
/// Portable launch identity for a media item selected by one app and opened by another app.
/// </summary>
public sealed record MediaLaunchTarget
{
    /// <summary>
    /// Stable media root id. The receiving app maps this id to a local base path.
    /// </summary>
    public string RootId { get; init; } = "";

    /// <summary>
    /// Slash-normalized path relative to the root.
    /// </summary>
    public string RelativePath { get; init; } = "";

    /// <summary>
    /// Optional stable id from the source media catalog.
    /// </summary>
    public string? MediaFileId { get; init; }

    /// <summary>
    /// Leaf file name, including extension.
    /// </summary>
    public string FileName { get; init; } = "";

    /// <summary>
    /// Broad media kind for display and app-selection hints.
    /// </summary>
    public MediaKind Kind { get; init; } = MediaKind.Unknown;

    /// <summary>
    /// Optional display title from the source catalog.
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// Optional poster or thumbnail URL used only for operator display.
    /// </summary>
    public string? PosterUrl { get; init; }

    /// <summary>
    /// Optional source catalog id that selected this target.
    /// </summary>
    public string? SourceCatalogId { get; init; }
}

/// <summary>
/// Request to launch a media target in a configured app on a connected device.
/// </summary>
public sealed record MediaAppLaunchRequest
{
    /// <summary>
    /// App mapping id on the receiving device, for example "vlc".
    /// </summary>
    public string AppId { get; init; } = "";

    /// <summary>
    /// Canonical portable identity: media://{escaped-root-id}/{escaped-relative-path}.
    /// </summary>
    public string TargetSlug { get; init; } = "";

    /// <summary>
    /// Expanded target details for display and compatibility.
    /// </summary>
    public MediaLaunchTarget Target { get; init; } = new();

    /// <summary>
    /// Optional connected device id to route the launch request.
    /// </summary>
    public string? DeviceId { get; init; }
}

/// <summary>
/// Result returned by a media app launcher or resident agent.
/// </summary>
public sealed record MediaAppLaunchResult
{
    public string Operation { get; init; } = "";
    public bool Success { get; init; }
    public string Message { get; init; } = "";
    public DateTimeOffset TimestampUtc { get; init; } = DateTimeOffset.UtcNow;
    public string TargetSlug { get; init; } = "";
    public string? ResolvedPath { get; init; }
    public string AppId { get; init; } = "";
}
