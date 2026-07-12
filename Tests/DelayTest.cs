using NUnit.Framework;
using PolyhydraGames.Core.Models.DelayTimer;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class DelayTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestNestedExceptions()
    {
        try
        {
            var timer = new DelayedTimer("Test", 5, 5, 15);
            for (var x = 0; x < 5; x++)
            {
                await timer.Delay();
            }
        }
        catch (MaximumDurationException ex)
        {
            Assert.Pass(ex.Message);
        }

        Assert.Fail("The error never threw");
    }

    [Test]
    public async Task TestMaximumDurationBoundaryDoesNotOvershoot()
    {
        MaximumDurationException? caught = null;
        var timer = new DelayedTimer("Test", 1, 1, 2);

        try
        {
            for (var x = 0; x < 4; x++)
            {
                await timer.Delay();
            }
        }
        catch (MaximumDurationException ex)
        {
            caught = ex;
        }

        Assert.That(() => caught, Is.Not.Null);
        Assert.That(() => caught!.State.CurrentDelay, Is.EqualTo(2));
        Assert.That(() => caught.Message, Does.Not.Contain("3"));
    }
}
