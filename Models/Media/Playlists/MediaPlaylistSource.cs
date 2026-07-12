using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Portable description of the media source used to build or resolve a playlist.
/// </summary>
public sealed record MediaPlaylistSource
{
    /// <summary>
    /// Optional stable source identifier from the owning application.
    /// </summary>
    public string? SourceId { get; init; }

    /// <summary>
    /// Human-readable source name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Optional related media scan root identifier.
    /// </summary>
    public string? MediaScanRootId { get; init; }

    /// <summary>
    /// Optional root path used by a generator or local runner.
    /// </summary>
    public string? RootPath { get; init; }

    /// <summary>
    /// Whether the source should be scanned recursively.
    /// </summary>
    public bool? Recursive { get; init; }

    /// <summary>
    /// Optional file patterns used by a generator.
    /// </summary>
    public IReadOnlyList<string> FilePatterns { get; init; } = Array.Empty<string>();

    /// <summary>
    /// Optional paths or patterns excluded by a generator.
    /// </summary>
    public IReadOnlyList<string> Exclusions { get; init; } = Array.Empty<string>();
}
