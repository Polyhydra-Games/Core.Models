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
        /// Exceptions are sent downstream by default. Supplying an error callback handles and drops them.
        /// </summary>
        public static IObservable<TResult> SelectAsync<TSource, TResult>(
            this IObservable<TSource> source,
            Func<TSource, Task<TResult>> asyncSelector,
            Action<Exception>? onError = null)
        {
            return onError == null
                ? source.SelectMany(x => Observable.FromAsync(() => asyncSelector(x)))
                : source.SelectMany(x => Observable.FromAsync(() => asyncSelector(x))
                    .Catch<TResult, Exception>(ex =>
                    {
                        onError(ex);
                        return Observable.Empty<TResult>();
                    }));
        }

        /// <summary>
        /// Async side effects whose failures are sent downstream by default. Supplying an error callback handles and drops them.
        /// </summary>
        public static IObservable<Unit> DoAsync<TSource>(
            this IObservable<TSource> source,
            Func<TSource, Task> asyncAction,
            Action<Exception>? onError = null)
        {
            return onError == null
                ? source.SelectMany(x => Observable.FromAsync(() => asyncAction(x)).Select(_ => Unit.Default))
                : source.SelectMany(x => Observable.FromAsync(() => asyncAction(x))
                    .Select(_ => Unit.Default)
                    .Catch<Unit, Exception>(ex =>
                    {
                        onError(ex);
                        return Observable.Empty<Unit>();
                    }));
        }
    }
}
