using NUnit.Framework;
using PolyhydraGames.Core.Models.DelayTimer;

namespace PolyhydraGames.Core.Models.Test;

/// <summary>
/// Concurrency and reset semantics for <see cref="DelayedTimer"/>. The timer works in whole seconds and has no
/// TimeProvider seam, so every pending Delay() here is cancelled via Reset(); nothing waits out a real delay.
/// </summary>
[TestFixture]
public class DelayedTimerTests
{
    private static readonly TimeSpan Bound = TimeSpan.FromSeconds(5);

    [Test]
    public async Task Reset_WhileIdle_DoesNotThrow_AndRestoresStartingDelay()
    {
        var timer = new DelayedTimer("idle", 2, 1, 10);

        Assert.DoesNotThrow(() => timer.Reset());
        Assert.DoesNotThrow(() => timer.Reset());

        Assert.That(timer.CTS, Is.Null);
        Assert.That(timer.CurrentDelay, Is.EqualTo(2));
        await Task.CompletedTask;
    }

    [Test]
    public async Task Reset_DuringPendingDelay_CancelsAwaiter_AndClearsState()
    {
        var timer = new DelayedTimer("pending", 0, 1, 10);
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
        var timer = new DelayedTimer("concurrent", 0, 1, 10);
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
        var timer = new DelayedTimer("many", 0, 1, 100);
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

        // Losers fault almost immediately; wait until callers - 1 have faulted.
        var deadline = DateTime.UtcNow + Bound;
        while (tasks.Count(t => t.IsFaulted) < callers - 1 && DateTime.UtcNow < deadline)
            await Task.Delay(5);

        var faulted = tasks.Where(t => t.IsFaulted).ToList();
        Assert.That(faulted, Has.Count.EqualTo(callers - 1));
        Assert.That(faulted.All(t => t.Exception!.InnerException is InvalidOperationException), Is.True);

        timer.Reset();
        var winner = tasks.Single(t => !t.IsFaulted);
        Assert.ThrowsAsync<TaskCanceledException>(async () => await winner.WaitAsync(Bound));
    }

    [Test]
    public async Task DelayResetRace_NeverCorruptsState_AndEveryDelayEndsCancelledOrRejected()
    {
        var timer = new DelayedTimer("race", 0, 1, 1000);
        const int iterations = 200;
        var outcomes = new List<Task>();
        using var stop = new CancellationTokenSource();

        var resetter = Task.Run(() =>
        {
            while (!stop.IsCancellationRequested)
                timer.Reset();
        });

        for (var i = 0; i < iterations; i++)
        {
            var d = timer.Delay();
            outcomes.Add(d);
            await Task.Yield();
        }

        stop.Cancel();
        await resetter.WaitAsync(Bound);
        timer.Reset();

        // Every delay must end promptly as cancelled, or as an overlap rejection; none may hang or fail otherwise.
        foreach (var outcome in outcomes)
        {
            try
            {
                await outcome.WaitAsync(Bound);
                Assert.Fail("A multi-second delay completed instead of being cancelled.");
            }
            catch (TaskCanceledException) { }
            catch (InvalidOperationException) { }
        }

        Assert.That(timer.CTS, Is.Null);
        Assert.That(timer.CurrentDelay, Is.Zero);
        Assert.DoesNotThrowAsync(async () =>
        {
            var next = timer.Delay();
            timer.Reset();
            try { await next.WaitAsync(Bound); } catch (TaskCanceledException) { }
        });
    }
}
