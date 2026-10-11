using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class SubjectObservableTests
{
    private sealed class Collector<T> : IObserver<T>
    {
        public List<T> Items { get; } = new();
        public void OnCompleted() { }
        public void OnError(Exception error) { }
        public void OnNext(T value) => Items.Add(value);
    }

    [Test]
    public void SetValue_NewValue_UpdatesValueAndNotifies()
    {
        using var subject = new SubjectObservable<int>();
        var c = new Collector<int>();
        using var sub = subject.Subscribe(c);

        Assert.That(subject.SetValue(5), Is.True);
        Assert.That(subject.Value, Is.EqualTo(5));
        Assert.That(c.Items, Is.EqualTo(new[] { 5 }));
    }

    [Test]
    public void SetValue_SameValue_WithEqualityCheck_DoesNotNotify()
    {
        using var subject = new SubjectObservable<int>();
        var c = new Collector<int>();
        using var sub = subject.Subscribe(c);

        subject.SetValue(1);
        Assert.That(subject.SetValue(1), Is.False);
        Assert.That(c.Items, Is.EqualTo(new[] { 1 }));
    }

    [Test]
    public void SetValue_DefaultValueFirst_WithEqualityCheck_IsTreatedAsUnchanged()
    {
        // Current behavior: initial _value is default, so setting default is a no-op.
        using var subject = new SubjectObservable<int>();
        Assert.That(subject.SetValue(0), Is.False);
    }

    [Test]
    public void SetValue_SameValue_WithoutEqualityCheck_NotifiesEveryTime()
    {
        using var subject = new SubjectObservable<int>(checkEquality: false);
        var c = new Collector<int>();
        using var sub = subject.Subscribe(c);

        Assert.That(subject.SetValue(2), Is.True);
        Assert.That(subject.SetValue(2), Is.True);
        Assert.That(c.Items, Is.EqualTo(new[] { 2, 2 }));
    }

    [Test]
    public void Unsubscribe_StopsNotifications()
    {
        using var subject = new SubjectObservable<string>();
        var c = new Collector<string>();
        var sub = subject.Subscribe(c);
        subject.SetValue("a");
        sub.Dispose();
        subject.SetValue("b");

        Assert.That(c.Items, Is.EqualTo(new[] { "a" }));
        Assert.That(subject.Value, Is.EqualTo("b"));
    }

    [Test]
    public void Dispose_ThenSetValue_Throws()
    {
        var subject = new SubjectObservable<int>();
        subject.Dispose();
        Assert.Throws<ObjectDisposedException>(() => subject.SetValue(1));
    }
}
