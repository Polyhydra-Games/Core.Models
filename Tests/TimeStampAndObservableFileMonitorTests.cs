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
}
