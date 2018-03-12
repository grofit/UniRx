using System.Reactive.Operators;

namespace System.Reactive.Linq
{
    public static class ObservableBlockingExtensions
    {
        public static T Wait<T>(this IObservable<T> source)
        {
            return new Wait<T>(source, Observable.InfiniteTimeSpan).Run();
        }

        public static T Wait<T>(this IObservable<T> source, TimeSpan timeout)
        {
            return new Wait<T>(source, timeout).Run();
        }
    }
}
