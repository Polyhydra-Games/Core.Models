using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using NUnit.Framework;
using PolyhydraGames.Core.Models.Identifiers;

namespace PolyhydraGames.Core.Models.Tests;

public class StronglyTypedIdentifierTests
{
    [Test]
    public void GuidBackedId_RoundTripsAsJsonString()
    {
        var id = TestIdentityId.New();
        var json = JsonSerializer.Serialize(id);
        var roundTrip = JsonSerializer.Deserialize<TestIdentityId>(json);

        Assert.That(roundTrip, Is.EqualTo(id));
        Assert.That(json, Does.Contain(id.ToString()));
    }

    [Test]
    public void GuidBackedId_CanUseFactoryRegistration()
    {
        var options = CreateFactoryOptions();
        var id = TestFactoryIdentityId.New();
        var json = JsonSerializer.Serialize(id, options);
        var roundTrip = JsonSerializer.Deserialize<TestFactoryIdentityId>(json, options);

        Assert.That(roundTrip, Is.EqualTo(id));
    }

    [Test]
    public void ConverterFactory_SupportsAllBuiltInBackingTypes()
    {
        var options = CreateFactoryOptions();

        Assert.That(JsonSerializer.Deserialize<TestFactoryPlatformId>("\" twitch-user \"", options).Value, Is.EqualTo("twitch-user"));
        Assert.That(JsonSerializer.Deserialize<TestFactoryOptionalPlatformId>("null", options).IsEmpty, Is.True);
        Assert.That(JsonSerializer.Deserialize<TestFactoryOptionalPlatformId>("\"   \"", options).IsEmpty, Is.True);
        Assert.That(JsonSerializer.Deserialize<TestFactoryNumericId>("42", options).Value, Is.EqualTo(42));
        Assert.That(JsonSerializer.Deserialize<TestFactorySnowflakeId>("\"9223372036854775\"", options).Value, Is.EqualTo(9_223_372_036_854_775));

        Assert.That(JsonSerializer.Serialize(new TestFactoryPlatformId("twitch-user"), options), Is.EqualTo("\"twitch-user\""));
        Assert.That(JsonSerializer.Serialize(new TestFactoryOptionalPlatformId("twitch-user"), options), Is.EqualTo("\"twitch-user\""));
        Assert.That(JsonSerializer.Serialize(new TestFactoryNumericId(42), options), Is.EqualTo("42"));
        Assert.That(JsonSerializer.Serialize(new TestFactorySnowflakeId(9_223_372_036_854_775), options), Is.EqualTo("9223372036854775"));
    }

    [Test]
    public void GuidBackedId_HelpersCreateParseAndTryParse()
    {
        var id = StronglyTypedId.NewGuid<TestIdentityId>();
        var parsed = StronglyTypedId.ParseGuid<TestIdentityId>(id.ToString());
        var parsedOk = StronglyTypedId.TryParseGuid<TestIdentityId>(id.ToString(), out var tryParsed);
        var emptyFails = StronglyTypedId.TryParseGuid<TestIdentityId>(Guid.Empty.ToString("D"), out _);

        Assert.That(parsed, Is.EqualTo(id));
        Assert.That(parsedOk, Is.True);
        Assert.That(tryParsed, Is.EqualTo(id));
        Assert.That(emptyFails, Is.False);
        Assert.Throws<FormatException>(() => StronglyTypedId.ParseGuid<TestIdentityId>(Guid.Empty.ToString("D")));
    }

