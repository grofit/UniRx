using System.Reactive.Linq;
using System.Reactive.Operators;

namespace System.Reactive.Unity.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<TSource> Distinct<TSource>(this IObservable<TSource> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<TSource>();
            return new DistinctObservable<TSource>(source, comparer);
        }

        public static IObservable<TSource> Distinct<TSource, TKey>(this IObservable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctObservable<TSource, TKey>(source, keySelector, comparer);
        }

        public static IObservable<T> DistinctUntilChanged<T>(this IObservable<T> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<T>();
            return new DistinctUntilChangedObservable<T>(source, comparer);
        }

        public static IObservable<T> DistinctUntilChanged<T, TKey>(this IObservable<T> source, Func<T, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctUntilChangedObservable<T, TKey>(source, keySelector, comparer);
        }

        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return source.GroupBy(keySelector, elementSelector, comparer);
        }

        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return source.GroupBy(keySelector, elementSelector, capacity, comparer);
        }
    }
}