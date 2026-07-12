using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models.Media.Playlists;

/// <summary>
/// Validation result for a portable playlist contract.
/// </summary>
public sealed record MediaPlaylistValidationResult
{
    /// <summary>
    /// True when no validation errors were found.
    /// </summary>
    public bool IsValid => Errors.Count == 0;

    /// <summary>
    /// Validation errors.
    /// </summary>
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();
}
