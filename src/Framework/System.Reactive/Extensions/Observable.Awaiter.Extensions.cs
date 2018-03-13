using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Extensions
{
    public static partial class ObservableExtensions
    {
        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <param name="source">Source sequence to await.</param>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IObservable<TSource> source) => Observable.GetAwaiter(source);

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <param name="source">Source sequence to await.</param>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IConnectableObservable<TSource> source) => Observable.GetAwaiter(source);

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <param name="source">Source sequence to await.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IObservable<TSource> source, CancellationToken cancellationToken) => Observable.GetAwaiter(source, cancellationToken);

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <param name="source">Source sequence to await.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IConnectableObservable<TSource> source, CancellationToken cancellationToken) => Observable.GetAwaiter(source, cancellationToken);
    }
}