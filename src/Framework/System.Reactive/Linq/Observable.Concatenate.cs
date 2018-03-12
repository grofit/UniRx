﻿using System.Collections.Generic;
using System.Linq;
using System.Reactive.Operators;
using System.Reactive.Schedulers;

namespace System.Reactive.Linq
{
    public partial class Observable
    {
        public static IEnumerable<IObservable<T>> CombineSources<T>(IObservable<T> first, IObservable<T>[] seconds)
        {
            yield return first;
            for (var i = 0; i < seconds.Length; i++)
            {
                yield return seconds[i];
            }
        }

        public static IObservable<TSource> Concat<TSource>(params IObservable<TSource>[] sources)
        {
            if (sources == null) throw new ArgumentNullException("sources");

            return new ConcatObservable<TSource>(sources);
        }

        public static IObservable<TSource> Merge<TSource>(params IObservable<TSource>[] sources)
        {
            return Merge(Scheduler.DefaultSchedulers.ConstantTimeOperations, sources);
        }

        public static IObservable<T> Merge<T>(IObservable<IObservable<T>> sources, int maxConcurrent)
        {
            return new MergeObservable<T>(sources, maxConcurrent, false);
        }

        public static IObservable<TSource> Merge<TSource>(IEnumerable<IObservable<TSource>> sources, int maxConcurrent)
        {
            return sources.Merge(maxConcurrent, Scheduler.DefaultSchedulers.ConstantTimeOperations);
        }

        public static IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
        {
            return new MergeObservable<TSource>(sources.ToObservable(scheduler), scheduler == Scheduler.CurrentThread);
        }

        public static IObservable<IList<T>> Zip<T>(params IObservable<T>[] sources)
        {
            return new ZipObservable<T>(sources);
        }

        public static IObservable<IList<T>> Zip<T>(IEnumerable<IObservable<T>> sources)
        {
            return Zip(sources.ToArray());
        }

        public static IObservable<TR> Zip<T1, T2, T3, TR>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, ZipFunc<T1, T2, T3, TR> resultSelector)
        {
            return new ZipObservable<T1, T2, T3, TR>(source1, source2, source3, resultSelector);
        }

        public static IObservable<TR> Zip<T1, T2, T3, T4, TR>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, ZipFunc<T1, T2, T3, T4, TR> resultSelector)
        {
            return new ZipObservable<T1, T2, T3, T4, TR>(source1, source2, source3, source4, resultSelector);
        }

        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, TR>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, ZipFunc<T1, T2, T3, T4, T5, TR> resultSelector)
        {
            return new ZipObservable<T1, T2, T3, T4, T5, TR>(source1, source2, source3, source4, source5, resultSelector);
        }

        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, T6, TR>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, ZipFunc<T1, T2, T3, T4, T5, T6, TR> resultSelector)
        {
            return new ZipObservable<T1, T2, T3, T4, T5, T6, TR>(source1, source2, source3, source4, source5, source6, resultSelector);
        }

        public static IObservable<TR> Zip<T1, T2, T3, T4, T5, T6, T7, TR>(IObservable<T1> source1, IObservable<T2> source2, IObservable<T3> source3, IObservable<T4> source4, IObservable<T5> source5, IObservable<T6> source6, IObservable<T7> source7, ZipFunc<T1, T2, T3, T4, T5, T6, T7, TR> resultSelector)
        {
            return new ZipObservable<T1, T2, T3, T4, T5, T6, T7, TR>(source1, source2, source3, source4, source5, source6, source7, resultSelector);
        }

        public static IObservable<IList<TSource>> CombineLatest<TSource>(params IObservable<TSource>[] sources)
        {
            return new CombineLatestObservable<TSource>(sources);
        }
        
        public static IObservable<IList<TSource>> ZipLatest<TSource>(params IObservable<TSource>[] sources)
        {
            return new ZipLatestObservable<TSource>(sources);
        }

        /// <summary>
        /// <para>Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// <para>If sequence is empty, return T[0] array.</para>
        /// </summary>
        public static IObservable<T[]> WhenAll<T>(params IObservable<T>[] sources)
        {
            if (sources.Length == 0) return Return(new T[0]);

            return new WhenAllObservable<T>(sources);
        }

        /// <summary>
        /// <para>Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// <para>If sequence is empty, return T[0] array.</para>
        /// </summary>
        public static IObservable<T[]> WhenAll<T>(IEnumerable<IObservable<T>> sources)
        {
            if (sources is IObservable<T>[] array) return WhenAll(array);

            return new WhenAllObservable<T>(sources);
        }

        /// <summary>
        /// <para>Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// </summary>
        public static IObservable<Unit> WhenAll(IEnumerable<IObservable<Unit>> sources)
        {
            if (sources is IObservable<Unit>[] array) return WhenAll(array);
            return new WhenAllObservable(sources);
        }

        public static IObservable<T> StartWith<T>(IObservable<T> source, T value)
        {
            return new StartWithObservable<T>(source, value);
        }

        /// <summary>
        /// <para>Specialized for single async operations like Task.WhenAll, Zip.Take(1).</para>
        /// </summary>
        public static IObservable<Unit> WhenAll(params IObservable<Unit>[] sources)
        {
            if (sources.Length == 0) return ReturnUnit();

            return new WhenAllObservable(sources);
        }
    }
}