using System.Collections.Generic;
using System.Linq;
using System.Reactive.Operators;
using System.Reactive.Schedulers;

namespace System.Reactive.Linq
{
    public static class ObservableConcatenationExtensions
    {
        public static IObservable<TSource> Concat<TSource>(this IEnumerable<IObservable<TSource>> sources) =>
            Observable.Concat(sources);

        public static IObservable<TSource> Concat<TSource>(this IObservable<IObservable<TSource>> sources) =>
            Observable.Concat(sources);

        public static IObservable<TSource> Concat<TSource>(this IObservable<TSource> first,
            params IObservable<TSource>[] seconds) => Observable.Concat(first, seconds);

        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources) =>
            Observable.Merge(sources);

        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources,
            IScheduler scheduler) => Observable.Merge(sources, scheduler);

        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources,
            int maxConcurrent) => Observable.Merge(sources, maxConcurrent);

        public static IObservable<TSource> Merge<TSource>(this IEnumerable<IObservable<TSource>> sources,
            int maxConcurrent, IScheduler scheduler) => Observable.Merge(sources, maxConcurrent, scheduler);

        public static IObservable<T> Merge<T>(this IObservable<T> first, params IObservable<T>[] seconds) =>
            Observable.Merge(first, seconds);

        public static IObservable<T> Merge<T>(this IObservable<T> first, IObservable<T> second, IScheduler scheduler) =>
            Observable.Merge(first, second, scheduler);

        public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources) => Observable.Merge(sources);

        public static IObservable<T> Merge<T>(this IObservable<IObservable<T>> sources, int maxConcurrent) =>
            Observable.Merge(sources, maxConcurrent);

        public static IObservable<TResult> Zip<TLeft, TRight, TResult>(this IObservable<TLeft> left,
            IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) => Observable.Zip(left, right, selector);

        public static IObservable<IList<T>> Zip<T>(this IEnumerable<IObservable<T>> sources) => Observable.Zip(sources);

        public static IObservable<TR> Zip<T1, T2, T3, TR>(this IObservable<T1> source1, IObservable<T2> source2,
            IObservable<T3> source3, ZipFunc<T1, T2, T3, TR> resultSelector) =>
            Observable.Zip(source1, source2, source3, resultSelector);

        public static IObservable<TR> Zip<T1, T2, T3, T4, TR>(this IObservable<T1> source1, IObservable<T2> source2,
            IObservable<T3> source3, IObservable<T4> source4, ZipFunc<T1, T2, T3, T4, TR> resultSelector) =>
            Observable.Zip(source1, source2, source3, source4, resultSelector);

        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1, IObservable<T2> source2,
            IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            ZipFunc<T1, T2, T3, T4, T5, TR> resultSelector) =>
            Observable.Zip(source1, source2, source3, source4, source5, resultSelector);

        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            IObservable<T6> source6, ZipFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector) =>
            Observable.Zip(source1, source2, source3, source4, source5, source6, resultSelector);

        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            IObservable<T6> source6, IObservable<T7> source7, ZipFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector) =>
            Observable.Zip(source1, source2, source3, source4, source5, source6, source7, resultSelector);

        public static IObservable<TResult> CombineLatest<TLeft, TRight, TResult>(this IObservable<TLeft> left,
            IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) =>
            Observable.CombineLatest(left, right, selector);

        public static IObservable<IList<T>> CombineLatest<T>(this IEnumerable<IObservable<T>> sources) =>
            Observable.CombineLatest(sources);

        public static IObservable<TR> CombineLatest<T1, T2, T3, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, CombineLatestFunc<T1, T2, T3, TR> resultSelector) =>
            Observable.CombineLatest(source1, source2, source3, resultSelector);

        public static IObservable<TR> CombineLatest<T1, T2, T3, T4, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4,
            CombineLatestFunc<T1, T2, T3, T4, TR> resultSelector) =>
            Observable.CombineLatest(source1, source2, source3, source4, resultSelector);

        public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            CombineLatestFunc<T1, T2, T3, T4, T5, TR> resultSelector) =>
            Observable.CombineLatest(source1, source2, source3, source4, source5, resultSelector);

        public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            IObservable<T6> source6, CombineLatestFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector) =>
            Observable.CombineLatest(source1, source2, source3, source4, source5, source6, resultSelector);

        public static IObservable<TR> CombineLatest<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            IObservable<T6> source6, IObservable<T7> source7,
            CombineLatestFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector) => Observable.CombineLatest(source1,
            source2, source3, source4, source5, source6, source7, resultSelector);

        public static IObservable<TResult> ZipLatest<TLeft, TRight, TResult>(this IObservable<TLeft> left,
            IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) =>
            Observable.ZipLatest(left, right, selector);

        public static IObservable<IList<T>> ZipLatest<T>(this IEnumerable<IObservable<T>> sources) =>
            Observable.ZipLatest(sources);

        public static IObservable<TR> ZipLatest<T1, T2, T3, TR>(this IObservable<T1> source1, IObservable<T2> source2,
            IObservable<T3> source3, ZipLatestFunc<T1, T2, T3, TR> resultSelector) =>
            Observable.ZipLatest(source1, source2, source3, resultSelector);

        public static IObservable<TR> ZipLatest<T1, T2, T3, T4, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4,
            ZipLatestFunc<T1, T2, T3, T4, TR> resultSelector) =>
            Observable.ZipLatest(source1, source2, source3, source4, resultSelector);

        public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            ZipLatestFunc<T1, T2, T3, T4, T5, TR> resultSelector) =>
            Observable.ZipLatest(source1, source2, source3, source4, source5, resultSelector);

        public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, T6, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            IObservable<T6> source6, ZipLatestFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector) =>
            Observable.ZipLatest(source1, source2, source3, source4, source5, source6, resultSelector);

        public static IObservable<TR> ZipLatest<T1, T2, T3, T4, T5, T6, T7, TR>(this IObservable<T1> source1,
            IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5,
            IObservable<T6> source6, IObservable<T7> source7,
            ZipLatestFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector) => Observable.ZipLatest(source1, source2,
            source3, source4, source5, source6, source7, resultSelector);

        public static IObservable<T> Switch<T>(this IObservable<IObservable<T>> sources) => Observable.Switch(sources);

        public static IObservable<TResult> WithLatestFrom<TLeft, TRight, TResult>(this IObservable<TLeft> left,
            IObservable<TRight> right, Func<TLeft, TRight, TResult> selector) =>
            Observable.WithLatestFrom(left, right, selector);

        /// <summary>
        /// <para>Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// <para>If sequence is empty, return T[0] array.</para>
        /// </summary>
        public static IObservable<T[]> WhenAll<T>(this IEnumerable<IObservable<T>> sources) =>
            Observable.WhenAll(sources);

        /// <summary>
        /// <para>Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// </summary>
        public static IObservable<Unit> WhenAll(this IEnumerable<IObservable<Unit>> sources) =>
            Observable.WhenAll(sources);

        public static IObservable<T> StartWith<T>(this IObservable<T> source, T value) =>
            Observable.StartWith(source, value);

        public static IObservable<T> StartWith<T>(this IObservable<T> source, Func<T> valueFactory) =>
            Observable.StartWith(source, valueFactory);

        public static IObservable<T> StartWith<T>(this IObservable<T> source, params T[] values) =>
            Observable.StartWith(source, values);

        public static IObservable<T> StartWith<T>(this IObservable<T> source, IEnumerable<T> values) =>
            Observable.StartWith(source, values);

        public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler, T value) =>
            Observable.StartWith(source, scheduler, value);

        public static IObservable<T> StartWith<T>(this IObservable<T> source, IScheduler scheduler,
            IEnumerable<T> values) => Observable.StartWith(source, scheduler, values);

        public static IObservable<T>
            StartWith<T>(this IObservable<T> source, IScheduler scheduler, params T[] values) =>
            Observable.StartWith(source, scheduler, values);
    }
}