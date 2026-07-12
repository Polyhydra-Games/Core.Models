using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class CacheTests
{
    [Test]
    public void NullValue_Should_Be_Expired()
    {
        var cache = new Cache<string>(null,TimeSpan.FromSeconds(10));
        Assert.That(() => cache.IsExpired, Is.True);
        cache.Value = "Hello";

        Assert.That(() => cache.IsExpired, Is.False);
    }

    [Test]
    public void Value_ShouldReturnSetValue_WhenNoExpiration()
    {
        var cache = new Cache<string>("Hello", null);
        Assert.That(() => "Hello" == cache.Value, Is.True);
    }

    [Test]
    public void Value_ShouldReturnSetValue_WhenExpirationLengthZero()
    {
        var cache = new Cache<int>(42, TimeSpan.Zero);
        Assert.That(() => 42 == cache.Value, Is.True);
    }

    [Test]
    public void Value_ShouldReturnNull_WhenExpired()
    {
        var cache = new Cache<string>("Test", TimeSpan.FromMilliseconds(100));
        System.Threading.Thread.Sleep(150);
        Assert.That(() => cache.Value == null, Is.True);
    }

    [Test]
    public void Value_ShouldReturnValue_WhenNotExpired()
    {
        var cache = new Cache<string>("Active", TimeSpan.FromSeconds(14));
        Assert.That(() => "Active" == cache.Value, Is.True);
    }

    [Test]
    public void Value_ShouldResetOnSet()
    {
        var cache = new Cache<string>("Initial", TimeSpan.FromSeconds(2));
        System.Threading.Thread.Sleep(2150);
        cache.Value = "Reset";
        Assert.That(() => "Reset" == cache.Value, Is.True);
    }

    [Test]
    public void Value_ShouldExpireAfterResetTime()
    {
        var cache = new Cache<string>("Initial", TimeSpan.FromMilliseconds(200));
        System.Threading.Thread.Sleep(150);
        cache.Value = "New";
        System.Threading.Thread.Sleep(250);
        Assert.That(() => cache.Value == null, Is.True);
    }

    [Test]
    public void Value_ShouldSupportValueTypes()
    {
        var cache = new Cache<int>(123, TimeSpan.FromSeconds(1));
        Assert.That(() => 123 == cache.Value, Is.True);
    }
}
