using System.Text.Json;
using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class JsonHelperTests
{
    private class TestObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
        public bool IsActive { get; set; }
    }

    [Test]
    public void ToJson_Should_SerializeObject()
    {
        var obj = new TestObject { Name = "Test", Value = 42, IsActive = true };
        var json = obj.ToJson();
        
        Assert.That(json, Is.Not.Empty);
        Assert.That(json, Does.Contain("Test"));
        Assert.That(json, Does.Contain("42"));
    }

    [Test]
    public void FromJson_Should_DeserializeObject()
    {
        var json = """{"Name":"Hello","Value":100,"IsActive":false}""";
        var result = json.FromJson<TestObject>() ?? throw new AssertionException("Deserialization returned null.");
        
        Assert.That(result.Name, Is.EqualTo("Hello"));
        Assert.That(result.Value, Is.EqualTo(100));
        Assert.That(result.IsActive, Is.False);
    }

    [Test]
    public void FromJson_Should_ReturnDefault_WhenEmptyString()
    {
        var result = "".FromJson<TestObject>() ?? throw new AssertionException("Default object was null.");
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(string.Empty));
        Assert.That(result.Value, Is.EqualTo(0));
    }

    [Test]
    public void FromJson_Should_ReturnDefault_WhenNullString()
    {
        string? nullString = null;
        var result = nullString.FromJson<TestObject>() ?? throw new AssertionException("Default object was null.");
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(string.Empty));
        Assert.That(result.Value, Is.EqualTo(0));
    }

    [Test]
    public void RoundTrip_Should_PreserveData()
    {
        var original = new TestObject { Name = "RoundTrip", Value = 999, IsActive = true };
        var json = original.ToJson();
        var restored = json.FromJson<TestObject>() ?? throw new AssertionException("Round-trip result was null.");
        
        Assert.That(restored.Name, Is.EqualTo(original.Name));
        Assert.That(restored.Value, Is.EqualTo(original.Value));
        Assert.That(restored.IsActive, Is.EqualTo(original.IsActive));
    }

    [Test]
    public void FromJson_Should_HandleInvalidJson_WhenContinueOnErrorTrue()
    {
        var invalidJson = "not valid json at all";
        var result = invalidJson.FromJson<TestObject>(continueOnError: true)
            ?? throw new AssertionException("Error recovery result was null.");
        
        // Should return default object due to error handling
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo(string.Empty));
    }

    [Test]
    public void FromJson_Type_Should_Throw_WhenContinueOnErrorIsFalse()
    {
        Assert.Throws<JsonException>(() => "invalid".FromJson(typeof(TestObject), continueOnError: false));
    }

    [Test]
    public void FromJson_Type_Should_PreserveSerializedNull()
    {
        Assert.That("null".FromJson(typeof(TestObject)), Is.Null);
    }

    [Test]
    public void FromJson_Generic_Should_PreserveSerializedNull()
    {
        Assert.That("null".FromJson<TestObject>(), Is.Null);
    }

    [Test]
    public void ToJson_Should_HandlePrimitiveTypes()
    {
        var json = 42.ToJson();
        Assert.That(json, Is.EqualTo("42"));
        
        json = true.ToJson();
        Assert.That(json, Is.EqualTo("true"));
        
        json = "hello".ToJson();
        Assert.That(json, Is.EqualTo("\"hello\""));
    }

    [Test]
    public void FromJson_Should_HandlePrimitiveTypes()
    {
        var result = "42".FromJson<int>();
        Assert.That(result, Is.EqualTo(42));

        var boolResult = "true".FromJson<bool>();
        Assert.That(boolResult, Is.True);

        var stringResult = "\"world\"".FromJson<string>();
        Assert.That(stringResult, Is.EqualTo("world"));
    }
}
