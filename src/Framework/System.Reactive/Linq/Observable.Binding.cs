using System.Reactive.Operators;
using System.Reactive.Schedulers;
using System.Reactive.Subjects;

namespace System.Reactive.Linq
{
    public partial class Observable
    {
        public static IConnectableObservable<T> Multicast<T>(IObservable<T> source, ISubject<T> subject)
        {
            return new ConnectableObservable<T>(source, subject);
        }

        public static IConnectableObservable<T> Publish<T>(IObservable<T> source)
        {
            return source.Multicast(new Subject<T>());
        }

        public static IConnectableObservable<T> Publish<T>(IObservable<T> source, T initialValue)
        {
            return source.Multicast(new BehaviorSubject<T>(initialValue));
        }

        public static IConnectableObservable<T> PublishLast<T>(IObservable<T> source)
        {
            return source.Multicast(new AsyncSubject<T>());
        }

        public static IConnectableObservable<T> Replay<T>(IObservable<T> source)
        {
            return source.Multicast(new ReplaySubject<T>());
        }

        public static IConnectableObservable<T> Replay<T>(IObservable<T> source, IScheduler scheduler)
        {
            return source.Multicast(new ReplaySubject<T>(scheduler));
        }

        public static IConnectableObservable<T> Replay<T>(IObservable<T> source, int bufferSize)
        {
            return source.Multicast(new ReplaySubject<T>(bufferSize));
        }

        public static IConnectableObservable<T> Replay<T>(IObservable<T> source, int bufferSize, IScheduler scheduler)
        {
            return source.Multicast(new ReplaySubject<T>(bufferSize, scheduler));
        }

        public static IConnectableObservable<T> Replay<T>(IObservable<T> source, TimeSpan window)
        {
            return source.Multicast(new ReplaySubject<T>(window));
        }

        public static IConnectableObservable<T> Replay<T>(IObservable<T> source, TimeSpan window, IScheduler scheduler)
        {
            return source.Multicast(new ReplaySubject<T>(window, scheduler));
        }

        public static IConnectableObservable<T> Replay<T>(IObservable<T> source, int bufferSize, TimeSpan window, IScheduler scheduler)
        {
            return source.Multicast(new ReplaySubject<T>(bufferSize, window, scheduler));
        }

        public static IObservable<T> RefCount<T>(IConnectableObservable<T> source)
        {
            return new RefCountObservable<T>(source);
        }

        /// <summary>
        /// same as Publish().RefCount()
        /// </summary>
        public static IObservable<T> Share<T>(IObservable<T> source)
        {
            return source.Publish().RefCount();
        }
    }
}