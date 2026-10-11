using System.ComponentModel;
using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class ToggleBooleanTests
{
    [Test]
    public void Title_And_IsToggled_ReflectConstructorDelegates()
    {
        var state = true;
        var toggle = new ToggleBoolean("Wifi", () => state, v => state = v);

        Assert.That(toggle.Title, Is.EqualTo("Wifi"));
        Assert.That(toggle.IsToggled, Is.True);
    }

    [Test]
    public void Setting_NewValue_CallsSetter_AndRaisesPropertyChanged()
    {
        var state = false;
        var toggle = new ToggleBoolean("t", () => state, v => state = v);
        var names = new List<string?>();
        toggle.PropertyChanged += (_, e) => names.Add(e.PropertyName);

        toggle.IsToggled = true;

        Assert.That(state, Is.True);
        Assert.That(names, Is.EqualTo(new[] { nameof(ToggleBoolean.IsToggled) }));
    }

    [Test]
    public void Setting_SameValue_DoesNotCallSetterOrRaise()
    {
        var state = true;
        var setCalls = 0;
        var raised = 0;
        var toggle = new ToggleBoolean("t", () => state, v => { setCalls++; state = v; });
        toggle.PropertyChanged += (_, _) => raised++;

        toggle.IsToggled = true;

        Assert.That(setCalls, Is.EqualTo(0));
        Assert.That(raised, Is.EqualTo(0));
    }

    [Test]
    public void ImplementsINotifyPropertyChanged()
    {
        var toggle = new ToggleBoolean("t", () => false, _ => { });
        Assert.That(toggle, Is.InstanceOf<INotifyPropertyChanged>());
    }
}
