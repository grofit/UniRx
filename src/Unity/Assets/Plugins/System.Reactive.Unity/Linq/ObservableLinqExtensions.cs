using System.Collections;
using System.Reactive.Extensions;
using System.Reactive.Linq;
using System.Reactive.Operators;
using UnityEngine;

namespace System.Reactive.Unity.Linq
{
    public static class ObservableLinqExtensions
    {
        public static IObservable<Vector2> Distinct(this IObservable<Vector2> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Vector2>();
            return new DistinctObservable<Vector2>(source, comparer);
        }

        public static IObservable<Vector3> Distinct(this IObservable<Vector3> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Vector3>();
            return new DistinctObservable<Vector3>(source, comparer);
        }

        public static IObservable<Vector4> Distinct(this IObservable<Vector4> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Vector4>();
            return new DistinctObservable<Vector4>(source, comparer);
        }

        public static IObservable<Quaternion> Distinct(this IObservable<Quaternion> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Quaternion>();
            return new DistinctObservable<Quaternion>(source, comparer);
        }

        public static IObservable<Color> Distinct(this IObservable<Color> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Color>();
            return new DistinctObservable<Color>(source, comparer);
        }

        public static IObservable<Rect> Distinct(this IObservable<Rect> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Rect>();
            return new DistinctObservable<Rect>(source, comparer);
        }

        public static IObservable<Bounds> Distinct(this IObservable<Bounds> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Bounds>();
            return new DistinctObservable<Bounds>(source, comparer);
        }

        public static IObservable<Vector2> Distinct<TKey>(this IObservable<Vector2> source, Func<Vector2, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctObservable<Vector2, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Vector3> Distinct<TKey>(this IObservable<Vector3> source, Func<Vector3, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctObservable<Vector3, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Vector4> Distinct<TKey>(this IObservable<Vector4> source, Func<Vector4, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctObservable<Vector4, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Quaternion> Distinct<TKey>(this IObservable<Quaternion> source, Func<Quaternion, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctObservable<Quaternion, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Color> Distinct<TKey>(this IObservable<Color> source, Func<Color, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctObservable<Color, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Rect> Distinct<TKey>(this IObservable<Rect> source, Func<Rect, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctObservable<Rect, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Bounds> Distinct<TKey>(this IObservable<Bounds> source, Func<Bounds, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctObservable<Bounds, TKey>(source, keySelector, comparer);
        }
        
        public static IObservable<Vector2> DistinctUntilChanged(this IObservable<Vector2> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Vector2>();
            return new DistinctUntilChangedObservable<Vector2>(source, comparer);
        }
        
        public static IObservable<Vector3> DistinctUntilChanged(this IObservable<Vector3> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Vector3>();
            return new DistinctUntilChangedObservable<Vector3>(source, comparer);
        }
        
        public static IObservable<Vector4> DistinctUntilChanged(this IObservable<Vector4> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Vector4>();
            return new DistinctUntilChangedObservable<Vector4>(source, comparer);
        }
        
        public static IObservable<Quaternion> DistinctUntilChanged(this IObservable<Quaternion> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Quaternion>();
            return new DistinctUntilChangedObservable<Quaternion>(source, comparer);
        }
        
        public static IObservable<Color> DistinctUntilChanged(this IObservable<Color> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Color>();
            return new DistinctUntilChangedObservable<Color>(source, comparer);
        }
        
        public static IObservable<Rect> DistinctUntilChanged(this IObservable<Rect> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Rect>();
            return new DistinctUntilChangedObservable<Rect>(source, comparer);
        }
        
        public static IObservable<Bounds> DistinctUntilChanged(this IObservable<Bounds> source)
        {
            var comparer = UnityEqualityComparer.GetDefault<Bounds>();
            return new DistinctUntilChangedObservable<Bounds>(source, comparer);
        }

        public static IObservable<Vector2> DistinctUntilChanged<TKey>(this IObservable<Vector2> source, Func<Vector2, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctUntilChangedObservable<Vector2, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Vector3> DistinctUntilChanged<TKey>(this IObservable<Vector3> source, Func<Vector3, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctUntilChangedObservable<Vector3, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Vector4> DistinctUntilChanged<TKey>(this IObservable<Vector4> source, Func<Vector4, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctUntilChangedObservable<Vector4, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Quaternion> DistinctUntilChanged<TKey>(this IObservable<Quaternion> source, Func<Quaternion, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctUntilChangedObservable<Quaternion, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Color> DistinctUntilChanged<TKey>(this IObservable<Color> source, Func<Color, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctUntilChangedObservable<Color, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Rect> DistinctUntilChanged<TKey>(this IObservable<Rect> source, Func<Rect, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctUntilChangedObservable<Rect, TKey>(source, keySelector, comparer);
        }

        public static IObservable<Bounds> DistinctUntilChanged<TKey>(this IObservable<Bounds> source, Func<Bounds, TKey> keySelector)
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return new DistinctUntilChangedObservable<Bounds, TKey>(source, keySelector, comparer);
        }

        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector) where TSource : struct
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return source.GroupBy(keySelector, elementSelector, comparer);
        }

        public static IObservable<IGroupedObservable<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this IObservable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, int capacity) where TSource : struct
        {
            var comparer = UnityEqualityComparer.GetDefault<TKey>();
            return source.GroupBy(keySelector, elementSelector, capacity, comparer);
        }

        public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, IEnumerator coroutine, bool publishEveryYield = false)
        {
            return source.SelectMany(Observable.FromCoroutine(() => coroutine, publishEveryYield));
        }

        public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<IEnumerator> selector, bool publishEveryYield = false)
        {
            return source.SelectMany(Observable.FromCoroutine(selector, publishEveryYield));
        }

        /// <summary>
        /// Note: publishEveryYield is always false. If you want to set true, use Observable.FromCoroutine(() => selector(x), true). This is workaround of Unity compiler's bug.
        /// </summary>
        public static IObservable<Unit> SelectMany<T>(this IObservable<T> source, Func<T, IEnumerator> selector)
        {
            return source.SelectMany(x => Observable.FromCoroutine(() => selector(x), false));
        }
    }
}