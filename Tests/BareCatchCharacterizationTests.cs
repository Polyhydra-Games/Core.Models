using System;
using System.Collections.Generic;
using System.Text.Json;
using NUnit.Framework;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Models.Identifiers;
using PolyhydraGames.Core.Models.Matrix;
using PolyhydraGames.Core.Models.Serialization;

namespace PolyhydraGames.Core.Models.Test;

/// <summary>Characterization tests for Core.Models#47 finding 5 (formerly bare catch blocks).</summary>
[TestFixture]
public sealed class BareCatchCharacterizationTests
{
    // --- StronglyTypedId.Try* ---

    [Test]
    public void TryParseGuid_ReturnsFalse_WhenCtorThrowsFormatOrArgument()
    {
        Assert.That(StronglyTypedId.TryParseGuid<ThrowingGuidId>(Guid.Empty.ToString(), out var a), Is.False);
        Assert.That(a, Is.EqualTo(default(ThrowingGuidId)));
        Assert.That(StronglyTypedId.TryParseGuid<ThrowingGuidId>("not-a-guid", out _), Is.False);
        Assert.That(StronglyTypedId.TryParseGuid<ThrowingGuidId>(Guid.NewGuid().ToString(), out var ok), Is.True);
        Assert.That(ok.Value, Is.Not.EqualTo(Guid.Empty));
    }

    [Test]
    public void TryFromString_ReturnsFalse_WhenCtorThrowsFormatOrArgument()
    {
        Assert.That(StronglyTypedId.TryFromString<ThrowingStringId>("format", out _), Is.False);
        Assert.That(StronglyTypedId.TryFromString<ThrowingStringId>("arg", out _), Is.False);
        Assert.That(StronglyTypedId.TryFromString<ThrowingStringId>(null, out _), Is.False);
        Assert.That(StronglyTypedId.TryFromString<ThrowingStringId>("fine", out var ok), Is.True);
        Assert.That(ok.Value, Is.EqualTo("fine"));
    }

    [Test]
    public void TryParseInt32_And_Int64_ReturnFalse_WhenCtorThrows()
    {
        Assert.That(StronglyTypedId.TryParseInt32<ThrowingIntId>("0", out _), Is.False);
        Assert.That(StronglyTypedId.TryParseInt32<ThrowingIntId>("-1", out _), Is.False);
        Assert.That(StronglyTypedId.TryParseInt32<ThrowingIntId>("abc", out _), Is.False);
        Assert.That(StronglyTypedId.TryParseInt32<ThrowingIntId>("5", out var i), Is.True);
        Assert.That(i.Value, Is.EqualTo(5));

        Assert.That(StronglyTypedId.TryParseInt64<ThrowingLongId>("0", out _), Is.False);
        Assert.That(StronglyTypedId.TryParseInt64<ThrowingLongId>("-1", out _), Is.False);
        Assert.That(StronglyTypedId.TryParseInt64<ThrowingLongId>("5", out var l), Is.True);
        Assert.That(l.Value, Is.EqualTo(5L));
    }

    // --- SystemTextJsonHelper.TrySerializeObject ---

    [Test]
    public void TrySerializeObject_ReturnsSuccess_ForValidObject()
    {
        var (success, value) = new SystemTextJsonHelper().TrySerializeObject(new Dictionary<string, int> { ["a"] = 1 });
        Assert.That(success, Is.True);
        Assert.That(value, Is.EqualTo("{\"a\":1}"));
    }

    [Test]
    public void TrySerializeObject_ReturnsFailure_ForCycle_AndUnsupportedType()
    {
        var node = new Node();
        node.Next = node;
        Assert.That(new SystemTextJsonHelper().TrySerializeObject(node), Is.EqualTo((false, (string?)null)));
        Assert.That(new SystemTextJsonHelper().TrySerializeObject(typeof(string)), Is.EqualTo((false, (string?)null)));
    }

    // --- MatrixDisplaySettingsSchema.ValidateRoundTrip ---

    [Test]
    public void ValidateRoundTrip_ReturnsFalse_ForInvalidInputs()
    {
        Assert.That(MatrixDisplaySettingsSchema.ValidateRoundTrip("not json"), Is.False);
        Assert.That(MatrixDisplaySettingsSchema.ValidateRoundTrip(""), Is.False);
        Assert.That(MatrixDisplaySettingsSchema.ValidateRoundTrip("null"), Is.False);
        Assert.That(MatrixDisplaySettingsSchema.ValidateRoundTrip("[1,2]"), Is.False);
        Assert.That(MatrixDisplaySettingsSchema.ValidateRoundTrip(null!), Is.False);
    }

    // --- FromCSV<T> continueOnError ---

    [Test]
    public void FromCsv_Generic_Throws_WhenContinueOnErrorIsFalse()
    {
        var previous = CSVExtensions.Provider;
        CSVExtensions.Provider = new SystemTextJsonHelper();
        try
        {
            Assert.Throws<JsonException>(() => "invalid".FromCSV<Payload>(continueOnError: false));
        }
        finally { CSVExtensions.Provider = previous; }
    }

    [Test]
    public void FromCsv_Generic_Swallows_WhenContinueOnErrorIsTrue()
    {
        var previous = CSVExtensions.Provider;
        CSVExtensions.Provider = new SystemTextJsonHelper();
        try
        {
            Assert.That(() => "invalid".FromCSV<Payload>(continueOnError: true), Throws.Nothing);
            Assert.That(() => "invalid".FromCSV<Payload>(), Throws.Nothing);
        }
        finally { CSVExtensions.Provider = previous; }
    }

    private sealed class Payload { }

    private sealed class Node { public Node? Next { get; set; } }

    private readonly struct ThrowingGuidId
    {
        public ThrowingGuidId(Guid value)
        {
            if (value == Guid.Empty) throw new FormatException("empty");
            Value = value;
        }
        public Guid Value { get; }
    }

    private readonly struct ThrowingStringId
    {
        public ThrowingStringId(string? value)
        {
            if (value == "format") throw new FormatException();
            if (value == "arg") throw new ArgumentException("arg");
            ArgumentNullException.ThrowIfNull(value);
            Value = value;
        }
        public string Value { get; }
    }

    private readonly struct ThrowingIntId
    {
        public ThrowingIntId(int value)
        {
            ArgumentOutOfRangeException.ThrowIfZero(value);
            if (value < 0) throw new FormatException();
            Value = value;
        }
        public int Value { get; }
    }

    private readonly struct ThrowingLongId
    {
        public ThrowingLongId(long value)
        {
            if (value == 0) throw new ArgumentException("zero");
            if (value < 0) throw new FormatException();
            Value = value;
        }
        public long Value { get; }
    }
}
