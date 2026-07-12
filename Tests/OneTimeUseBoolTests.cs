using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class OneTimeUseBoolTests
{
    [Test]
    public void OneTimeUseBool()
    {
        var oneTimeUseBool = new OneTimeUseBool();
        Assert.That(() => oneTimeUseBool.Value, Is.True);
        Assert.That(() => !oneTimeUseBool.Value, Is.True);
    }

}
