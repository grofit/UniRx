using System.Reactive.Linq;

namespace System.Reactive.Extensions
{
    public static partial class ObservableExtensions
    {
        public static IObservable<TSource> Scan<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator) => Observable.Scan(source, accumulator);
        public static IObservable<TAccumulate> Scan<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator) => Observable.Scan(source, seed, accumulator);
        public static IObservable<TSource> Aggregate<TSource>(this IObservable<TSource> source, Func<TSource, TSource, TSource> accumulator) => Observable.Aggregate(source, accumulator);
        public static IObservable<TAccumulate> Aggregate<TSource, TAccumulate>(this IObservable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator) => Observable.Aggregate(source, seed, accumulator);

        public static IObservable<TResult> Aggregate<TSource, TAccumulate, TResult>(this IObservable<TSource> source,
            TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator,
            Func<TAccumulate, TResult> resultSelector) => Observable.Aggregate(source, seed, accumulator, resultSelector);
    }
}
