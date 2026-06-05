
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PolyhydraGames.Core.Models.DelayTimer;

/// <summary>
/// This class is designed to store state for a timer that has a maximum delay length, and every time another delay is requested, additional time is required between delays, until an exception is thrown.
/// </summary>
public class DelayedTimer
{
    public int StartingDelay { get; }
    public int Step { get; }
    public int MaxDelay { get; }
    public int CurrentDelay { get; private set; }
    public string Name { get; set; }

    public CancellationTokenSource? CTS { get; private set; }
    public DelayedTimer(string name, int startingDelay, int step, int maxDelay)
    {
        Name = name;
        StartingDelay = startingDelay;
        Step = step;
        MaxDelay = maxDelay; 
    }



    public async Task Delay()
    {
        if (CTS != null)
        {
            throw new Exception($"{Name} - This timer is intended to only be used by one action at a time");
        }

        CTS = new CancellationTokenSource();
        var nextDelay = CurrentDelay + Step;
        if (nextDelay > MaxDelay)
        {
            CurrentDelay = MaxDelay;
            CTS.Dispose();
            CTS = default;
            throw new MaximumDurationException(this);
        }

        CurrentDelay = nextDelay;

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(CurrentDelay), CTS.Token);
            CTS.Dispose();
            CTS = default;
        }
        catch
        {
            Reset();
            throw;
        }

    }

    public void Reset()
    {
        CTS = null;
        CurrentDelay = StartingDelay; 
    }
}