
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PolyhydraGames.Core.Models.DelayTimer;

/// <summary>
/// This class is designed to store state for a timer that has a maximum delay length, and every time another delay is requested, additional time is required between delays, until an exception is thrown.
/// </summary>
public class DelayedTimer
{
    private readonly object _sync = new();
    private readonly TimeProvider _timeProvider;
    public int StartingDelay { get; }
    public int Step { get; }
    public int MaxDelay { get; }
    public int CurrentDelay { get; private set; }
    public string Name { get; set; }

    public CancellationTokenSource? CTS { get; private set; }
    public DelayedTimer(string name, int startingDelay, int step, int maxDelay)
        : this(name, startingDelay, step, maxDelay, TimeProvider.System)
    {
    }

    internal DelayedTimer(string name, int startingDelay, int step, int maxDelay, TimeProvider timeProvider)
    {
        _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        Name = name;
        StartingDelay = startingDelay;
        Step = step;
        MaxDelay = maxDelay;
    }



    public async Task Delay()
    {
        CancellationTokenSource cancellation;
        CancellationToken token;
        int nextDelay;
        lock (_sync)
        {
            if (CTS != null)
                throw new InvalidOperationException($"{Name} - This timer is intended to only be used by one action at a time");

            cancellation = new CancellationTokenSource();
            token = cancellation.Token;
            CTS = cancellation;
            nextDelay = CurrentDelay + Step;
            if (nextDelay > MaxDelay)
            {
                CurrentDelay = MaxDelay;
                CTS = null;
                cancellation.Dispose();
                throw new MaximumDurationException(this);
            }

            CurrentDelay = nextDelay;
        }

        try
        {
            await Task.Delay(TimeSpan.FromSeconds(nextDelay), _timeProvider, token);
        }
        catch
        {
            lock (_sync)
            {
                if (ReferenceEquals(CTS, cancellation))
                    CurrentDelay = StartingDelay;
            }

            throw;
        }
        finally
        {
            lock (_sync)
            {
                if (ReferenceEquals(CTS, cancellation))
                {
                    CTS = null;
                    cancellation.Dispose();
                }
            }
        }

    }

    public void Reset()
    {
        CancellationTokenSource? cancellation;
        lock (_sync)
        {
            cancellation = CTS;
            CTS = null;
            CurrentDelay = StartingDelay;
        }

        if (cancellation == null) return;

        // Cancel outside the lock so cancellation callbacks never run under _sync. Ownership was taken above
        // (CTS cleared), so Delay()'s finally block will not dispose it; Reset is the sole disposer.
        try
        {
            cancellation.Cancel();
        }
        finally
        {
            cancellation.Dispose();
        }
    }
}
