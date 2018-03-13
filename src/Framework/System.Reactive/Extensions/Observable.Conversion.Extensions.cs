using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Schedulers;

namespace System.Reactive.Extensions
{
    public static partial class ObservableExtensions
    {
        public static IObservable<T> AsObservable<T>(this IObservable<T> source) => 
            Observable.AsObservable(source);

        public static IObservable<T> ToObservable<T>(this IEnumerable<T> source) => 
            Observable.ToObservable(source);

        public static IObservable<T> ToObservable<T>(this IEnumerable<T> source, IScheduler scheduler) =>
            Observable.ToObservable(source, scheduler);

        public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source) =>
            Observable.Cast<TSource, TResult>(source);

        /// <summary>
        /// witness is for type inference.
        /// </summary>
        public static IObservable<TResult> Cast<TSource, TResult>(this IObservable<TSource> source, TResult witness) =>
            Observable.Cast<TSource, TResult>(source, witness);

        public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source) =>
            Observable.OfType<TSource, TResult>(source);

        /// <summary>
        /// witness is for type inference.
        /// </summary>
        public static IObservable<TResult> OfType<TSource, TResult>(this IObservable<TSource> source, TResult witness) => 
            Observable.OfType<TSource, TResult>(source, witness);

        /// <summary>
        /// Converting .Select(_ => Unit.Default) sequence.
        /// </summary>
        public static IObservable<Unit> AsUnitObservable<T>(this IObservable<T> source) =>
            Observable.AsUnitObservable(source);

        /// <summary>
        /// Same as LastOrDefault().AsUnitObservable().
        /// </summary>
        public static IObservable<Unit> AsSingleUnitObservable<T>(this IObservable<T> source) =>
            Observable.AsSingleUnitObservable(source);
    }
}