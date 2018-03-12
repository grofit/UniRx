using System.Collections.Generic;
using System.Reactive.Operators;
using System.Reactive.Schedulers;

namespace System.Reactive.Linq
{
    public static class ObservableErrorHandlingExtensions
    {
        /// <summary>
        /// <para>Repeats the source observable sequence until it successfully terminates.</para>
        /// <para>This is same as Retry().</para>
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource>(this IObservable<TSource> source) =>
            Observable.OnErrorRetry(source);

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError)
            where TException : Exception => Observable.OnErrorRetry(source, onError);

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence after delay time.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError, TimeSpan delay)
            where TException : Exception => Observable.OnErrorRetry(source, onError, delay);

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError, int retryCount)
            where TException : Exception => Observable.OnErrorRetry(source, onError, retryCount);

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence after delay time during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay)
            where TException : Exception => Observable.OnErrorRetry(source, onError, retryCount, delay);

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence after delay time(work on delayScheduler) during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            this IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay,
            IScheduler delayScheduler)
            where TException : Exception => Observable.OnErrorRetry(source, onError, retryCount, delay, delayScheduler);

        public static IObservable<T> Finally<T>(this IObservable<T> source, Action finallyAction) =>
            Observable.Finally(source, finallyAction);

        public static IObservable<T> Catch<T, TException>(this IObservable<T> source, Func<TException, IObservable<T>> errorHandler)
            where TException : Exception => Observable.Catch(source, errorHandler);

        public static IObservable<TSource> Catch<TSource>(this IEnumerable<IObservable<TSource>> sources) =>
            Observable.Catch(sources);

        /// <summary>Catch exception and return Observable.Empty.</summary>
        public static IObservable<TSource> CatchIgnore<TSource>(this IObservable<TSource> source) =>
            Observable.CatchIgnore(source);

        /// <summary>Catch exception and return Observable.Empty.</summary>
        public static IObservable<TSource> CatchIgnore<TSource, TException>(this IObservable<TSource> source, Action<TException> errorAction)
            where TException : Exception => Observable.CatchIgnore(source, errorAction);

        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source) => Observable.Retry(source);

        public static IObservable<TSource> Retry<TSource>(this IObservable<TSource> source, int retryCount) =>
            Observable.Retry(source, retryCount);
    }
}