using NUnit.Framework;
using PolyhydraGames.Core.Models.Matrix;
using System.Text.Json;

namespace PolyhydraGames.Core.Models.Tests;

public class MatrixDisplaySettingsTests
{
    [Test]
    public void Default_CreatesValidSettings()
    {
        var settings = MatrixDisplaySettings.CreateDefault();

        Assert.That(settings, Is.Not.Null);
        Assert.That(settings.Words, Is.Not.Empty);
        Assert.That(settings.Speed, Is.InRange(10, 200));
        Assert.That(settings.Density, Is.InRange(1, 10));
        Assert.That(settings.Version, Is.EqualTo(1));
    }

    [Test]
    public void Serialization_RoundTrip_PreservesValues()
    {
        var original = new MatrixDisplaySettings
        {
            Words = new List<string> { "HELLO", "WORLD" },
            Speed = 75,
            Density = 5,
            Style = MatrixStyle.Retro,
            Layout = MatrixLayout.Top,
            Version = 1
        };

        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<MatrixDisplaySettings>(json);

        Assert.That(deserialized, Is.Not.Null);
        Assert.That(deserialized!.Words.Count, Is.EqualTo(original.Words.Count));
        Assert.That(deserialized.Speed, Is.EqualTo(original.Speed));
        Assert.That(deserialized.Density, Is.EqualTo(original.Density));
        Assert.That(deserialized.Style, Is.EqualTo(original.Style));
        Assert.That(deserialized.Layout, Is.EqualTo(original.Layout));
        Assert.That(deserialized.Version, Is.EqualTo(original.Version));
    }

    [Test]
    public void Schema_ValidateRoundTrip_ReturnsTrue()
    {
        var json = JsonSerializer.Serialize(MatrixDisplaySettings.CreateDefault());
        var result = MatrixDisplaySettingsSchema.ValidateRoundTrip(json);

        Assert.That(result, Is.True);
    }

    [Test]
    public void Schema_GetSchema_ReturnsValidJson()
    {
        var schema = MatrixDisplaySettingsSchema.GetSchema();

        Assert.That(schema, Is.Not.Null);
        Assert.That(schema, Does.Contain("MatrixDisplaySettings"));
        Assert.That(schema, Does.Contain("words"));
        Assert.That(schema, Does.Contain("speed"));
        Assert.That(schema, Does.Contain("version"));
    }
}
