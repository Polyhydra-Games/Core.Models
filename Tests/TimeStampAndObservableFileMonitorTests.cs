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

        Assert.DoesNotThrow(() => Parallel.For(0, 2_000, _ => TimeStamp.Stamp(name)));

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

        Assert.DoesNotThrow(() => Parallel.For(0, 2_001, _ => TimeStamp.Stamp(name)));

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

    // Concurrent lifecycle coverage: hammer Initialize() and Dispose() from many threads at
    // once on the same instance. This proves the _watcherGate lock actually serializes the
    // two lifecycle operations - no unexpected exceptions, no leaked watcher, and the
    // instance always ends up fully disposed once every racing call has returned.
    [Test]
    public void InitializeAndDispose_CalledConcurrently_NeverThrowUnexpectedlyAndEndDisposed()
    {
        var folder = Path.Combine(Path.GetTempPath(), $"core-models-monitor-race-{Guid.NewGuid():N}");
        Directory.CreateDirectory(folder);

        try
        {
            var monitor = new ObservableFileMonitor(folder, "watched.txt");
            var unexpected = new ConcurrentBag<Exception>();

            var actions = new List<Action>();
            for (var i = 0; i < 25; i++)
            {
                actions.Add(() =>
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
                });
            }

            for (var i = 0; i < 25; i++)
            {
                actions.Add(() =>
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
            }

            Parallel.Invoke(actions.ToArray());

            Assert.Multiple(() =>
            {
                Assert.That(unexpected, Is.Empty,
                    () => string.Join(Environment.NewLine, unexpected.Select(e => e.ToString())));
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
