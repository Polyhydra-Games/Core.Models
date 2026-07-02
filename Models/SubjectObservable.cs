using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace PolyhydraGames.Core.Models;

public class SubjectObservable<T> : IObservable<T>
{
    private readonly bool _checkEquality;
    private T _value = default!;

    public T Value
    {
        get => _value;

    }

    public bool SetValue(T value)
    {
        if (_checkEquality && EqualityComparer<T>.Default.Equals(_value, value))
        {

            return false;
        }


        _value = value;
        _subject.OnNext(value);
        return true;
    }

    private readonly Subject<T> _subject = new();
    private readonly IObservable<T> _observable;
    /// <summary>
    /// Initializer
    /// </summary>
    /// <param name="checkEquality">This determines if we should update the object and notify every time a value is sent regardless of equality</param>
    public SubjectObservable(bool checkEquality = true)
    {
        _checkEquality = checkEquality;
        _observable = _subject.AsObservable();
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return _observable.Subscribe(observer);
    }
    public void Dispose()
    {
        _subject.Dispose();
    }

}
