#nullable enable
using System;

namespace PolyhydraGames.Core.Models.DelayTimer;

public class MaximumDurationException : Exception
{
    public DelayedTimer State { get; private set; }

    public MaximumDurationException(DelayedTimer timer) : base(GetErrorMessage(timer))
    {
        State = timer;
    }
    private static string GetErrorMessage(DelayedTimer timer)
    {
        return $"This timer attempted to run for {timer.CurrentDelay} seconds. Max: {timer.MaxDelay}";
    }
}
