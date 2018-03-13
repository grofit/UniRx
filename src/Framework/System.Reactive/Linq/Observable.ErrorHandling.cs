using System.Collections.Generic;
using System.Reactive.Extensions;
using System.Reactive.Operators;
using System.Reactive.Schedulers;

namespace System.Reactive.Linq
{
    public partial class Observable
    {
        /// <summary>
        /// <para>Repeats the source observable sequence until it successfully terminates.</para>
        /// <para>This is same as Retry().</para>
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource>(IObservable<TSource> source)
        {
            var result = source.Retry();
            return result;
        }

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            IObservable<TSource> source, Action<TException> onError)
            where TException : Exception
        {
            return source.OnErrorRetry(onError, TimeSpan.Zero);
        }

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence after delay time.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            IObservable<TSource> source, Action<TException> onError, TimeSpan delay)
            where TException : Exception
        {
            return source.OnErrorRetry(onError, int.MaxValue, delay);
        }

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            IObservable<TSource> source, Action<TException> onError, int retryCount)
            where TException : Exception
        {
            return source.OnErrorRetry(onError, retryCount, TimeSpan.Zero);
        }

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence after delay time during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay)
            where TException : Exception
        {
            return source.OnErrorRetry(onError, retryCount, delay, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        /// <summary>
        /// When catched exception, do onError action and repeat observable sequence after delay time(work on delayScheduler) during within retryCount.
        /// </summary>
        public static IObservable<TSource> OnErrorRetry<TSource, TException>(
            IObservable<TSource> source, Action<TException> onError, int retryCount, TimeSpan delay, IScheduler delayScheduler)
            where TException : Exception
        {
            var result = System.Reactive.Linq.Observable.Defer(() =>
            {
                var dueTime = (delay.Ticks < 0) ? TimeSpan.Zero : delay;
                var count = 0;

                IObservable<TSource> self = null;
                self = source.Catch((TException ex) =>
                {
                    onError(ex);

                    return (++count < retryCount)
                        ? (dueTime == TimeSpan.Zero)
                            ? self.SubscribeOn(Scheduler.CurrentThread)
                            : self.DelaySubscription(dueTime, delayScheduler).SubscribeOn(Scheduler.CurrentThread)
                        : System.Reactive.Linq.Observable.Throw<TSource>(ex);
                });
                return self;
            });

            return result;
        }

        public static IObservable<T> Finally<T>(IObservable<T> source, Action finallyAction)
        {
            return new FinallyObservable<T>(source, finallyAction);
        }

        public static IObservable<T> Catch<T, TException>(IObservable<T> source, Func<TException, IObservable<T>> errorHandler)
            where TException : Exception
        {
            return new CatchObservable<T, TException>(source, errorHandler);
        }

        public static IObservable<TSource> Catch<TSource>(IEnumerable<IObservable<TSource>> sources)
        {
            return new CatchObservable<TSource>(sources);
        }

        /// <summary>Catch exception and return Observable.Empty.</summary>
        public static IObservable<TSource> CatchIgnore<TSource>(IObservable<TSource> source)
        {
            return source.Catch<TSource, Exception>(Stubs.CatchIgnore<TSource>);
        }

        /// <summary>Catch exception and return Observable.Empty.</summary>
        public static IObservable<TSource> CatchIgnore<TSource, TException>(IObservable<TSource> source, Action<TException> errorAction)
            where TException : Exception
        {
            var result = source.Catch((TException ex) =>
            {
                errorAction(ex);
                return System.Reactive.Linq.Observable.Empty<TSource>();
            });
            return result;
        }

        public static IObservable<TSource> Retry<TSource>(IObservable<TSource> source)
        {
            return Observable.RepeatInfinite(source).Catch();
        }

        public static IObservable<TSource> Retry<TSource>(IObservable<TSource> source, int retryCount)
        {
            return System.Linq.Enumerable.Repeat(source, retryCount).Catch();
        }
    }
}