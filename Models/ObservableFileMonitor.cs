using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PolyhydraGames.Core.Interfaces;

namespace PolyhydraGames.Core.Models;

public class ObservableFileMonitor : IObservableFileMonitor
{
    public Subject<string> FileSubject { get; } = new Subject<string>();
    public IObservable<string> Observable { get; }

    // Specify the file to monitor
    private readonly string _folder;
    public readonly string FilePath;
    public ObservableFileMonitor(string folder, string file)
    {
        _folder = folder;
        FilePath = Path.Combine(folder, file);
        Observable = FileSubject.AsObservable();
    }

    public void Initialize()
    {
        // Create a new FileSystemWatcher
        var watcher = new FileSystemWatcher()
        {
            Path = _folder,

            // Set the filter to the file name
            Filter = Path.GetFileName(FilePath),

            // Enable the FileSystemWatcher
            EnableRaisingEvents = true
        };

        // Set the path to the directory containing the file

        // Subscribe to the Changed event
        watcher.Changed += (o, x) =>
        {
            var file = x.FullPath;
            FileSubject.OnNext(file);
            // Handle the file change event
            Debug.WriteLine($"File changed: {x.FullPath}");
        };


        Debug.WriteLine($"Monitoring changes to {FilePath}. Press Enter to exit.");
    }
}
