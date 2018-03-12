namespace System.Reactive.Linq
{
    public static class ObservableBlockingExtensions
    {
        public static T Wait<T>(this IObservable<T> source) => Observable.Wait(source);
        public static T Wait<T>(this IObservable<T> source, TimeSpan timeout) => Observable.Wait(source, timeout);
    }
}
