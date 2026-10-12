using System.Text.Json;
using NUnit.Framework;
using PolyhydraGames.Core.Models.Serialization;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class SystemTextJsonHelperTests
{
    private sealed class Item
    {
        public string Name { get; set; } = "";
        public int Count { get; set; }
    }

    [Test]
    public void DefaultCtor_IsCaseInsensitive()
    {
        var helper = new SystemTextJsonHelper();
        Assert.That(helper.Options.PropertyNameCaseInsensitive, Is.True);
        var item = helper.DeserializeObject<Item>("""{"name":"a","COUNT":3}""");
        Assert.That(item.Name, Is.EqualTo("a"));
        Assert.That(item.Count, Is.EqualTo(3));
    }

    [Test]
    public void CustomOptions_AreExposedAndUsed()
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = false };
        var helper = new SystemTextJsonHelper(options);
        Assert.That(helper.Options, Is.SameAs(options));
        var item = helper.DeserializeObject<Item>("""{"name":"a"}""");
        Assert.That(item.Name, Is.EqualTo(""));
    }

    [Test]
    public void DeserializeObject_ByType_ReturnsInstance()
    {
        var obj = new SystemTextJsonHelper().DeserializeObject("""{"Name":"z"}""", typeof(Item));
        Assert.That(obj, Is.TypeOf<Item>());
        Assert.That(((Item)obj).Name, Is.EqualTo("z"));
    }

    [Test]
    public void DeserializeObject_InvalidJson_Throws_EvenWhenContinueOnErrorTrue()
    {
        // Current behavior: continueOnError is ignored by the provider; callers handle it.
        Assert.Throws<JsonException>(() => new SystemTextJsonHelper().DeserializeObject<Item>("nope", true));
    }

    [Test]
    public void SerializeObject_RoundTrips()
    {
        var helper = new SystemTextJsonHelper();
        var json = helper.SerializeObject(new Item { Name = "n", Count = 2 });
        Assert.That(json, Is.EqualTo("""{"Name":"n","Count":2}"""));
    }

    [Test]
    public void TrySerializeObject_Success()
    {
        var (ok, value) = new SystemTextJsonHelper().TrySerializeObject(new Item { Name = "n" });
        Assert.That(ok, Is.True);
        Assert.That(value, Does.Contain("\"Name\":\"n\""));
    }

    [Test]
    public void TrySerializeObject_Failure_ReturnsFalseAndNull()
    {
        var self = new List<object>();
        self.Add(self); // cycle -> JsonException
        var (ok, value) = new SystemTextJsonHelper().TrySerializeObject(self);
        Assert.That(ok, Is.False);
        Assert.That(value, Is.Null);
    }
}
