using System.Collections.Generic;

namespace System.Reactive.Linq
{
    public partial class Observable
    {
        public static IObservable<T> Amb<T>(params IObservable<T>[] sources)
        {
            return Amb((IEnumerable<IObservable<T>>)sources);
        }

        public static IObservable<T> Amb<T>(IEnumerable<IObservable<T>> sources)
        {
            var result = Observable.Never<T>();
            foreach (var item in sources)
            {
                var second = item;
                result = result.Amb(second);
            }
            return result;
        }
    }
}