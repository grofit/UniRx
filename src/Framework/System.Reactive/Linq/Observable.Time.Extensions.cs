using System.Reactive.Operators;
using System.Reactive.Schedulers;

namespace System.Reactive.Linq
{
    public static class ObservableTimeExtensions
    {
        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source)
        {
            return Timestamp<TSource>(source, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<Timestamped<TSource>> Timestamp<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            return new TimestampObservable<TSource>(source, scheduler);
        }

        public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source)
        {
            return TimeInterval(source, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<TimeInterval<TSource>> TimeInterval<TSource>(this IObservable<TSource> source, IScheduler scheduler)
        {
            return new TimeIntervalObservable<TSource>(source, scheduler);
        }

        public static IObservable<T> Delay<T>(this IObservable<T> source, TimeSpan dueTime)
        {
            return source.Delay(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<TSource> Delay<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return new DelayObservable<TSource>(source, dueTime, scheduler);
        }

        public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval)
        {
            return source.Sample(interval, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<T> Sample<T>(this IObservable<T> source, TimeSpan interval, IScheduler scheduler)
        {
            return new SampleObservable<T>(source, interval, scheduler);
        }

        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
        {
            return source.Throttle(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<TSource> Throttle<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return new ThrottleObservable<TSource>(source, dueTime, scheduler);
        }

        public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime)
        {
            return source.ThrottleFirst(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<TSource> ThrottleFirst<TSource>(this IObservable<TSource> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return new ThrottleFirstObservable<TSource>(source, dueTime, scheduler);
        }

        public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime)
        {
            return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<T> Timeout<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return new TimeoutObservable<T>(source, dueTime, scheduler);
        }

        public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime)
        {
            return source.Timeout(dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<T> Timeout<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            return new TimeoutObservable<T>(source, dueTime, scheduler);
        }
    }
}