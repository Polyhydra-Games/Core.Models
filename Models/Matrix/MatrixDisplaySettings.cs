using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PolyhydraGames.Core.Models.Matrix;

/// <summary>
/// Layout positions for matrix word overlays
/// </summary>
public enum MatrixLayout
{
    Top,
    Bottom,
    Full
}

/// <summary>
/// Visual style presets for matrix display
/// </summary>
public enum MatrixStyle
{
    Classic,
    Neon,
    Retro
}

/// <summary>
/// Shared settings contract for matrix word overlay display.
/// Used by both ChannelCheevos and cc-client applications.
/// </summary>
public class MatrixDisplaySettings
{
    /// <summary>
    /// List of words to display in the matrix rain overlay
    /// </summary>
    [JsonPropertyName("words")]
    public List<string> Words { get; set; } = [];

    /// <summary>
    /// Speed of the matrix rain animation in milliseconds per character
    /// </summary>
    [JsonPropertyName("speed")]
    public int Speed { get; set; } = 50;

    /// <summary>
    /// Density of characters per frame (1-10 scale)
    /// </summary>
    [JsonPropertyName("density")]
    public int Density { get; set; } = 3;

    /// <summary>
    /// Visual style preset for the matrix display
    /// </summary>
    [JsonPropertyName("style")]
    public MatrixStyle Style { get; set; } = MatrixStyle.Classic;

    /// <summary>
    /// Layout position for word overlay
    /// </summary>
    [JsonPropertyName("layout")]
    public MatrixLayout Layout { get; set; } = MatrixLayout.Bottom;

    /// <summary>
    /// Version number for future schema migrations
    /// </summary>
    [JsonPropertyName("version")]
    public int Version { get; set; } = 1;

    /// <summary>
    /// Creates a default configuration with sample words
    /// </summary>
    public static MatrixDisplaySettings CreateDefault()
    {
        return new MatrixDisplaySettings
        {
            Words = ["POLYHYDRA", "GAMES", "STREAM", "LIVE", "CODE", "GAME"],
            Speed = 50,
            Density = 3,
            Style = MatrixStyle.Neon,
            Layout = MatrixLayout.Bottom,
            Version = 1
        };
    }
}

/// <summary>
/// JSON Schema documentation for MatrixDisplaySettings
/// </summary>
public static class MatrixDisplaySettingsSchema
{
    /// <summary>
    /// Returns the JSON schema for MatrixDisplaySettings persistence
    /// </summary>
    public static string GetSchema() => """
        {
          "$schema": "http://json-schema.org/draft-07/schema#",
          "title": "MatrixDisplaySettings",
          "type": "object",
          "properties": {
            "words": {
              "type": "array",
              "items": { "type": "string" },
              "description": "List of words to display in matrix rain overlay"
            },
            "speed": {
              "type": "integer",
              "minimum": 10,
              "maximum": 200,
              "default": 50,
              "description": "Milliseconds per character"
            },
            "density": {
              "type": "integer",
              "minimum": 1,
              "maximum": 10,
              "default": 3,
              "description": "Characters per frame"
            },
            "style": {
              "type": "string",
              "enum": ["Classic", "Neon", "Retro"],
              "default": "Neon"
            },
            "layout": {
              "type": "string",
              "enum": ["Top", "Bottom", "Full"],
              "default": "Bottom"
            },
            "version": {
              "type": "integer",
              "minimum": 1,
              "default": 1,
              "description": "Schema version for migration"
            }
          }
        }
        """;

    /// <summary>
    /// Validates that a JSON string can be deserialized and serialized back to the same value
    /// </summary>
    public static bool ValidateRoundTrip(string json)
    {
        try
        {
            var settings = JsonSerializer.Deserialize<MatrixDisplaySettings>(json);
            if (settings == null) return false;

            var reSerialized = JsonSerializer.Serialize(settings);
            var reDeserialized = JsonSerializer.Deserialize<MatrixDisplaySettings>(reSerialized);

            return reDeserialized != null &&
                   JsonSerializer.Serialize(reDeserialized) == reSerialized;
        }
        catch
        {
            return false;
        }
    }
}
