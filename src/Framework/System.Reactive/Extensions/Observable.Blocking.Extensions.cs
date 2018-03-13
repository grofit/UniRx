using System.Reactive.Linq;

namespace System.Reactive.Extensions
{
    public static partial class ObservableExtensions
    {
        public static T Wait<T>(this IObservable<T> source) => Observable.Wait(source);
        public static T Wait<T>(this IObservable<T> source, TimeSpan timeout) => Observable.Wait(source, timeout);
    }
}
