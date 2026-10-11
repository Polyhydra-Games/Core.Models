using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class EnumDescriptionTests
{
    private enum Color { Red, Green }

    [Test]
    public void Record_ExposesValueTitleDescription_AndInterface()
    {
        var d = new EnumDescription<Color>(Color.Green, "Grn", "The green one");
        Assert.That(d.Value, Is.EqualTo(Color.Green));
        Assert.That(d.Title, Is.EqualTo("Grn"));
        Assert.That(d.Description, Is.EqualTo("The green one"));
        Assert.That(d, Is.InstanceOf<IEnumDescription>());
    }

    [Test]
    public void Record_HasValueEquality()
    {
        Assert.That(new EnumDescription<Color>(Color.Red, "a", "b"),
            Is.EqualTo(new EnumDescription<Color>(Color.Red, "a", "b")));
    }

    [Test]
    public void ToDescription_BuildsRecord()
    {
        var d = Color.Red.ToDescription("R", "desc");
        Assert.That(d, Is.EqualTo(new EnumDescription<Color>(Color.Red, "R", "desc")));
    }

    [Test]
    public void ToDescriptions_Defaults_UseEnumNameAndEmptyDescription()
    {
        var all = Color.Red.ToDescriptions();
        Assert.That(all.Select(x => x.Value), Is.EqualTo(new[] { Color.Red, Color.Green }));
        Assert.That(all.Select(x => x.Title), Is.EqualTo(new[] { "Red", "Green" }));
        Assert.That(all.All(x => x.Description == ""), Is.True);
    }

    [Test]
    public void ToDescriptions_UsesProvidedSelectors()
    {
        var all = Color.Red.ToDescriptions(c => c.ToString().ToUpperInvariant(), c => "d-" + c);
        Assert.That(all[1].Title, Is.EqualTo("GREEN"));
        Assert.That(all[1].Description, Is.EqualTo("d-Green"));
    }
}
