using System.Reactive.Linq;
using System.Reactive.Schedulers;

namespace System.Reactive.Extensions
{
    public static partial class ObservableExtensions
    {
        public static IObservable<T> Synchronize<T>(this IObservable<T> source) => Observable.Synchronize(source);

        public static IObservable<T> Synchronize<T>(this IObservable<T> source, object gate) =>
            Observable.Synchronize(source, gate);

        public static IObservable<T> ObserveOn<T>(this IObservable<T> source, IScheduler scheduler) =>
            Observable.ObserveOn(source, scheduler);

        public static IObservable<T> SubscribeOn<T>(this IObservable<T> source, IScheduler scheduler) =>
            Observable.SubscribeOn(source, scheduler);

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime) =>
            Observable.DelaySubscription(source, dueTime);

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, TimeSpan dueTime,
            IScheduler scheduler) => Observable.DelaySubscription(source, dueTime, scheduler);

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime) =>
            Observable.DelaySubscription(source, dueTime);

        public static IObservable<T> DelaySubscription<T>(this IObservable<T> source, DateTimeOffset dueTime,
            IScheduler scheduler) => Observable.DelaySubscription(source, dueTime, scheduler);

        public static IObservable<T> Amb<T>(this IObservable<T> source, IObservable<T> second) =>
            Observable.Amb(source, second);
    }
}