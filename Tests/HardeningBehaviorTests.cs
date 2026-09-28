using System.Diagnostics;
using System.IO;
using System.Reactive.Subjects;
using System.Reflection;
using System.Text.Json;
using NUnit.Framework;
using PolyhydraGames.Core.Interfaces;
using PolyhydraGames.Core.Models.Analytics;
using PolyhydraGames.Core.Models.DelayTimer;
using PolyhydraGames.Core.Models.Serialization;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public sealed class HardeningBehaviorTests
{
    [Test]
    public void FromCsv_Type_Should_Throw_WhenContinueOnErrorIsFalse()
    {
        var previous = CSVExtensions.Provider;
        CSVExtensions.Provider = new SystemTextJsonHelper();
        try
        {
            Assert.Throws<JsonException>(() => "invalid".FromCSV(typeof(TestPayload), continueOnError: false));
        }
        finally
        {
            CSVExtensions.Provider = previous;
        }
    }

    [Test]
    public void FromCsv_Type_Should_PreserveSerializedNull()
    {
        var previous = CSVExtensions.Provider;
        CSVExtensions.Provider = new SystemTextJsonHelper();
        try
        {
            Assert.That("null".FromCSV(typeof(TestPayload)), Is.Null);
            Assert.That("null".FromCSV<TestPayload>(), Is.Null);
        }
        finally
        {
            CSVExtensions.Provider = previous;
        }
    }

    [Test]
    public void Provider_CanBeReadWhileProviderIsChangedConcurrently()
    {
        var previous = CSVExtensions.Provider;
        var first = new SystemTextJsonHelper();
        const string missingProvider = "ISerialization provider was not set. Please initialize this before use";
        CSVExtensions.Provider = first;
        var failures = 0;

        try
        {
            Parallel.Invoke(
                () => Parallel.For(0, 20_000, index =>
                {
                    CSVExtensions.Provider = index % 2 == 0 ? null : first;
                    Thread.Yield();
                }),
                () => Parallel.For(0, 20_000, _ =>
                {
                    try
                    {
                        var value = """{"value":"ok"}""".FromCSV<Dictionary<string, string>>()
                            ?? throw new InvalidOperationException("The JSON provider returned null.");
                        if (value["value"] != "ok") Interlocked.Increment(ref failures);
                    }
                    catch (NullReferenceException ex) when (ex.Message == missingProvider)
                    {
                        // Calls that observe an unset provider fail with the documented guard.
                    }
                    catch
                    {
                        Interlocked.Increment(ref failures);
                    }
                }));
        }
        finally
        {
            CSVExtensions.Provider = previous;
        }

        Assert.That(failures, Is.Zero);
    }

    [Test]
    public async Task Reset_CancelsPendingDelay_AndAllowsAnotherDelay()
    {
        var timer = new DelayedTimer("reset", 0, 1, 2);
        var pending = timer.Delay();
        var cancellation = timer.CTS ?? throw new AssertionException("Delay did not create a cancellation source.");

        timer.Reset();

        Assert.That(cancellation.IsCancellationRequested, Is.True);
        Assert.ThrowsAsync<TaskCanceledException>(async () => await pending);
        Assert.DoesNotThrowAsync(async () => await timer.Delay());
    }

    [Test]
    public async Task ExternalCancellation_ResetsDelayBeforeNextWait()
    {
        var timer = new DelayedTimer("external-cancel", 0, 1, 3);
        var pending = timer.Delay();
        var cancellation = timer.CTS ?? throw new AssertionException("Delay did not create a cancellation source.");

        cancellation.Cancel();

        Assert.ThrowsAsync<TaskCanceledException>(async () => await pending);
        Assert.That(timer.CurrentDelay, Is.Zero);

        var next = timer.Delay();
        Assert.That(timer.CurrentDelay, Is.EqualTo(1));
        timer.Reset();
        Assert.ThrowsAsync<TaskCanceledException>(async () => await next);
    }

    [Test]
    public void TryRunAction_ContinuesAfterServiceFailure()
    {
        var manager = new AnalyticsManager();
        manager.AddService(DispatchProxy.Create<IAnalytics, NoOpAnalyticsProxy>());
        manager.AddService(DispatchProxy.Create<IAnalytics, NoOpAnalyticsProxy>());
        var calls = 0;

        var result = manager.TryRunAction(_ =>
        {
            calls++;
            if (calls == 1) throw new InvalidOperationException("first service failed");
        });

        Assert.That(result, Is.False);
        Assert.That(calls, Is.EqualTo(2));
    }

    [Test]
    public void TryRunAction_LogsServiceFailures()
    {
        var manager = new AnalyticsManager();
        manager.AddService(DispatchProxy.Create<IAnalytics, NoOpAnalyticsProxy>());
        var originalListeners = Trace.Listeners.Cast<TraceListener>().ToArray();
        using var writer = new StringWriter();
        using var listener = new TextWriterTraceListener(writer);

        try
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(listener);
            manager.TryRunAction(_ => throw new InvalidOperationException("analytics failure detail"));
            Trace.Flush();

            Assert.That(writer.ToString(), Does.Contain("analytics failure detail"));
        }
        finally
        {
            Trace.Listeners.Clear();
            foreach (var originalListener in originalListeners) Trace.Listeners.Add(originalListener);
        }
    }

    [Test]
    public async Task TryRunActionAsync_ContinuesAfterServiceFailure()
    {
        var manager = new AnalyticsManager();
        manager.AddService(DispatchProxy.Create<IAnalytics, NoOpAnalyticsProxy>());
        manager.AddService(DispatchProxy.Create<IAnalytics, NoOpAnalyticsProxy>());
        var calls = 0;

        var result = await manager.TryRunActionAsync(_ =>
        {
            calls++;
            return calls == 1
                ? Task.FromException(new InvalidOperationException("first service failed"))
                : Task.CompletedTask;
        });

        Assert.That(result, Is.False);
        Assert.That(calls, Is.EqualTo(2));
    }

    [Test]
    public void TryRunAction_UsesSnapshotWhenServicesAreAddedDuringIteration()
    {
        var manager = new AnalyticsManager();
        manager.AddService(DispatchProxy.Create<IAnalytics, NoOpAnalyticsProxy>());
        manager.AddService(DispatchProxy.Create<IAnalytics, NoOpAnalyticsProxy>());
        var calls = 0;

        var result = manager.TryRunAction(_ =>
        {
            calls++;
            if (calls == 1) manager.AddService(DispatchProxy.Create<IAnalytics, NoOpAnalyticsProxy>());
        });

        Assert.That(result, Is.True);
        Assert.That(calls, Is.EqualTo(2));
        Assert.That(manager.Services.Count(), Is.EqualTo(3));
    }

    [Test]
    public async Task SelectAsync_PropagatesSelectorExceptionsByDefault()
    {
        var source = new Subject<int>();
        var error = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
        source.SelectAsync<int, int>(_ => Task.FromException<int>(new InvalidOperationException("selector failed")))
            .Subscribe(_ => { }, ex => error.TrySetResult(ex));

        source.OnNext(1);

        Assert.That(await error.Task.WaitAsync(TimeSpan.FromSeconds(2)), Is.TypeOf<InvalidOperationException>());
    }

    [Test]
    public async Task DoAsync_PropagatesActionExceptionsByDefault()
    {
        var source = new Subject<int>();
        var error = new TaskCompletionSource<Exception>(TaskCreationOptions.RunContinuationsAsynchronously);
        source.DoAsync(_ => Task.FromException(new InvalidOperationException("action failed")))
            .Subscribe(_ => { }, ex => error.TrySetResult(ex));

        source.OnNext(1);

        Assert.That(await error.Task.WaitAsync(TimeSpan.FromSeconds(2)), Is.TypeOf<InvalidOperationException>());
    }

    private sealed class TestPayload { }

    public class NoOpAnalyticsProxy : DispatchProxy
    {
        protected override object? Invoke(System.Reflection.MethodInfo? targetMethod, object?[]? args) => null;
    }
}
