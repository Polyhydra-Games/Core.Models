namespace PolyhydraGames.Core.Models.Test;

/// <summary>Deterministic <see cref="TimeProvider"/>: timers fire only when <see cref="Advance"/> is called.</summary>
internal sealed class ManualTimeProvider : TimeProvider
{
    private readonly object _gate = new();
    private readonly List<ManualTimer> _timers = new();
    private DateTimeOffset _now = new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);

    public override DateTimeOffset GetUtcNow() { lock (_gate) return _now; }

    public int ActiveTimerCount { get { lock (_gate) return _timers.Count; } }

    public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
    {
        var timer = new ManualTimer(this, callback, state);
        timer.Change(dueTime, period);
        return timer;
    }

    public void Advance(TimeSpan by)
    {
        List<ManualTimer> due;
        lock (_gate)
        {
            _now += by;
            due = _timers.Where(t => t.Due <= _now).ToList();
            foreach (var t in due) _timers.Remove(t);
        }
        foreach (var t in due) t.Fire();
    }

    private sealed class ManualTimer : ITimer
    {
        private readonly ManualTimeProvider _owner;
        private readonly TimerCallback _callback;
        private readonly object? _state;
        public DateTimeOffset Due;

        public ManualTimer(ManualTimeProvider owner, TimerCallback callback, object? state)
        {
            _owner = owner; _callback = callback; _state = state;
        }

        public void Fire() => _callback(_state);

        public bool Change(TimeSpan dueTime, TimeSpan period)
        {
            lock (_owner._gate)
            {
                _owner._timers.Remove(this);
                if (dueTime == Timeout.InfiniteTimeSpan) return true;
                Due = _owner._now + dueTime;
                _owner._timers.Add(this);
                return true;
            }
        }

        public void Dispose() { lock (_owner._gate) _owner._timers.Remove(this); }
        public ValueTask DisposeAsync() { Dispose(); return ValueTask.CompletedTask; }
    }
}
