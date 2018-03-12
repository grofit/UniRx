using System.Collections.Generic;
using System.Reactive.Operators;
using System.Reactive.Schedulers;

namespace System.Reactive.Linq
{
    public static class ObservableConcurrencyExtensions
    {
        public static IObservable<T> Synchronize<T>(this IObservable<T> source)
        {
            return new SynchronizeObservable<T>(source, new object());
        }

        public static IObservable<T> Synchronize<T>(this IObservable<T> source, object gate)
        {
            return new SynchronizeObservable<T>(source, gate);
        }

        public static IObservable<T> ObserveOn<T>(this IObservable<T> source, IScheduler scheduler)
        {
            return new ObserveOnObservable<T>(source, scheduler);
        }

        public static IObservable<T> SubscribeOn<T>(this IObservable<T> source, IScheduler scheduler)
        {
            return new SubscribeOnObservable<T>(source, scheduler);
        }

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime)
        {
            return new DelaySubscriptionObservable<T>(source, dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime, IScheduler scheduler)
        {
            return new DelaySubscriptionObservable<T>(source, dueTime, scheduler);
        }

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime)
        {
            return new DelaySubscriptionObservable<T>(source, dueTime, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime, IScheduler scheduler)
        {
            return new DelaySubscriptionObservable<T>(source, dueTime, scheduler);
        }

        public static IObservable<T> Amb<T>(this IObservable<T> source, IObservable<T> second)
        {
            return new AmbObservable<T>(source, second);
        }
    }
}