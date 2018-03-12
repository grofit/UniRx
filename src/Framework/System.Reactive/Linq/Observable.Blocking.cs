using System.Reactive.Operators;

namespace System.Reactive.Linq
{
    public partial class Observable
    {
        public static T Wait<T>(IObservable<T> source)
        {
            return new Wait<T>(source, Observable.InfiniteTimeSpan).Run();
        }

        public static T Wait<T>(IObservable<T> source, TimeSpan timeout)
        {
            return new Wait<T>(source, timeout).Run();
        }
    }
}
