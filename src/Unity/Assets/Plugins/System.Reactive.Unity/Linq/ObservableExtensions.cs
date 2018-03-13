using System.Collections;
using System.Collections.Generic;
using System.Reactive.Extensions;
using System.Reactive.Unity.Operators;
using System.Reactive.Unity.Triggers;
using System.Threading;
using UnityEngine;

namespace System.Reactive.Unity.Linq
{
    public static class ObservableExtensions
    {
        public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> source, Component target)
        {
            return source.TakeUntil(target.OnDestroyAsObservable());
        }

        public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> source, GameObject target)
        {
            return source.TakeUntil(target.OnDestroyAsObservable());
        }

        public static IObservable<T> TakeUntilDisable<T>(this IObservable<T> source, Component target)
        {
            return source.TakeUntil(target.OnDisableAsObservable());
        }

        public static IObservable<T> TakeUntilDisable<T>(this IObservable<T> source, GameObject target)
        {
            return source.TakeUntil(target.OnDisableAsObservable());
        }

        static IObservable<T> RepeatUntilCore<T>(this IEnumerable<IObservable<T>> sources, IObservable<Unit> trigger, GameObject lifeTimeChecker)
        {
            return new RepeatUntilObservable<T>(sources, trigger, lifeTimeChecker);
        }

        public static IObservable<T> RepeatUntilDestroy<T>(this IObservable<T> source, GameObject target)
        {
            return RepeatUntilCore(Reactive.Linq.Observable.RepeatInfinite(source), target.OnDestroyAsObservable(), target);
        }

        public static IObservable<T> RepeatUntilDestroy<T>(this IObservable<T> source, Component target)
        {
            return RepeatUntilCore(Reactive.Linq.Observable.RepeatInfinite(source), target.OnDestroyAsObservable(), (target != null) ? target.gameObject : null);
        }

        public static IObservable<T> RepeatUntilDisable<T>(this IObservable<T> source, GameObject target)
        {
            return RepeatUntilCore(Reactive.Linq.Observable.RepeatInfinite(source), target.OnDisableAsObservable(), target);
        }

        public static IObservable<T> RepeatUntilDisable<T>(this IObservable<T> source, Component target)
        {
            return RepeatUntilCore(Reactive.Linq.Observable.RepeatInfinite(source), target.OnDisableAsObservable(), (target != null) ? target.gameObject : null);
        }

        public static IObservable<Unit> ToObservable(this IEnumerator coroutine, bool publishEveryYield = false) =>
            Observable.ToObservable(coroutine, publishEveryYield);

        public static IObservable<T> DelayFrame<T>(this IObservable<T> source, int frameCount,
            FrameCountType frameCountType = FrameCountType.Update) =>
            Observable.DelayFrame(source, frameCount, frameCountType);

        public static IObservable<T> Sample<T, T2>(this IObservable<T> source, IObservable<T2> sampler) =>
            Observable.Sample(source, sampler);

        public static IObservable<T> SampleFrame<T>(this IObservable<T> source, int frameCount,
            FrameCountType frameCountType = FrameCountType.Update) =>
            Observable.SampleFrame(source, frameCount, frameCountType);

        public static IObservable<TSource> ThrottleFrame<TSource>(this IObservable<TSource> source, int frameCount,
            FrameCountType frameCountType = FrameCountType.Update) =>
            Observable.ThrottleFrame(source, frameCount, frameCountType);

        public static IObservable<TSource> ThrottleFirstFrame<TSource>(this IObservable<TSource> source, int frameCount,
            FrameCountType frameCountType = FrameCountType.Update) =>
            Observable.ThrottleFirstFrame(source, frameCount, frameCountType);

        public static IObservable<T> TimeoutFrame<T>(this IObservable<T> source, int frameCount,
            FrameCountType frameCountType = FrameCountType.Update) =>
            Observable.TimeoutFrame(source, frameCount, frameCountType);

        public static IObservable<T> DelayFrameSubscription<T>(this IObservable<T> source, int frameCount,
            FrameCountType frameCountType = FrameCountType.Update) =>
            Observable.DelayFrameSubscription(source, frameCount, frameCountType);

        /// <summary>
        /// Convert to yieldable IEnumerator. e.g. yield return source.ToYieldInstruction();.
        /// If needs last result, you can take ObservableYieldInstruction.HasResult/Result property.
        /// This overload throws exception if received OnError events(same as coroutine).
        /// </summary>
        public static ObservableYieldInstruction<T> ToYieldInstruction<T>(this IObservable<T> source) =>
            Observable.ToYieldInstruction(source);

        /// <summary>
        /// Convert to yieldable IEnumerator. e.g. yield return source.ToYieldInstruction();.
        /// If needs last result, you can take ObservableYieldInstruction.HasResult/Result property.
        /// This overload throws exception if received OnError events(same as coroutine).
        /// </summary>
        public static ObservableYieldInstruction<T> ToYieldInstruction<T>(this IObservable<T> source,
            CancellationToken cancel) => Observable.ToYieldInstruction(source, cancel);

        /// <summary>
        /// Convert to yieldable IEnumerator. e.g. yield return source.ToYieldInstruction();.
        /// If needs last result, you can take ObservableYieldInstruction.HasResult/Result property.
        /// If throwOnError = false, you can take ObservableYieldInstruction.HasError/Error property.
        /// </summary>
        public static ObservableYieldInstruction<T>
            ToYieldInstruction<T>(this IObservable<T> source, bool throwOnError) =>
            Observable.ToYieldInstruction(source, throwOnError);

        /// <summary>
        /// Convert to yieldable IEnumerator. e.g. yield return source.ToYieldInstruction();.
        /// If needs last result, you can take ObservableYieldInstruction.HasResult/Result property.
        /// If throwOnError = false, you can take ObservableYieldInstruction.HasError/Error property.
        /// </summary>
        public static ObservableYieldInstruction<T> ToYieldInstruction<T>(this IObservable<T> source, bool throwOnError,
            CancellationToken cancel) => Observable.ToYieldInstruction(source, throwOnError, cancel);

        /// <summary>Convert to awaitable IEnumerator.</summary>
        public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source,
            CancellationToken cancel = default(CancellationToken)) => Observable.ToAwaitableEnumerator(source, cancel);

        /// <summary>Convert to awaitable IEnumerator.</summary>
        public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<T> onResult,
            CancellationToken cancel = default(CancellationToken)) =>
            Observable.ToAwaitableEnumerator(source, onResult, cancel);

        /// <summary>Convert to awaitable IEnumerator.</summary>
        public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<Exception> onError,
            CancellationToken cancel = default(CancellationToken)) =>
            Observable.ToAwaitableEnumerator(source, onError, cancel);

        /// <summary>Convert to awaitable IEnumerator.</summary>
        public static IEnumerator ToAwaitableEnumerator<T>(this IObservable<T> source, Action<T> onResult,
            Action<Exception> onError, CancellationToken cancel = default(CancellationToken)) =>
            Observable.ToAwaitableEnumerator(source, onResult, onError, cancel);

        /// <summary>AutoStart observable as coroutine.</summary>
        public static Coroutine StartAsCoroutine<T>(this IObservable<T> source,
            CancellationToken cancel = default(CancellationToken)) => Observable.StartAsCoroutine(source, cancel);

        /// <summary>AutoStart observable as coroutine.</summary>
        public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<T> onResult,
            CancellationToken cancel = default(CancellationToken)) =>
            Observable.StartAsCoroutine(source, onResult, cancel);

        /// <summary>AutoStart observable as coroutine.</summary>
        public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<Exception> onError,
            CancellationToken cancel = default(CancellationToken)) =>
            Observable.StartAsCoroutine(source, onError, cancel);

        /// <summary>AutoStart observable as coroutine.</summary>
        public static Coroutine StartAsCoroutine<T>(this IObservable<T> source, Action<T> onResult,
            Action<Exception> onError, CancellationToken cancel = default(CancellationToken)) =>
            Observable.StartAsCoroutine(source, onResult, onError, cancel);

        public static IObservable<T> ObserveOnMainThread<T>(this IObservable<T> source) =>
            Observable.ObserveOnMainThread(source);

        public static IObservable<T> ObserveOnMainThread<T>(this IObservable<T> source,
            MainThreadDispatchType dispatchType) => Observable.ObserveOnMainThread(source, dispatchType);

        public static IObservable<T> SubscribeOnMainThread<T>(this IObservable<T> source) =>
            Observable.SubscribeOnMainThread(source);

        public static IObservable<FrameInterval<T>> FrameInterval<T>(this IObservable<T> source) =>
            Observable.FrameInterval(source);

        public static IObservable<TimeInterval<T>> FrameTimeInterval<T>(this IObservable<T> source,
            bool ignoreTimeScale = false) => Observable.FrameTimeInterval(source, ignoreTimeScale);

        /// <summary>
        /// Buffer elements in during target frame counts. Default raise same frame of end(frameCount = 0, frameCountType = EndOfFrame).
        /// </summary>
        public static IObservable<IList<T>> BatchFrame<T>(this IObservable<T> source) => Observable.BatchFrame(source);

        /// <summary>
        /// Buffer elements in during target frame counts.
        /// </summary>
        public static IObservable<IList<T>> BatchFrame<T>(this IObservable<T> source, int frameCount,
            FrameCountType frameCountType) => Observable.BatchFrame(source, frameCount, frameCountType);

        /// <summary>
        /// Wait command in during target frame counts. Default raise same frame of end(frameCount = 0, frameCountType = EndOfFrame).
        /// </summary>
        public static IObservable<Unit> BatchFrame(this IObservable<Unit> source) => Observable.BatchFrame(source);

        /// <summary>
        /// Wait command in during target frame counts.
        /// </summary>
        public static IObservable<Unit> BatchFrame(this IObservable<Unit> source, int frameCount,
            FrameCountType frameCountType) => Observable.BatchFrame(source, frameCount, frameCountType);
    }
}