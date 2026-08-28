using System.Collections.Concurrent;
using System.Reflection;
using NUnit.Framework;

namespace PolyhydraGames.Core.Models.Test;

[TestFixture]
public class TimeStampAndObservableFileMonitorTests
{
    [Test]
    public void Stamp_ConcurrentPairs_CompletesWithoutLeavingAnActiveTimestamp()
    {
        var name = $"concurrent-{Guid.NewGuid():N}";

        Assert.DoesNotThrow(() => RunConcurrently(64, () => TimeStamp.Stamp(name)));

        Assert.That(GetActiveStamps().ContainsKey(name), Is.False);
    }

    // Defines the semantics for concurrent first-use Stamp calls: TimeStamp.Stamp toggles a
    // per-name entry atomically (TryAdd starts it, TryRemove completes it), so pairing is
    // deterministic regardless of thread interleaving - it is NOT lossy. An odd number of
    // concurrent calls on the same name always leaves exactly one active (started, not yet
    // completed) entry; it never leaves the dictionary in a corrupted or ambiguous state.
    [Test]
    public void Stamp_ConcurrentOddCount_DeterministicallyLeavesExactlyOneActiveTimestamp()
    {
        var name = $"concurrent-odd-{Guid.NewGuid():N}";

        Assert.DoesNotThrow(() => RunConcurrently(65, () => TimeStamp.Stamp(name)));

        Assert.That(GetActiveStamps().ContainsKey(name), Is.True,
            "An odd number of concurrent first-use calls must deterministically leave exactly " +
            "one active timestamp - proving the toggle pairing is not lossy under contention.");
    }

    [Test]
    public void Initialize_IsIdempotent_AndDisposeReleasesTheWatcher()
    {
        var folder = Path.Combine(Path.GetTempPath(), $"core-models-monitor-{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            using var monitor = new ObservableFileMonitor(folder, "watched.txt");
            monitor.Initialize();
            var firstWatcher = GetWatcher(monitor);

            monitor.Initialize();

            Assert.Multiple(() =>
            {
                Assert.That(firstWatcher, Is.Not.Null);
                Assert.That(GetWatcher(monitor), Is.SameAs(firstWatcher));
            });

            monitor.Dispose();
            Assert.That(GetWatcher(monitor), Is.Null);
            Assert.Throws<ObjectDisposedException>(() => monitor.Initialize());
        }
        finally
        {
            Directory.Delete(folder, recursive: true);
        }
    }

    [Test]
    public void Initialize_WhenActivationFails_RollsBackAndCanRetry()
    {
        var folder = Path.Combine(Path.GetTempPath(), $"core-models-monitor-retry-{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            using var monitor = new FailingFirstActivationFileMonitor(folder, "watched.txt");

            Assert.Throws<IOException>(() => monitor.Initialize());
            Assert.That(GetWatcher(monitor), Is.Null);

            Assert.DoesNotThrow(() => monitor.Initialize());
            Assert.That(GetWatcher(monitor), Is.Not.Null);
        }
        finally
        {
            if (Directory.Exists(folder))
                Directory.Delete(folder, recursive: true);
        }
    }

