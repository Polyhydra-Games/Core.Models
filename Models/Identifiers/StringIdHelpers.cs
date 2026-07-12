namespace PolyhydraGames.Core.Models.Identifiers;

/// <summary>
/// Shared helpers for string-backed identifier values.
/// </summary>
public static class StringIdHelpers
{
    /// <summary>
    /// Normalizes an optional string identifier value by trimming non-empty values and returning empty for null or whitespace.
    /// </summary>
    public static string NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
    }

    /// <summary>
    /// Returns true when a string identifier value should be treated as missing.
    /// </summary>
    public static bool IsMissing(string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }
}
