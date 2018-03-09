using System.Reactive.Operators;
using System.Reactive.Schedulers;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        public static IObservable<long> Interval(TimeSpan period)
        {
            return new TimerObservable(period, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<long> Interval(TimeSpan period, IScheduler scheduler)
        {
            return new TimerObservable(period, period, scheduler);
        }

        public static IObservable<long> Timer(TimeSpan dueTime)
        {
            return new TimerObservable(dueTime, null, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<long> Timer(DateTimeOffset dueTime)
        {
            return new TimerObservable(dueTime, null, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period)
        {
            return new TimerObservable(dueTime, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period)
        {
            return new TimerObservable(dueTime, period, Scheduler.DefaultSchedulers.TimeBasedOperations);
        }

        public static IObservable<long> Timer(TimeSpan dueTime, IScheduler scheduler)
        {
            return new TimerObservable(dueTime, null, scheduler);
        }

        public static IObservable<long> Timer(DateTimeOffset dueTime, IScheduler scheduler)
        {
            return new TimerObservable(dueTime, null, scheduler);
        }

        public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan period, IScheduler scheduler)
        {
            return new TimerObservable(dueTime, period, scheduler);
        }

        public static IObservable<long> Timer(DateTimeOffset dueTime, TimeSpan period, IScheduler scheduler)
        {
            return new TimerObservable(dueTime, period, scheduler);
        }

        
    }
}