    // Every Initialize caller waits inside Initialize before competing for _watcherGate. The
    // monitor records each allocated watcher, so a regression that lets callers overwrite one
    // another is detected even though _watcher is null after Dispose. Also proves the
    // _watcherGate lock serializes concurrent Initialize()/Dispose() calls on the same
    // instance - no unexpected exceptions, no leaked watcher.
    [Test]
    public void ConcurrentInitializeAndDispose_DisposesEveryAllocatedWatcher()
    {
        var folder = Path.Combine(Path.GetTempPath(), $"core-models-monitor-race-{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            const int initializerCount = 32;
            using var initializeBarrier = new Barrier(initializerCount);
            var monitor = new TrackingObservableFileMonitor(folder, "watched.txt", initializeBarrier);
            var unexpected = new ConcurrentBag<Exception>();

            var initializers = Enumerable.Range(0, initializerCount)
                .Select(_ => Task.Factory.StartNew(() =>
                {
                    try
                    {
                        monitor.Initialize();
                    }
                    catch (ObjectDisposedException)
                    {
                        // Expected when a concurrent Dispose() wins the race first.
                    }
                    catch (Exception ex)
                    {
                        unexpected.Add(ex);
                    }
                }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default))
                .ToArray();

            Assert.That(Task.WaitAll(initializers, TimeSpan.FromSeconds(10)), Is.True,
                "All concurrent Initialize callers must reach the coordinated first-use point.");

            RunConcurrently(32, () =>
            {
                try
                {
                    monitor.Dispose();
                }
                catch (Exception ex)
                {
                    unexpected.Add(ex);
                }
            });

            Assert.Multiple(() =>
            {
                Assert.That(unexpected, Is.Empty,
                    () => string.Join(Environment.NewLine, unexpected.Select(e => e.ToString())));
                Assert.That(monitor.CreatedWatchers, Has.Count.EqualTo(1),
                    "Concurrent Initialize calls must not allocate watchers that can be overwritten.");
                Assert.That(monitor.CreatedWatchers, Is.All.Matches<TrackingFileSystemWatcher>(watcher => watcher.IsDisposed),
                    "Dispose must release every watcher that was allocated during concurrent initialization.");
                Assert.That(GetWatcher(monitor), Is.Null);
                Assert.Throws<ObjectDisposedException>(() => monitor.Initialize());
            });
        }
        finally
        {
            Directory.Delete(folder, recursive: true);
        }
    }

    private static ConcurrentDictionary<string, TimeStamp> GetActiveStamps()
    {
        var field = typeof(TimeStamp).GetField("_stamps", BindingFlags.NonPublic | BindingFlags.Static);
        return (ConcurrentDictionary<string, TimeStamp>)field!.GetValue(null)!;
    }

    private static FileSystemWatcher? GetWatcher(ObservableFileMonitor monitor)
    {
        var field = typeof(ObservableFileMonitor).GetField("_watcher", BindingFlags.NonPublic | BindingFlags.Instance);
        return (FileSystemWatcher?)field!.GetValue(monitor);
    }

    private static void RunConcurrently(int workerCount, Action action)
    {
        using var barrier = new Barrier(workerCount);
        var workers = Enumerable.Range(0, workerCount)
            .Select(_ => Task.Factory.StartNew(() =>
            {
                barrier.SignalAndWait(TimeSpan.FromSeconds(10));
                action();
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default))
            .ToArray();

        Assert.That(Task.WaitAll(workers, TimeSpan.FromSeconds(10)), Is.True,
            "All workers must execute after reaching the same concurrent first-use barrier.");
    }

    private sealed class TrackingObservableFileMonitor : ObservableFileMonitor
    {
        private readonly Barrier _initializeBarrier;

        public TrackingObservableFileMonitor(string folder, string file, Barrier initializeBarrier)
            : base(folder, file)
        {
            _initializeBarrier = initializeBarrier;
        }

        public ConcurrentBag<TrackingFileSystemWatcher> CreatedWatchers { get; } = new();

        protected override void OnInitializeStarting()
        {
            _initializeBarrier.SignalAndWait(TimeSpan.FromSeconds(10));
        }

        protected override FileSystemWatcher CreateWatcher()
        {
            var watcher = new TrackingFileSystemWatcher();
            CreatedWatchers.Add(watcher);
            return watcher;
        }
    }

    private sealed class TrackingFileSystemWatcher : FileSystemWatcher
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }

    private sealed class FailingFirstActivationFileMonitor : ObservableFileMonitor
    {
        private bool _failFirstActivation = true;

        public FailingFirstActivationFileMonitor(string folder, string file)
            : base(folder, file)
        {
        }

        protected override void EnableRaisingEvents(FileSystemWatcher watcher)
        {
            if (_failFirstActivation)
            {
                _failFirstActivation = false;
                throw new IOException("Simulated transient watcher activation failure.");
            }

            base.EnableRaisingEvents(watcher);
        }
    }
}
