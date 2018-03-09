using System.Collections.Generic;
using System.Reactive.Operators;
using System.Reactive.Schedulers;

namespace System.Reactive.Linq
{
    public static partial class Observable
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

        public static IObservable<TSource> Merge<TSource>(IScheduler scheduler, params IObservable<TSource>[] sources)
        {
            return new MergeObservable<TSource>(sources.ToObservable(scheduler), scheduler == Scheduler.CurrentThread);
        }

        public static IObservable<IList<T>> Zip<T>(params IObservable<T>[] sources)
        {
            return new ZipObservable<T>(sources);
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
        /// </summary>
        public static IObservable<Unit> WhenAll(params IObservable<Unit>[] sources)
        {
            if (sources.Length == 0) return ReturnUnit();

            return new WhenAllObservable(sources);
        }

        
    }
}