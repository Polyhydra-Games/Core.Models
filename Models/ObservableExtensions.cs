using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace PolyhydraGames.Core.Models
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// Projects each element into an async operation and flattens the results.
        /// Any exception inside the async operation is sent downstream (or optionally logged).
        /// </summary>
        public static IObservable<TResult> SelectAsync<TSource, TResult>(
            this IObservable<TSource> source,
            Func<TSource, Task<TResult>> asyncSelector,
            Action<Exception>? onError = null)
        {
            return source.SelectMany(x =>
                Observable.FromAsync(() => asyncSelector(x))
                    .Catch<TResult, Exception>(ex =>
                    {
                        onError?.Invoke(ex);
                        return Observable.Empty<TResult>();
                    })
            );
        }

        /// <summary>
        /// Fire-and-forget async side effects, ignoring results.
        /// </summary>
        public static IObservable<Unit> DoAsync<TSource>(
            this IObservable<TSource> source,
            Func<TSource, Task> asyncAction,
            Action<Exception>? onError = null)
        {
            return source.SelectMany(x =>
                Observable.FromAsync(() => asyncAction(x))
                    .Select(_ => Unit.Default)
                    .Catch<Unit, Exception>(ex =>
                    {
                        onError?.Invoke(ex);
                        return Observable.Empty<Unit>();
                    })
            );
        }
    }
}
