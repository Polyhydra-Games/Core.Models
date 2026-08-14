using System;
using System.Collections.Concurrent;

namespace PolyhydraGames.Core.Models;

public class TimeStamp
{
    private static readonly ConcurrentDictionary<string, TimeStamp> _stamps = new ();

    public static void Stamp(string stamp)
    {
        while (true)
        {
            if (_stamps.TryAdd(stamp, new TimeStamp(stamp)))
                return;

            if (_stamps.TryRemove(stamp, out var startedStamp))
            {
                startedStamp.Stamp();
                return;
            }
        }
    }
    public TimeStamp(string name)
    {
        Name = name;
        StartTime = DateTime.Now;

    }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime LastStamp { get; set; }
    public TimeSpan Duration => LastStamp - StartTime;

    public string Stamp()
    {
        LastStamp = DateTime.Now; 
        return ToString();
    }
    public override string ToString()
    {
        return $"{Name} Stamp Finished -  Start Time: {StartTime} LastStamp: {LastStamp} Duration: {Duration}";
    }
}
