using NUnit.Framework;
using PolyhydraGames.Core.Models.DelayTimer;

namespace PolyhydraGames.Core.Models.Test;

/// <summary>
/// Concurrency and reset semantics for <see cref="DelayedTimer"/>. Time is driven by a
/// <see cref="ManualTimeProvider"/>, so nothing waits on the wall clock.
/// </summary>
[TestFixture]
public class DelayedTimerTests
{
    private static readonly TimeSpan Bound = TimeSpan.FromSeconds(5);

    [Test]
    public async Task Reset_WhileIdle_DoesNotThrow_AndRestoresStartingDelay()
    {
        var timer = new DelayedTimer("idle", 2, 1, 10, new ManualTimeProvider());

        Assert.DoesNotThrow(() => timer.Reset());
        Assert.DoesNotThrow(() => timer.Reset());

        Assert.That(timer.CTS, Is.Null);
        Assert.That(timer.CurrentDelay, Is.EqualTo(2));
        await Task.CompletedTask;
    }

    [Test]
    public async Task Reset_DuringPendingDelay_CancelsAwaiter_AndClearsState()
    {
        var timer = new DelayedTimer("pending", 0, 1, 10, new ManualTimeProvider());
        var pending = timer.Delay();
        Assert.That(timer.CTS, Is.Not.Null);
        Assert.That(timer.CurrentDelay, Is.EqualTo(1));

        timer.Reset();

        Assert.ThrowsAsync<TaskCanceledException>(async () => await pending.WaitAsync(Bound));
        Assert.That(timer.CTS, Is.Null);
        Assert.That(timer.CurrentDelay, Is.Zero);
        await Task.CompletedTask;
    }

    [Test]
    public async Task ConcurrentDelay_WhilePending_ThrowsInvalidOperation_AndFirstStaysPending()
    {
        var timer = new DelayedTimer("concurrent", 0, 1, 10, new ManualTimeProvider());
        var first = timer.Delay();

        Assert.ThrowsAsync<InvalidOperationException>(async () => await timer.Delay());
        Assert.That(first.IsCompleted, Is.False);

        timer.Reset();
        Assert.ThrowsAsync<TaskCanceledException>(async () => await first.WaitAsync(Bound));
        await Task.CompletedTask;
    }

    [Test]
    public async Task ConcurrentDelay_ManyThreads_ExactlyOneWins()
    {
        var timer = new DelayedTimer("many", 0, 1, 100, new ManualTimeProvider());
        const int callers = 32;
        using var gate = new ManualResetEventSlim(false);

        var tasks = Enumerable.Range(0, callers)
            .Select(_ => Task.Run(() =>
            {
                gate.Wait();
                return timer.Delay();
            }))
            .ToArray();

        gate.Set();

        // Losers fault promptly; await them via WhenAny (no polling, no sleeps) until callers - 1 have faulted.
        var pending = tasks.ToList();
        var faultedCount = 0;
        while (faultedCount < callers - 1)
        {
            var done = await Task.WhenAny(pending).WaitAsync(Bound);
            pending.Remove(done);
            faultedCount++;
        }

        var faulted = tasks.Where(t => t.IsFaulted).ToList();
        Assert.That(faulted, Has.Count.EqualTo(callers - 1));
        Assert.That(faulted.All(t => t.Exception!.InnerException is InvalidOperationException), Is.True);

        timer.Reset();
        var winner = tasks.Single(t => !t.IsFaulted);
        Assert.ThrowsAsync<TaskCanceledException>(async () => await winner.WaitAsync(Bound));
    }

    [Test]
    public async Task Delay_CompletesOnlyWhenTimeProviderAdvances_AndKeepsEscalatedDelay()
    {
        var clock = new ManualTimeProvider();
        var timer = new DelayedTimer("manual", 0, 2, 10, clock);

        var first = timer.Delay();
        clock.Advance(TimeSpan.FromSeconds(1));
        Assert.That(first.IsCompleted, Is.False);
        clock.Advance(TimeSpan.FromSeconds(1));
        await first.WaitAsync(Bound);

        Assert.That(timer.CTS, Is.Null);
        Assert.That(timer.CurrentDelay, Is.EqualTo(2));
        Assert.That(clock.ActiveTimerCount, Is.Zero);

        var second = timer.Delay();
        clock.Advance(TimeSpan.FromSeconds(3));
        Assert.That(second.IsCompleted, Is.False, "Second delay escalates to 4s.");
        clock.Advance(TimeSpan.FromSeconds(1));
        await second.WaitAsync(Bound);
        Assert.That(timer.CurrentDelay, Is.EqualTo(4));
    }

    [Test]
    public void Delay_BeyondMaximum_ThrowsMaximumDuration_WithoutLeavingCts()
    {
        var clock = new ManualTimeProvider();
        var timer = new DelayedTimer("max", 0, 5, 4, clock);

        Assert.ThrowsAsync<MaximumDurationException>(async () => await timer.Delay());
        Assert.That(timer.CTS, Is.Null);
        Assert.That(timer.CurrentDelay, Is.EqualTo(4));
    }

    [Test]
    public async Task DelayResetRace_NeverCorruptsState_AndEveryDelayEndsCancelledOrRejected()
    {
        var clock = new ManualTimeProvider();
        var timer = new DelayedTimer("race", 0, 1, 100000, clock);
        const int iterations = 500;
        var outcomes = new List<Task>();
        using var stop = new CancellationTokenSource();

        var resetter = Task.Run(() =>
        {
            while (!stop.IsCancellationRequested)
                timer.Reset();
        });

        for (var i = 0; i < iterations; i++)
        {
            outcomes.Add(timer.Delay());
            await Task.Yield();
        }

        stop.Cancel();
        await resetter.WaitAsync(Bound);
        timer.Reset();

        // The manual clock never advances, so no delay can complete: each must be cancelled or rejected,
        // and none may surface ObjectDisposedException from a disposed CTS.
        foreach (var outcome in outcomes)
        {
            try
            {
                await outcome.WaitAsync(Bound);
                Assert.Fail("A delay completed although the clock never advanced.");
            }
            catch (TaskCanceledException) { }
            catch (InvalidOperationException) { }
        }

        Assert.That(timer.CTS, Is.Null);
        Assert.That(timer.CurrentDelay, Is.Zero);
        Assert.That(clock.ActiveTimerCount, Is.Zero);
        var next = timer.Delay();
        timer.Reset();
        Assert.ThrowsAsync<TaskCanceledException>(async () => await next.WaitAsync(Bound));
    }
}
