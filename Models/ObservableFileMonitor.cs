using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models;

public class ObservableFileMonitor : IObservableFileMonitor, IDisposable
{
    public Subject<string> FileSubject { get; } = new Subject<string>();
    public IObservable<string> Observable { get; }

    // Specify the file to monitor
    private readonly string _folder;
    private readonly object _watcherGate = new();
    private FileSystemWatcher? _watcher;
    private bool _disposed;
    public readonly string FilePath;
    public ObservableFileMonitor(string folder, string file)
    {
        _folder = folder;
        FilePath = Path.Combine(folder, file);
        Observable = FileSubject.AsObservable();
    }

    public void Initialize()
    {
        OnInitializeStarting();

        lock (_watcherGate)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (_watcher is not null)
                return;

            var watcher = CreateWatcher();
            watcher.Path = _folder;
            watcher.Filter = Path.GetFileName(FilePath);

            // Subscribe before enabling notifications so no change is missed.
            watcher.Changed += OnFileChanged;

            try
            {
                EnableRaisingEvents(watcher);
                _watcher = watcher;
            }
            catch
            {
                watcher.Changed -= OnFileChanged;
                watcher.Dispose();
                throw;
            }
        }

        Debug.WriteLine($"Monitoring changes to {FilePath}. Press Enter to exit.");
    }

    protected virtual void OnInitializeStarting()
    {
    }

    protected virtual FileSystemWatcher CreateWatcher()
    {
        return new FileSystemWatcher();
    }

    protected virtual void EnableRaisingEvents(FileSystemWatcher watcher)
    {
        watcher.EnableRaisingEvents = true;
    }

    public void Dispose()
    {
        FileSystemWatcher? watcher;

        lock (_watcherGate)
        {
            if (_disposed)
                return;

            _disposed = true;
            watcher = _watcher;
            _watcher = null;
        }

        if (watcher is not null)
        {
            watcher.Changed -= OnFileChanged;
            watcher.Dispose();
        }

        FileSubject.OnCompleted();
        FileSubject.Dispose();
        GC.SuppressFinalize(this);
    }

    private void OnFileChanged(object sender, FileSystemEventArgs eventArgs)
    {
        lock (_watcherGate)
        {
            if (_disposed)
                return;

            FileSubject.OnNext(eventArgs.FullPath);
            Debug.WriteLine($"File changed: {eventArgs.FullPath}");
        }
    }
}
