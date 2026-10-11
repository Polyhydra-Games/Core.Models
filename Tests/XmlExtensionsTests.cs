using NUnit.Framework;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Models.Serialization;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
[NonParallelizable]
public class XmlExtensionsTests
{
    private sealed class Payload { public string Name { get; set; } = ""; }

    private sealed class FakeProvider : ISerializationProvider
    {
        public bool Throw { get; set; }
        public int Calls { get; private set; }
        public T DeserializeObject<T>(string value, bool continueOnError = true)
        {
            Calls++;
            if (Throw) throw new InvalidOperationException("boom");
            return (T)(object)new Payload { Name = value };
        }
        public object DeserializeObject(string value, Type type, bool continueOnError = true)
        {
            Calls++;
            if (Throw) throw new InvalidOperationException("boom");
            return new Payload { Name = value };
        }
        public string SerializeObject(object value) => "<xml:" + value + ">";
        public (bool Success, string? Value) TrySerializeObject(object value) => (true, SerializeObject(value));
    }

    private ISerializationProvider? _previous;

    [SetUp] public void SetUp() => _previous = XmlExtensions.Provider;
    [TearDown] public void TearDown() => XmlExtensions.Provider = _previous;

    [Test]
    public void NoProvider_AllEntryPointsThrowNullReference()
    {
        XmlExtensions.Provider = null;
        Assert.Throws<NullReferenceException>(() => "x".FromXML(typeof(Payload)));
        Assert.Throws<NullReferenceException>(() => "x".FromXML<Payload>());
        Assert.Throws<NullReferenceException>(() => new object().ToXML());
    }

    [Test]
    public void FromXML_Generic_EmptyString_ReturnsDefaultWithoutCallingProvider()
    {
        var p = new FakeProvider();
        XmlExtensions.Provider = p;
        Assert.That("".FromXML<Payload>(), Is.Null);
        Assert.That(p.Calls, Is.EqualTo(0));
    }

    [Test]
    public void FromXML_Generic_DelegatesToProvider()
    {
        XmlExtensions.Provider = new FakeProvider();
        Assert.That("hi".FromXML<Payload>()!.Name, Is.EqualTo("hi"));
    }

    [Test]
    public void FromXML_Type_EmptyString_CreatesInstance()
    {
        XmlExtensions.Provider = new FakeProvider();
        Assert.That("".FromXML(typeof(Payload)), Is.TypeOf<Payload>());
    }

    [Test]
    public void FromXML_Type_ProviderThrows_ContinueOnError_ReturnsNewInstance()
    {
        XmlExtensions.Provider = new FakeProvider { Throw = true };
        var result = "bad".FromXML(typeof(Payload));
        Assert.That(result, Is.TypeOf<Payload>());
        Assert.That(((Payload)result!).Name, Is.EqualTo(""));
    }

    [Test]
    public void FromXML_Type_ProviderThrows_NoContinue_Rethrows()
    {
        XmlExtensions.Provider = new FakeProvider { Throw = true };
        Assert.Throws<InvalidOperationException>(() => "bad".FromXML(typeof(Payload), continueOnError: false));
    }

    [Test]
    public void FromXML_Generic_ProviderThrows_Propagates()
    {
        // Current behavior: the generic overload does not catch provider exceptions.
        XmlExtensions.Provider = new FakeProvider { Throw = true };
        Assert.Throws<InvalidOperationException>(() => "bad".FromXML<Payload>());
    }

    [Test]
    public void ToXML_DelegatesToProvider()
    {
        XmlExtensions.Provider = new FakeProvider();
        Assert.That("abc".ToXML(), Is.EqualTo("<xml:abc>"));
    }
}
