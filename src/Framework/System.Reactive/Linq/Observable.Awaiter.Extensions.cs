using System.Reactive.Subjects;
using System.Threading;

namespace System.Reactive.Linq
{
    public static class ObservableAwaiterExtensions
    {
        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <param name="source">Source sequence to await.</param>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return Observable.RunAsync(source, CancellationToken.None);
        }

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <param name="source">Source sequence to await.</param>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IConnectableObservable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return Observable.RunAsync(source, CancellationToken.None);
        }

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <param name="source">Source sequence to await.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IObservable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null) throw new ArgumentNullException("source");

            return Observable.RunAsync(source, cancellationToken);
        }

        /// <summary>
        /// Gets an awaiter that returns the last value of the observable sequence or throws an exception if the sequence is empty.
        /// This operation subscribes to the observable sequence, making it hot.
        /// </summary>
        /// <param name="source">Source sequence to await.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public static AsyncSubject<TSource> GetAwaiter<TSource>(this IConnectableObservable<TSource> source, CancellationToken cancellationToken)
        {
            if (source == null) throw new ArgumentNullException("source");

            return Observable.RunAsync(source, cancellationToken);
        }
    }
}