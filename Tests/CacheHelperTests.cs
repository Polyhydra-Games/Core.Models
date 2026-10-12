using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class CacheHelperTests
{
    [Test]
    public void CreateCache_DefaultExpiration_IsFiveMinutes()
    {
        var cache = CacheHelper.CreateCache("v");
        Assert.That(cache.ExpirationLength, Is.EqualTo(TimeSpan.FromMinutes(5)));
        Assert.That(cache.Value, Is.EqualTo("v"));
        Assert.That(cache.IsExpired, Is.False);
    }

    [Test]
    public void CreateCache_CustomExpiration_IsPreserved()
    {
        var cache = CacheHelper.CreateCache(42, TimeSpan.FromSeconds(30));
        Assert.That(cache.ExpirationLength, Is.EqualTo(TimeSpan.FromSeconds(30)));
        Assert.That(cache.Value, Is.EqualTo(42));
    }

    [Test]
    public void CreateCache_NullValue_IsExpired()
    {
        var cache = CacheHelper.CreateCache<string>(null);
        Assert.That(cache.IsExpired, Is.True);
        Assert.That(cache.Value, Is.Null);
    }

    [Test]
    public void CreateCache_NegativeExpiration_NeverExpires()
    {
        var cache = CacheHelper.CreateCache("x", TimeSpan.FromSeconds(-1));
        Assert.That(cache.IsExpired, Is.False);
        Assert.That(cache.Value, Is.EqualTo("x"));
    }
}
