using System.Reactive.Linq;
using System.Reactive.Schedulers;

namespace System.Reactive.Extensions
{
    public static partial class ObservableExtensions
    {
        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source) => Observable.Timestamp(source);

        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source,
            IScheduler scheduler) => Observable.Timestamp(source, scheduler);

        public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source) =>
            Observable.TimeInterval(source);

        public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source,
            IScheduler scheduler) => Observable.TimeInterval(source, scheduler);

        public static IObservable<T> Delay<T>(this IObservable<T> source, TimeSpan dueTime) =>
            Observable.Delay(source, dueTime);

        public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, TimeSpan dueTime,
            IScheduler scheduler) => Observable.Delay(source, dueTime, scheduler);

        public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval) =>
            Observable.Sample(source, interval);

        public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval, IScheduler scheduler) =>
            Observable.Sample(source, interval, scheduler);

        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime) =>
            Observable.Throttle(source, dueTime);

        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime,
            IScheduler scheduler) => Observable.Throttle(source, dueTime, scheduler);

        public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime) =>
            Observable.ThrottleFirst(source, dueTime);

        public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime,
            IScheduler scheduler) => Observable.ThrottleFirst(source, dueTime, scheduler);

        public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime) => Observable.Timeout(source, dueTime);

        public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler) =>
            Observable.Timeout(source, dueTime, scheduler);

        public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime) =>
            Observable.Timeout(source, dueTime);

        public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime,
            IScheduler scheduler) => Observable.Timeout(source, dueTime, scheduler);
    }
}