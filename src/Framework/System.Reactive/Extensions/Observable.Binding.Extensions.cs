using System.Reactive.Linq;
using System.Reactive.Schedulers;
using System.Reactive.Subjects;

namespace System.Reactive.Extensions
{
    public static partial class ObservableExtensions
    {
        public static IConnectableObservable<T> Multicast<T>(this IObservable<T> source, ISubject<T> subject) => Observable.Multicast(source, subject);
        public static IConnectableObservable<T> Publish<T>(this IObservable<T> source) => Observable.Publish(source);
        public static IConnectableObservable<T> Publish<T>(this IObservable<T> source, T initialValue) => Observable.Publish(source, initialValue);
        public static IConnectableObservable<T> PublishLast<T>(this IObservable<T> source) => Observable.PublishLast(source);
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source) => Observable.Replay(source);
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, IScheduler scheduler) => Observable.Replay(source, scheduler);
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize) => Observable.Replay(source, bufferSize);
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, IScheduler scheduler) => Observable.Replay(source, bufferSize, scheduler);
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window) => Observable.Replay(source, window);
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, TimeSpan window, IScheduler scheduler) => Observable.Replay(source, window, scheduler);
        public static IConnectableObservable<T> Replay<T>(this IObservable<T> source, int bufferSize, TimeSpan window, IScheduler scheduler) => Observable.Replay(source, bufferSize, window, scheduler);
        public static IObservable<T> RefCount<T>(this IConnectableObservable<T> source) => Observable.RefCount(source);

        /// <summary>
        /// same as Publish().RefCount()
        /// </summary>
        public static IObservable<T> Share<T>(this IObservable<T> source) => Observable.Share(source);
    }
}