    [Test]
    public void GuidBackedId_RejectsEmptyGuid()
    {
        Assert.Throws<FormatException>(() => _ = (TestIdentityId)Guid.Empty);
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TestIdentityId>("\"00000000-0000-0000-0000-000000000000\""));
    }

    [Test]
    public void GuidBackedIds_DoNotCompareEqualAcrossTypes()
    {
        var value = Guid.NewGuid();
        var identityId = new TestIdentityId(value);
        var otherId = new TestOtherIdentityId(value);

        Assert.That(identityId.Equals(otherId), Is.False);
        Assert.That(otherId.Equals(identityId), Is.False);
        Assert.That((Guid)identityId, Is.EqualTo((Guid)otherId));
    }

    [Test]
    public void StringBackedId_NormalizesWhitespaceToEmpty()
    {
        var empty = (TestPlatformId)"";
        var whitespace = (TestPlatformId)"   ";

        Assert.That(whitespace, Is.EqualTo(empty));
        Assert.That(whitespace.IsEmpty, Is.True);
        Assert.That(whitespace.Value, Is.EqualTo(""));
    }

    [Test]
    public void StringBackedId_RoundTripsAsJsonString()
    {
        var id = (TestPlatformId)" 12345 ";
        var json = JsonSerializer.Serialize(id);
        var roundTrip = JsonSerializer.Deserialize<TestPlatformId>(json);

        Assert.That(json, Is.EqualTo("\"12345\""));
        Assert.That(roundTrip, Is.EqualTo(id));
    }

    [Test]
    public void StringBackedId_HelpersCreateAndTryCreate()
    {
        var id = StronglyTypedId.FromString<TestPlatformId>(" 12345 ");
        var ok = StronglyTypedId.TryFromString<TestPlatformId>(" 12345 ", out var tryParsed);

        Assert.That(id.Value, Is.EqualTo("12345"));
        Assert.That(ok, Is.True);
        Assert.That(tryParsed, Is.EqualTo(id));
    }

    [Test]
    public void StringBackedId_RejectsJsonNull()
    {
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TestPlatformId>("null"));
    }

    [Test]
    public void OptionalStringBackedId_NormalizesMissingValues()
    {
        var nullJson = JsonSerializer.Deserialize<TestOptionalPlatformId>("null");
        var whitespaceJson = JsonSerializer.Deserialize<TestOptionalPlatformId>("\"   \"");
        var helperValue = new TestOptionalPlatformId(StringIdHelpers.NormalizeOptional(" twitch-user "));

        Assert.That(nullJson.IsEmpty, Is.True);
        Assert.That(whitespaceJson.IsEmpty, Is.True);
        Assert.That(helperValue.Value, Is.EqualTo("twitch-user"));
        Assert.That(StringIdHelpers.IsMissing("   "), Is.True);
        Assert.That(StringIdHelpers.IsMissing("twitch-user"), Is.False);
    }

    [Test]
    public void IntBackedId_RoundTripsAsJsonNumber()
    {
        var id = (TestNumericId)42;
        var json = JsonSerializer.Serialize(id);
        var roundTrip = JsonSerializer.Deserialize<TestNumericId>(json);

        Assert.That(json, Is.EqualTo("42"));
        Assert.That(roundTrip, Is.EqualTo(id));
    }

    [Test]
    public void IntBackedId_CanReadIntegerString()
    {
        var id = JsonSerializer.Deserialize<TestNumericId>("\"42\"");

        Assert.That(id.Value, Is.EqualTo(42));
    }

    [Test]
    public void IntBackedId_HelpersParseAndTryParse()
    {
        var id = StronglyTypedId.ParseInt32<TestNumericId>("42");
        var ok = StronglyTypedId.TryParseInt32<TestNumericId>("42", out var tryParsed);
        var zeroFails = StronglyTypedId.TryParseInt32<TestNumericId>("0", out _);

        Assert.That(id.Value, Is.EqualTo(42));
        Assert.That(ok, Is.True);
        Assert.That(tryParsed, Is.EqualTo(id));
        Assert.That(zeroFails, Is.False);
        Assert.Throws<FormatException>(() => StronglyTypedId.ParseInt32<TestNumericId>("0"));
    }

    [Test]
    public void IntBackedId_RejectsZero()
    {
        Assert.Throws<FormatException>(() => _ = (TestNumericId)0);
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<TestNumericId>("0"));
    }

    [Test]
    public void Int64BackedId_RoundTripsAsJsonNumber()
    {
        var id = (TestSnowflakeId)9_223_372_036_854_775;
        var json = JsonSerializer.Serialize(id);
        var roundTrip = JsonSerializer.Deserialize<TestSnowflakeId>(json);

        Assert.That(json, Is.EqualTo("9223372036854775"));
        Assert.That(roundTrip, Is.EqualTo(id));
    }

    [Test]
    public void Int64BackedId_HelpersParseAndTryParse()
    {
        var id = StronglyTypedId.ParseInt64<TestSnowflakeId>("9223372036854775");
        var ok = StronglyTypedId.TryParseInt64<TestSnowflakeId>("9223372036854775", out var tryParsed);
        var negativeFails = StronglyTypedId.TryParseInt64<TestSnowflakeId>("-1", out _);

        Assert.That(id.Value, Is.EqualTo(9_223_372_036_854_775));
        Assert.That(ok, Is.True);
        Assert.That(tryParsed, Is.EqualTo(id));
        Assert.That(negativeFails, Is.False);
        Assert.Throws<FormatException>(() => StronglyTypedId.ParseInt64<TestSnowflakeId>("-1"));
    }

    [Test]
    public void Factory_RejectsUnsupportedBackingType()
    {
        var options = CreateFactoryOptions();

        Assert.Throws<NotSupportedException>(() => JsonSerializer.Serialize(new TestUnsupportedId(DateTimeOffset.UtcNow), options));
    }

    private static JsonSerializerOptions CreateFactoryOptions()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new StronglyTypedIdJsonConverterFactory());
        return options;
    }

    [DebuggerDisplay("{Value}")]
    [JsonConverter(typeof(StronglyTypedGuidConverter<TestOtherIdentityId>))]
    private readonly struct TestOtherIdentityId : IEquatable<TestOtherIdentityId>, IStronglyTypedId<Guid>
    {
        public TestOtherIdentityId(Guid value)
        {
            if (value == Guid.Empty)
                throw new FormatException("Invalid TestOtherIdentityId: cannot use an empty guid.");

            Value = value;
        }

        public Guid Value { get; }

        public override string ToString()
        {
            return Value.ToString("D");
        }

        public static implicit operator Guid(TestOtherIdentityId key)
        {
            return key.Value;
        }

        public bool Equals(TestOtherIdentityId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TestOtherIdentityId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    [DebuggerDisplay("{Value}")]
    [JsonConverter(typeof(StronglyTypedGuidConverter<TestIdentityId>))]
    private readonly struct TestIdentityId : IEquatable<TestIdentityId>, IStronglyTypedId<Guid>
    {
        public TestIdentityId(Guid value)
        {
            if (value == Guid.Empty)
                throw new FormatException("Invalid TestIdentityId: cannot use an empty guid.");

            Value = value;
        }

        public Guid Value { get; }

        public static TestIdentityId New()
        {
            return new TestIdentityId(Guid.NewGuid());
        }

        public override string ToString()
        {
            return Value.ToString("D");
        }

        public static implicit operator Guid(TestIdentityId key)
        {
            return key.Value;
        }

        public static explicit operator TestIdentityId(Guid value)
        {
            return new TestIdentityId(value);
        }

        public static explicit operator string(TestIdentityId key)
        {
            return key.Value.ToString("D");
        }

        public static explicit operator TestIdentityId(string value)
        {
            if (!Guid.TryParse(value, out var guid))
                throw new FormatException($"Invalid TestIdentityId: '{value}' is not a valid Guid.");

            return new TestIdentityId(guid);
        }

        public bool Equals(TestIdentityId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TestIdentityId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    [DebuggerDisplay("{Value}")]
    private readonly struct TestFactoryIdentityId : IEquatable<TestFactoryIdentityId>, IStronglyTypedId<Guid>
    {
        public TestFactoryIdentityId(Guid value)
        {
            if (value == Guid.Empty)
                throw new FormatException("Invalid TestFactoryIdentityId: cannot use an empty guid.");

            Value = value;
        }

        public Guid Value { get; }

        public static TestFactoryIdentityId New()
        {
            return new TestFactoryIdentityId(Guid.NewGuid());
        }

        public override string ToString()
        {
            return Value.ToString("D");
        }

        public bool Equals(TestFactoryIdentityId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is TestFactoryIdentityId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    [DebuggerDisplay("{Value}")]
    private readonly struct TestFactoryPlatformId : IStronglyTypedId<string>
    {
        public TestFactoryPlatformId(string? value)
        {
            Value = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
        }

        public string Value { get; }

        public override string ToString()
        {
            return Value;
        }
    }

    [DebuggerDisplay("{Value}")]
    private readonly struct TestFactoryOptionalPlatformId : IOptionalStringId
    {
        public TestFactoryOptionalPlatformId(string? value)
        {
            Value = StringIdHelpers.NormalizeOptional(value);
        }

        public string Value { get; }

        public bool IsEmpty => StringIdHelpers.IsMissing(Value);

        public override string ToString()
        {
            return Value;
        }
    }

    [DebuggerDisplay("{Value}")]
    private readonly struct TestFactoryNumericId : IStronglyTypedId<int>
    {
        public TestFactoryNumericId(int value)
        {
            if (value <= 0)
                throw new FormatException("Invalid TestFactoryNumericId: value must be greater than zero.");

            Value = value;
        }

        public int Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [DebuggerDisplay("{Value}")]
    private readonly struct TestFactorySnowflakeId : IStronglyTypedId<long>
    {
        public TestFactorySnowflakeId(long value)
        {
            if (value <= 0)
                throw new FormatException("Invalid TestFactorySnowflakeId: value must be greater than zero.");

            Value = value;
        }

        public long Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    [DebuggerDisplay("{Value}")]
    [JsonConverter(typeof(StronglyTypedStringConverter<TestPlatformId>))]
    private readonly struct TestPlatformId : IEquatable<TestPlatformId>, IStringId
    {
        public TestPlatformId(string? value)
        {
            Value = string.IsNullOrWhiteSpace(value) ? "" : value.Trim();
        }

        public string Value { get; }

        public bool IsEmpty => string.IsNullOrEmpty(Value);

        public override string ToString()
        {
            return Value;
        }

        public static explicit operator TestPlatformId(string? value)
        {
            return new TestPlatformId(value);
        }

        public bool Equals(TestPlatformId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            return obj is TestPlatformId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return IsEmpty ? 0 : Value.GetHashCode();
        }
    }

    [DebuggerDisplay("{Value}")]
    [JsonConverter(typeof(StronglyTypedOptionalStringConverter<TestOptionalPlatformId>))]
    private readonly struct TestOptionalPlatformId : IEquatable<TestOptionalPlatformId>, IOptionalStringId
    {
        public TestOptionalPlatformId(string? value)
        {
            Value = StringIdHelpers.NormalizeOptional(value);
        }

        public string Value { get; }

        public bool IsEmpty => StringIdHelpers.IsMissing(Value);

        public override string ToString()
        {
            return Value;
        }

        public bool Equals(TestOptionalPlatformId other)
        {
            return string.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            return obj is TestOptionalPlatformId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return IsEmpty ? 0 : Value.GetHashCode();
        }
    }

    [DebuggerDisplay("{Value}")]
    [JsonConverter(typeof(StronglyTypedInt32Converter<TestNumericId>))]
    private readonly struct TestNumericId : IEquatable<TestNumericId>, IStronglyTypedId<int>
    {
        public TestNumericId(int value)
        {
            if (value <= 0)
                throw new FormatException("Invalid TestNumericId: value must be greater than zero.");

            Value = value;
        }

        public int Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static explicit operator TestNumericId(int value)
        {
            return new TestNumericId(value);
        }

        public bool Equals(TestNumericId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is TestNumericId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }
    }

    [DebuggerDisplay("{Value}")]
    [JsonConverter(typeof(StronglyTypedInt64Converter<TestSnowflakeId>))]
    private readonly struct TestSnowflakeId : IEquatable<TestSnowflakeId>, IStronglyTypedId<long>
    {
        public TestSnowflakeId(long value)
        {
            if (value <= 0)
                throw new FormatException("Invalid TestSnowflakeId: value must be greater than zero.");

            Value = value;
        }

        public long Value { get; }

        public override string ToString()
        {
            return Value.ToString();
        }

        public static explicit operator TestSnowflakeId(long value)
        {
            return new TestSnowflakeId(value);
        }

        public bool Equals(TestSnowflakeId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object? obj)
        {
            return obj is TestSnowflakeId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }

    private readonly struct TestUnsupportedId : IStronglyTypedId<DateTimeOffset>
    {
        public TestUnsupportedId(DateTimeOffset value)
        {
            Value = value;
        }

        public DateTimeOffset Value { get; }
    }
}
