using System.Reactive.Schedulers;

namespace System.Reactive.Extensions
{
    public static partial class ObservableExtensions
    {
        public static bool IsRequiredSubscribeOnCurrentThread<T>(this IObservable<T> source)
        {
            if (!(source is IOptimizedObservable<T> obs)) return true;
            return obs.IsRequiredSubscribeOnCurrentThread();
        }

        public static bool IsRequiredSubscribeOnCurrentThread<T>(this IObservable<T> source, IScheduler scheduler)
        {
            if (scheduler == Scheduler.CurrentThread) return true;
            return IsRequiredSubscribeOnCurrentThread(source);
        }
    }
}