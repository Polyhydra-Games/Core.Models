#pragma warning disable CA1861 // inline expected arrays are idiomatic in NUnit assertions
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class ObservableExtensionsTests
{
    private static void WaitUntil(Func<bool> condition)
    {
        Assert.That(SpinWait.SpinUntil(condition, TimeSpan.FromSeconds(5)), Is.True, "timed out waiting for condition");
    }

    [Test]
    public void SelectAsync_WithoutOnError_ThrowingSelectorFaultsStream()
    {
        var source = new Subject<int>();
        var results = new List<int>();
        Exception? error = null;
        source.SelectAsync<int, int>(x => x == 2 ? throw new InvalidOperationException("boom") : Task.FromResult(x * 10))
            .Subscribe(results.Add, ex => error = ex);

        source.OnNext(1);
        source.OnNext(2);

        Assert.That(results, Is.EqualTo(new[] { 10 }));
        Assert.That(error, Is.InstanceOf<InvalidOperationException>());
        Assert.That(error!.Message, Is.EqualTo("boom"));
    }

    [Test]
    public void SelectAsync_WithoutOnError_FaultedTaskFaultsStream()
    {
        var source = new Subject<int>();
        Exception? error = null;
        source.SelectAsync<int, int>(_ => Task.FromException<int>(new ArgumentException("bad")))
            .Subscribe(_ => { }, ex => error = ex);

        source.OnNext(1);

        Assert.That(error, Is.InstanceOf<ArgumentException>());
    }

    [Test]
    public void SelectAsync_WithOnError_DropsFailureAndContinues()
    {
        var source = new Subject<int>();
        var results = new List<int>();
        var handled = new List<Exception>();
        Exception? error = null;
        var completed = false;
        source.SelectAsync<int, int>(
                x => x == 2 ? Task.FromException<int>(new InvalidOperationException("boom")) : Task.FromResult(x * 10),
                handled.Add)
            .Subscribe(results.Add, ex => error = ex, () => completed = true);

        source.OnNext(1);
        source.OnNext(2);
        source.OnNext(3);
        source.OnCompleted();

        Assert.That(results, Is.EqualTo(new[] { 10, 30 }));
        Assert.That(handled, Has.Count.EqualTo(1));
        Assert.That(handled[0], Is.InstanceOf<InvalidOperationException>());
        Assert.That(error, Is.Null);
        Assert.That(completed, Is.True);
    }

    [Test]
    public void SelectAsync_WithOnError_SynchronousThrowIsAlsoHandled()
    {
        var source = new Subject<int>();
        var results = new List<int>();
        var handled = new List<Exception>();
        source.SelectAsync<int, int>(
                x => x == 1 ? throw new InvalidOperationException("sync") : Task.FromResult(x),
                handled.Add)
            .Subscribe(results.Add);

        source.OnNext(1);
        source.OnNext(2);

        Assert.That(handled, Has.Count.EqualTo(1));
        Assert.That(results, Is.EqualTo(new[] { 2 }));
    }

    [Test]
    public void SelectAsync_WithoutOnError_StreamTerminatesAfterFirstFailure()
    {
        var source = new Subject<int>();
        var calls = new List<int>();
        var results = new List<int>();
        var errors = 0;
        source.SelectAsync<int, int>(x =>
            {
                calls.Add(x);
                return x == 1 ? Task.FromException<int>(new InvalidOperationException()) : Task.FromResult(x);
            })
            .Subscribe(results.Add, _ => errors++);

        source.OnNext(1);
        source.OnNext(2);
        source.OnNext(3);

        Assert.That(errors, Is.EqualTo(1));
        Assert.That(results, Is.Empty);
        Assert.That(calls, Is.EqualTo(new[] { 1 }), "selector must not run after the pipeline faulted");
        Assert.That(source.HasObservers, Is.False, "faulted pipeline must unsubscribe from source");
    }

    [Test]
    public void SelectAsync_SequentialCompletions_PreserveOrder()
    {
        var source = new Subject<int>();
        var gates = new Dictionary<int, TaskCompletionSource<string>>();
        var results = new List<string>();
        source.SelectAsync(x => (gates[x] = new TaskCompletionSource<string>()).Task).Subscribe(results.Add);

        source.OnNext(1);
        gates[1].SetResult("a");
        WaitUntil(() => results.Count == 1);
        source.OnNext(2);
        gates[2].SetResult("b");
        WaitUntil(() => results.Count == 2);
        source.OnNext(3);
        gates[3].SetResult("c");
        WaitUntil(() => results.Count == 3);

        Assert.That(results, Is.EqualTo(new[] { "a", "b", "c" }));
    }

    [Test]
    public void SelectAsync_EmitsInCompletionOrder_WhenSelectorsOverlap()
    {
        var source = new Subject<int>();
        var gates = new Dictionary<int, TaskCompletionSource<int>>();
        var results = new List<int>();
        source.SelectAsync(x => (gates[x] = new TaskCompletionSource<int>()).Task).Subscribe(results.Add);

        source.OnNext(1);
        source.OnNext(2);
        gates[2].SetResult(200);
        WaitUntil(() => results.Count == 1);
        gates[1].SetResult(100);
        WaitUntil(() => results.Count == 2);

        Assert.That(results, Is.EqualTo(new[] { 200, 100 }));
    }

    [Test]
    public void SelectAsync_Unsubscribe_StopsFurtherEmissions()
    {
        var source = new Subject<int>();
        var results = new List<int>();
        var pending = new TaskCompletionSource<int>();
        var sub = source.SelectAsync(x => x == 2 ? pending.Task : Task.FromResult(x)).Subscribe(results.Add);

        source.OnNext(1);
        source.OnNext(2);
        sub.Dispose();
        pending.SetResult(2);
        source.OnNext(3);

        Assert.That(results, Is.EqualTo(new[] { 1 }));
        Assert.That(source.HasObservers, Is.False);
    }

    [Test]
    public async Task SelectAsync_Unsubscribe_WithInFlightTask_DoesNotEmitLateResult()
    {
        var results = new List<int>();
        var pending = new TaskCompletionSource<int>();
        var sub = Observable.Return(1).SelectAsync(_ => pending.Task).Subscribe(results.Add);
        sub.Dispose();
        pending.SetResult(5);
        await Task.Delay(20); // tiny bounded wait: late continuation must not emit
        Assert.That(results, Is.Empty);
    }

    [Test]
    public void DoAsync_WithoutOnError_FaultsStreamAndTerminates()
    {
        var source = new Subject<int>();
        var calls = new List<int>();
        var units = 0;
        Exception? error = null;
        source.DoAsync(x =>
            {
                calls.Add(x);
                return x == 2 ? Task.FromException(new InvalidOperationException("do")) : Task.CompletedTask;
            })
            .Subscribe(_ => units++, ex => error = ex);

        source.OnNext(1);
        source.OnNext(2);
        source.OnNext(3);

        Assert.That(units, Is.EqualTo(1));
        Assert.That(error, Is.InstanceOf<InvalidOperationException>());
        Assert.That(calls, Is.EqualTo(new[] { 1, 2 }));
        Assert.That(source.HasObservers, Is.False);
    }

    [Test]
    public void DoAsync_WithOnError_DropsFailureAndContinues()
    {
        var source = new Subject<int>();
        var units = new List<Unit>();
        var handled = new List<Exception>();
        Exception? error = null;
        source.DoAsync(
                x => x == 2 ? Task.FromException(new InvalidOperationException("do")) : Task.CompletedTask,
                handled.Add)
            .Subscribe(units.Add, ex => error = ex);

        source.OnNext(1);
        source.OnNext(2);
        source.OnNext(3);

        Assert.That(units, Has.Count.EqualTo(2));
        Assert.That(handled, Has.Count.EqualTo(1));
        Assert.That(error, Is.Null);
    }

    [Test]
    public void DoAsync_SequentialCompletions_PreserveOrderOfSideEffects()
    {
        var source = new Subject<int>();
        var gates = new Dictionary<int, TaskCompletionSource>();
        var order = new List<int>();
        source.DoAsync(async x =>
            {
                var tcs = gates[x] = new TaskCompletionSource();
                await tcs.Task;
                order.Add(x);
            })
            .Subscribe();

        for (var i = 1; i <= 3; i++)
        {
            source.OnNext(i);
            gates[i].SetResult();
            var expected = i;
            WaitUntil(() => order.Count == expected);
        }

        Assert.That(order, Is.EqualTo(new[] { 1, 2, 3 }));
    }

    [Test]
    public void DoAsync_Unsubscribe_StopsFurtherEmissions()
    {
        var source = new Subject<int>();
        var units = 0;
        var pending = new TaskCompletionSource();
        var sub = source.DoAsync(x => x == 2 ? pending.Task : Task.CompletedTask).Subscribe(_ => units++);

        source.OnNext(1);
        source.OnNext(2);
        sub.Dispose();
        pending.SetResult();
        source.OnNext(3);

        Assert.That(units, Is.EqualTo(1));
        Assert.That(source.HasObservers, Is.False);
    }
}
