using System;
using System.Collections.Generic;

namespace PolyhydraGames.Core.Models;

public class TimeStamp
{
    private static Dictionary<string, TimeStamp> _stamps = new ();

    public static void Stamp(string stamp)
    {
        if (_stamps.ContainsKey(stamp))
        {
            _stamps[stamp].Stamp();
            _stamps.Remove(stamp);
            return;
        }

        _stamps[stamp] = new TimeStamp(stamp);
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
