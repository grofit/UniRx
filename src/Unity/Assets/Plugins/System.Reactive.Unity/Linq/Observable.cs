using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Unity.Operators;
using System.Threading;
using UnityEngine;
using System.Reactive.Operators;
using System.Reactive.Unity.Schedulers;

namespace System.Reactive.Unity.Linq
{
    public partial class Observable : System.Reactive.Linq.Observable
    {
        readonly static HashSet<Type> YieldInstructionTypes = new HashSet<Type>
        {
            typeof(WWW),
            typeof(WaitForEndOfFrame),
            typeof(WaitForFixedUpdate),
            typeof(WaitForSeconds),
            typeof(AsyncOperation),
            typeof(Coroutine)
        };
        
        class EveryAfterUpdateInvoker : IEnumerator
        {
            long count = -1;
            readonly IObserver<long> observer;
            readonly CancellationToken cancellationToken;

            public EveryAfterUpdateInvoker(IObserver<long> observer, CancellationToken cancellationToken)
            {
                this.observer = observer;
                this.cancellationToken = cancellationToken;
            }

            public bool MoveNext()
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    if (count != -1) // ignore first/immediate invoke
                    {
                        observer.OnNext(count++);
                    }
                    else
                    {
                        count++;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public object Current
            {
                get
                {
                    return null;
                }
            }

            public void Reset()
            {
                throw new NotSupportedException();
            }
        }
        

        /// <summary>From has no callback coroutine to IObservable. If publishEveryYield = true then publish OnNext every yield return else return once on enumeration completed.</summary>
        public static IObservable<Unit> FromCoroutine(Func<IEnumerator> coroutine, bool publishEveryYield = false)
        {
            return FromCoroutine<Unit>((observer, cancellationToken) => WrapEnumerator(coroutine(), observer, cancellationToken, publishEveryYield));
        }

        /// <summary>From has no callback coroutine to IObservable. If publishEveryYield = true then publish OnNext every yield return else return once on enumeration completed.</summary>
        public static IObservable<Unit> FromCoroutine(Func<CancellationToken, IEnumerator> coroutine, bool publishEveryYield = false)
        {
            return FromCoroutine<Unit>((observer, cancellationToken) => WrapEnumerator(coroutine(cancellationToken), observer, cancellationToken, publishEveryYield));
        }

        /// <summary>
        /// MicroCoroutine is lightweight, fast coroutine dispatcher.
        /// IEnumerator supports only yield return null.
        /// If publishEveryYield = true then publish OnNext every yield return else return once on enumeration completed.
        /// </summary>
        public static IObservable<Unit> FromMicroCoroutine(Func<IEnumerator> coroutine, bool publishEveryYield = false, FrameCountType frameCountType = FrameCountType.Update)
        {
            return FromMicroCoroutine<Unit>((observer, cancellationToken) => WrapEnumerator(coroutine(), observer, cancellationToken, publishEveryYield), frameCountType);
        }

        /// <summary>
        /// MicroCoroutine is lightweight, fast coroutine dispatcher.
        /// IEnumerator supports only yield return null.
        /// If publishEveryYield = true then publish OnNext every yield return else return once on enumeration completed.
        /// </summary>
        public static IObservable<Unit> FromMicroCoroutine(Func<CancellationToken, IEnumerator> coroutine, bool publishEveryYield = false, FrameCountType frameCountType = FrameCountType.Update)
        {
            return FromMicroCoroutine<Unit>((observer, cancellationToken) => WrapEnumerator(coroutine(cancellationToken), observer, cancellationToken, publishEveryYield), frameCountType);
        }

        static IEnumerator WrapEnumerator(IEnumerator enumerator, IObserver<Unit> observer, CancellationToken cancellationToken, bool publishEveryYield)
        {
            var hasNext = default(bool);
            var raisedError = false;
            do
            {
                try
                {
                    hasNext = enumerator.MoveNext();
                }
                catch (Exception ex)
                {
                    try
                    {
                        raisedError = true;
                        observer.OnError(ex);
                    }
                    finally
                    {
                        var d = enumerator as IDisposable;
                        if (d != null)
                        {
                            d.Dispose();
                        }
                    }
                    yield break;
                }
                if (hasNext && publishEveryYield)
                {
                    try
                    {
                        observer.OnNext(Unit.Default);
                    }
                    catch
                    {
                        var d = enumerator as IDisposable;
                        if (d != null)
                        {
                            d.Dispose();
                        }
                        throw;
                    }
                }
                if (hasNext)
                {
                    var current = enumerator.Current;
                    var customHandler = current as ICustomYieldInstructionErrorHandler;
                    if (customHandler != null && customHandler.IsReThrowOnError)
                    {
                        // If throws exception in Custom YieldInsrtuction, can't handle parent coroutine.
                        // It is C# limitation.
                        // so store error info and retrieve from parent.
                        customHandler.ForceDisableRethrowOnError();
                        yield return current;
                        customHandler.ForceEnableRethrowOnError();

                        if (customHandler.HasError)
                        {
                            try
                            {
                                raisedError = true;
                                observer.OnError(customHandler.Error);
                            }
                            finally
                            {
                                var d = enumerator as IDisposable;
                                if (d != null)
                                {
                                    d.Dispose();
                                }
                            }
                            yield break;
                        }
                    }
                    else
                    {
                        yield return enumerator.Current; // yield inner YieldInstruction
                    }
                }
            } while (hasNext && !cancellationToken.IsCancellationRequested);

            try
            {
                if (!raisedError && !cancellationToken.IsCancellationRequested)
                {
                    observer.OnNext(Unit.Default); // last one
                    observer.OnCompleted();
                }
            }
            finally
            {
                var d = enumerator as IDisposable;
                if (d != null)
                {
                    d.Dispose();
                }
            }
        }

        /// <summary>Convert coroutine to typed IObservable. If nullAsNextUpdate = true then yield return null when Enumerator.Current and no null publish observer.OnNext.</summary>
        public static IObservable<T> FromCoroutineValue<T>(Func<IEnumerator> coroutine, bool nullAsNextUpdate = true)
        {
            return FromCoroutine<T>((observer, cancellationToken) => WrapEnumeratorYieldValue<T>(coroutine(), observer, cancellationToken, nullAsNextUpdate));
        }

        /// <summary>Convert coroutine to typed IObservable. If nullAsNextUpdate = true then yield return null when Enumerator.Current and no null publish observer.OnNext.</summary>
        public static IObservable<T> FromCoroutineValue<T>(Func<CancellationToken, IEnumerator> coroutine, bool nullAsNextUpdate = true)
        {
            return FromCoroutine<T>((observer, cancellationToken) => WrapEnumeratorYieldValue<T>(coroutine(cancellationToken), observer, cancellationToken, nullAsNextUpdate));
        }

        static IEnumerator WrapEnumeratorYieldValue<T>(IEnumerator enumerator, IObserver<T> observer, CancellationToken cancellationToken, bool nullAsNextUpdate)
        {
            var hasNext = default(bool);
            var current = default(object);
            var raisedError = false;
            do
            {
                try
                {
                    hasNext = enumerator.MoveNext();
                    if (hasNext) current = enumerator.Current;
                }
                catch (Exception ex)
                {
                    try
                    {
                        raisedError = true;
                        observer.OnError(ex);
                    }
                    finally
                    {
                        var d = enumerator as IDisposable;
                        if (d != null)
                        {
                            d.Dispose();
                        }
                    }
                    yield break;
                }

                if (hasNext)
                {
                    if (current != null && YieldInstructionTypes.Contains(current.GetType()))
                    {
                        yield return current;
                    }
                    else if (current is IEnumerator)
                    {
                        var customHandler = current as ICustomYieldInstructionErrorHandler;
                        if (customHandler != null && customHandler.IsReThrowOnError)
                        {
                            // If throws exception in Custom YieldInsrtuction, can't handle parent coroutine.
                            // It is C# limitation.
                            // so store error info and retrieve from parent.
                            customHandler.ForceDisableRethrowOnError();
                            yield return current;
                            customHandler.ForceEnableRethrowOnError();

                            if (customHandler.HasError)
                            {
                                try
                                {
                                    raisedError = true;
                                    observer.OnError(customHandler.Error);
                                }
                                finally
                                {
                                    var d = enumerator as IDisposable;
                                    if (d != null)
                                    {
                                        d.Dispose();
                                    }
                                }
                                yield break;
                            }
                        }
                        else
                        {
                            yield return current;
                        }
                    }

                    else if (current == null && nullAsNextUpdate)
                    {
                        yield return null;
                    }
                    else
                    {
                        try
                        {
                            observer.OnNext((T)current);
                        }
                        catch
                        {
                            var d = enumerator as IDisposable;
                            if (d != null)
                            {
                                d.Dispose();
                            }
                            throw;
                        }
                    }
                }
            } while (hasNext && !cancellationToken.IsCancellationRequested);

            try
            {
                if (!raisedError && !cancellationToken.IsCancellationRequested)
                {
                    observer.OnCompleted();
                }
            }
            finally
            {
                var d = enumerator as IDisposable;
                if (d != null)
                {
                    d.Dispose();
                }
            }
        }

        public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, IEnumerator> coroutine)
        {
            return FromCoroutine<T>((observer, cancellationToken) => WrapToCancellableEnumerator(coroutine(observer), cancellationToken));
        }

        /// <summary>
        /// MicroCoroutine is lightweight, fast coroutine dispatcher.
        /// IEnumerator supports only yield return null.
        /// </summary>
        public static IObservable<T> FromMicroCoroutine<T>(Func<IObserver<T>, IEnumerator> coroutine, FrameCountType frameCountType = FrameCountType.Update)
        {
            return FromMicroCoroutine<T>((observer, cancellationToken) => WrapToCancellableEnumerator(coroutine(observer), cancellationToken), frameCountType);
        }

        static IEnumerator WrapToCancellableEnumerator(IEnumerator enumerator, CancellationToken cancellationToken)
        {
            var hasNext = default(bool);
            do
            {
                try
                {
                    hasNext = enumerator.MoveNext();
                }
                catch
                {
                    var d = enumerator as IDisposable;
                    if (d != null)
                    {
                        d.Dispose();
                    }
                    yield break;
                }

                yield return enumerator.Current; // yield inner YieldInstruction
            } while (hasNext && !cancellationToken.IsCancellationRequested);

            {
                var d = enumerator as IDisposable;
                if (d != null)
                {
                    d.Dispose();
                }
            }
        }

        public static IObservable<T> FromCoroutine<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine)
        {
            return new FromCoroutineObservable<T>(coroutine);
        }

        /// <summary>
        /// MicroCoroutine is lightweight, fast coroutine dispatcher.
        /// IEnumerator supports only yield return null.
        /// </summary>
        public static IObservable<T> FromMicroCoroutine<T>(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine, FrameCountType frameCountType = FrameCountType.Update)
        {
            return new FromMicroCoroutineObservable<T>(coroutine, frameCountType);
        }
        
        // variation of FromCoroutine

        /// <summary>
        /// EveryUpdate calls coroutine's yield return null timing. It is after all Update and before LateUpdate.
        /// </summary>
        public static IObservable<long> EveryUpdate()
        {
            return FromMicroCoroutine<long>((observer, cancellationToken) => EveryCycleCore(observer, cancellationToken), FrameCountType.Update);
        }

        public static IObservable<long> EveryFixedUpdate()
        {
            return FromMicroCoroutine<long>((observer, cancellationToken) => EveryCycleCore(observer, cancellationToken), FrameCountType.FixedUpdate);
        }

        public static IObservable<long> EveryEndOfFrame()
        {
            return FromMicroCoroutine<long>((observer, cancellationToken) => EveryCycleCore(observer, cancellationToken), FrameCountType.EndOfFrame);
        }

        static IEnumerator EveryCycleCore(IObserver<long> observer, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) yield break;
            var count = 0L;
            while (true)
            {
                yield return null;
                if (cancellationToken.IsCancellationRequested) yield break;

                observer.OnNext(count++);
            }
        }

        /// <summary>
        /// EveryGameObjectUpdate calls from MainThreadDispatcher's Update.
        /// </summary>
        public static IObservable<long> EveryGameObjectUpdate()
        {
            return MainThreadDispatcher.UpdateAsObservable().Scan(-1L, (x, y) => x + 1);
        }

        /// <summary>
        /// EveryLateUpdate calls from MainThreadDispatcher's OnLateUpdate.
        /// </summary>
        public static IObservable<long> EveryLateUpdate()
        {
            return MainThreadDispatcher.LateUpdateAsObservable().Scan(-1L, (x, y) => x + 1);
        }
        
        /// <summary>
        /// [Obsolete]Same as EveryUpdate.
        /// </summary>
        [Obsolete]
        public static IObservable<long> EveryAfterUpdate()
        {
            return FromCoroutine<long>((observer, cancellationToken) => new EveryAfterUpdateInvoker(observer, cancellationToken));
        }

        #region Observable.Time Frame Extensions

        // Interval, Timer, Delay, Sample, Throttle, Timeout

        public static IObservable<Unit> NextFrame(FrameCountType frameCountType = FrameCountType.Update)
        {
            return FromMicroCoroutine<Unit>(NextFrameCore, frameCountType);
        }

        static IEnumerator NextFrameCore(IObserver<Unit> observer, CancellationToken cancellation)
        {
            yield return null;

            if (!cancellation.IsCancellationRequested)
            {
                observer.OnNext(Unit.Default);
                observer.OnCompleted();
            }
        }

        public static IObservable<long> IntervalFrame(int intervalFrameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            return TimerFrame(intervalFrameCount, intervalFrameCount, frameCountType);
        }

        public static IObservable<long> TimerFrame(int dueTimeFrameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            return FromMicroCoroutine<long>((observer, cancellation) => TimerFrameCore(observer, dueTimeFrameCount, cancellation), frameCountType);
        }

        public static IObservable<long> TimerFrame(int dueTimeFrameCount, int periodFrameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            return FromMicroCoroutine<long>((observer, cancellation) => TimerFrameCore(observer, dueTimeFrameCount, periodFrameCount, cancellation), frameCountType);
        }

        static IEnumerator TimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, CancellationToken cancel)
        {
            // normalize
            if (dueTimeFrameCount <= 0) dueTimeFrameCount = 0;

            var currentFrame = 0;

            // initial phase
            while (!cancel.IsCancellationRequested)
            {
                if (currentFrame++ == dueTimeFrameCount)
                {
                    observer.OnNext(0);
                    observer.OnCompleted();
                    break;
                }
                yield return null;
            }
        }

        static IEnumerator TimerFrameCore(IObserver<long> observer, int dueTimeFrameCount, int periodFrameCount, CancellationToken cancel)
        {
            // normalize
            if (dueTimeFrameCount <= 0) dueTimeFrameCount = 0;
            if (periodFrameCount <= 0) periodFrameCount = 1;

            var sendCount = 0L;
            var currentFrame = 0;

            // initial phase
            while (!cancel.IsCancellationRequested)
            {
                if (currentFrame++ == dueTimeFrameCount)
                {
                    observer.OnNext(sendCount++);
                    currentFrame = -1;
                    break;
                }
                yield return null;
            }

            // period phase
            while (!cancel.IsCancellationRequested)
            {
                if (++currentFrame == periodFrameCount)
                {
                    observer.OnNext(sendCount++);
                    currentFrame = 0;
                }
                yield return null;
            }
        }

        public static IObservable<T> DelayFrame<T>(IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            if (frameCount < 0) throw new ArgumentOutOfRangeException("frameCount");
            return new DelayFrameObservable<T>(source, frameCount, frameCountType);
        }

        public static IObservable<T> Sample<T, T2>(IObservable<T> source, IObservable<T2> sampler)
        {
            return new SampleObservable<T, T2>(source, sampler);
        }

        public static IObservable<T> SampleFrame<T>(IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            if (frameCount < 0) throw new ArgumentOutOfRangeException("frameCount");
            return new SampleFrameObservable<T>(source, frameCount, frameCountType);
        }

        public static IObservable<TSource> ThrottleFrame<TSource>(IObservable<TSource> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            if (frameCount < 0) throw new ArgumentOutOfRangeException("frameCount");
            return new ThrottleFrameObservable<TSource>(source, frameCount, frameCountType);
        }

        public static IObservable<TSource> ThrottleFirstFrame<TSource>(IObservable<TSource> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            if (frameCount < 0) throw new ArgumentOutOfRangeException("frameCount");
            return new ThrottleFirstFrameObservable<TSource>(source, frameCount, frameCountType);
        }

        public static IObservable<T> TimeoutFrame<T>(IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            if (frameCount < 0) throw new ArgumentOutOfRangeException("frameCount");
            return new TimeoutFrameObservable<T>(source, frameCount, frameCountType);
        }

        public static IObservable<T> DelayFrameSubscription<T>(IObservable<T> source, int frameCount, FrameCountType frameCountType = FrameCountType.Update)
        {
            if (frameCount < 0) throw new ArgumentOutOfRangeException("frameCount");
            return new DelayFrameSubscriptionObservable<T>(source, frameCount, frameCountType);
        }

        #endregion

        /// <summary>
        /// Convert to yieldable IEnumerator. e.g. yield return source.ToYieldInstruction();.
        /// If needs last result, you can take ObservableYieldInstruction.HasResult/Result property.
        /// This overload throws exception if received OnError events(same as coroutine).
        /// </summary>
        public static ObservableYieldInstruction<T> ToYieldInstruction<T>(IObservable<T> source)
        {
            return new ObservableYieldInstruction<T>(source, true, CancellationToken.None);
        }

        /// <summary>
        /// Convert to yieldable IEnumerator. e.g. yield return source.ToYieldInstruction();.
        /// If needs last result, you can take ObservableYieldInstruction.HasResult/Result property.
        /// This overload throws exception if received OnError events(same as coroutine).
        /// </summary>
        public static ObservableYieldInstruction<T> ToYieldInstruction<T>(IObservable<T> source, CancellationToken cancel)
        {
            return new ObservableYieldInstruction<T>(source, true, cancel);
        }

        /// <summary>
        /// Convert to yieldable IEnumerator. e.g. yield return source.ToYieldInstruction();.
        /// If needs last result, you can take ObservableYieldInstruction.HasResult/Result property.
        /// If throwOnError = false, you can take ObservableYieldInstruction.HasError/Error property.
        /// </summary>
        public static ObservableYieldInstruction<T> ToYieldInstruction<T>(IObservable<T> source, bool throwOnError)
        {
            return new ObservableYieldInstruction<T>(source, throwOnError, CancellationToken.None);
        }

        /// <summary>
        /// Convert to yieldable IEnumerator. e.g. yield return source.ToYieldInstruction();.
        /// If needs last result, you can take ObservableYieldInstruction.HasResult/Result property.
        /// If throwOnError = false, you can take ObservableYieldInstruction.HasError/Error property.
        /// </summary>
        public static ObservableYieldInstruction<T> ToYieldInstruction<T>(IObservable<T> source, bool throwOnError, CancellationToken cancel)
        {
            return new ObservableYieldInstruction<T>(source, throwOnError, cancel);
        }
        
        /// <summary>Convert to awaitable IEnumerator.</summary>
        public static IEnumerator ToAwaitableEnumerator<T>(IObservable<T> source, CancellationToken cancel = default(CancellationToken))
        {
            return ToAwaitableEnumerator<T>(source, Stubs<T>.Ignore, Stubs.Throw, cancel);
        }

        /// <summary>Convert to awaitable IEnumerator.</summary>
        public static IEnumerator ToAwaitableEnumerator<T>(IObservable<T> source, Action<T> onResult, CancellationToken cancel = default(CancellationToken))
        {
            return ToAwaitableEnumerator<T>(source, onResult, Stubs.Throw, cancel);
        }

        /// <summary>Convert to awaitable IEnumerator.</summary>
        public static IEnumerator ToAwaitableEnumerator<T>(IObservable<T> source, Action<Exception> onError, CancellationToken cancel = default(CancellationToken))
        {
            return ToAwaitableEnumerator<T>(source, Stubs<T>.Ignore, onError, cancel);
        }

        /// <summary>Convert to awaitable IEnumerator.</summary>
        public static IEnumerator ToAwaitableEnumerator<T>(IObservable<T> source, Action<T> onResult, Action<Exception> onError, CancellationToken cancel = default(CancellationToken))
        {
            var enumerator = new ObservableYieldInstruction<T>(source, false, cancel);
            var e = (IEnumerator<T>)enumerator;
            while (e.MoveNext() && !cancel.IsCancellationRequested)
            {
                yield return null;
            }

            if (cancel.IsCancellationRequested)
            {
                enumerator.Dispose();
                yield break;
            }

            if (enumerator.HasResult)
            {
                onResult(enumerator.Result);
            }
            else if (enumerator.HasError)
            {
                onError(enumerator.Error);
            }
        }

        /// <summary>AutoStart observable as coroutine.</summary>
        public static Coroutine StartAsCoroutine<T>(IObservable<T> source, CancellationToken cancel = default(CancellationToken))
        {
            return StartAsCoroutine<T>(source, Stubs<T>.Ignore, Stubs.Throw, cancel);
        }

        /// <summary>AutoStart observable as coroutine.</summary>
        public static Coroutine StartAsCoroutine<T>(IObservable<T> source, Action<T> onResult, CancellationToken cancel = default(CancellationToken))
        {
            return StartAsCoroutine<T>(source, onResult, Stubs.Throw, cancel);
        }

        /// <summary>AutoStart observable as coroutine.</summary>
        public static Coroutine StartAsCoroutine<T>(IObservable<T> source, Action<Exception> onError, CancellationToken cancel = default(CancellationToken))
        {
            return StartAsCoroutine<T>(source, Stubs<T>.Ignore, onError, cancel);
        }

        /// <summary>AutoStart observable as coroutine.</summary>
        public static Coroutine StartAsCoroutine<T>(IObservable<T> source, Action<T> onResult, Action<Exception> onError, CancellationToken cancel = default(CancellationToken))
        {
            return MainThreadDispatcher.StartCoroutine(source.ToAwaitableEnumerator(onResult, onError, cancel));
        }

        public static IObservable<T> ObserveOnMainThread<T>(IObservable<T> source)
        {
            return source.ObserveOn(Scheduler.MainThread);
        }

        public static IObservable<T> ObserveOnMainThread<T>(IObservable<T> source, MainThreadDispatchType dispatchType)
        {
            switch (dispatchType)
            {
                case MainThreadDispatchType.Update:
                    return source.ObserveOnMainThread(); // faster path

                // others, bit slower

                case MainThreadDispatchType.FixedUpdate:
                    return source.SelectMany(_ => EveryFixedUpdate().Take(1), (x, _) => x);
                case MainThreadDispatchType.EndOfFrame:
                    return source.SelectMany(_ => EveryEndOfFrame().Take(1), (x, _) => x);
                case MainThreadDispatchType.GameObjectUpdate:
                    return source.SelectMany(_ => MainThreadDispatcher.UpdateAsObservable().Take(1), (x, _) => x);
                case MainThreadDispatchType.LateUpdate:
                    return source.SelectMany(_ => MainThreadDispatcher.LateUpdateAsObservable().Take(1), (x, _) => x);

#pragma warning disable 612 // Type or member is obsolete
                case MainThreadDispatchType.AfterUpdate:
                    return source.SelectMany(_ => EveryAfterUpdate().Take(1), (x, _) => x);
#pragma warning restore 612 // Type or member is obsolete

                default:
                    throw new ArgumentException("type is invalid");
            }
        }

        public static IObservable<T> SubscribeOnMainThread<T>(IObservable<T> source)
        {
            return source.SubscribeOn(Scheduler.MainThread);
        }

        // I can't avoid Unity 5.3's uNET weaver bug, pending...

        //public static IObservable<T> SubscribeOnMainThread<T>(this IObservable<T> source, MainThreadDispatchType dispatchType)
        //{
        //    switch (dispatchType)
        //    {
        //        case MainThreadDispatchType.Update:
        //            return source.SubscribeOnMainThread(); // faster path

        //        // others, bit slower

        //        case MainThreadDispatchType.FixedUpdate:
        //            return new UniRx.Operators.SubscribeOnMainThreadObservable<T>(source, EveryFixedUpdate().Take(1));
        //        case MainThreadDispatchType.EndOfFrame:
        //            return new UniRx.Operators.SubscribeOnMainThreadObservable<T>(source, EveryEndOfFrame().Take(1));
        //        case MainThreadDispatchType.GameObjectUpdate:
        //            return new UniRx.Operators.SubscribeOnMainThreadObservable<T>(source, MainThreadDispatcher.UpdateAsObservable().Select(_ => 0L).Take(1));
        //        case MainThreadDispatchType.LateUpdate:
        //            return new UniRx.Operators.SubscribeOnMainThreadObservable<T>(source, MainThreadDispatcher.LateUpdateAsObservable().Select(_ => 0L).Take(1));
        //        case MainThreadDispatchType.AfterUpdate:
        //            return new UniRx.Operators.SubscribeOnMainThreadObservable<T>(source, EveryAfterUpdate().Take(1));
        //        default:
        //            throw new ArgumentException("type is invalid");
        //    }
        //}

        public static IObservable<bool> EveryApplicationPause()
        {
            return MainThreadDispatcher.OnApplicationPauseAsObservable().AsObservable();
        }

        public static IObservable<bool> EveryApplicationFocus()
        {
            return MainThreadDispatcher.OnApplicationFocusAsObservable().AsObservable();
        }

        /// <summary>publish OnNext(Unit) and OnCompleted() on application quit.</summary>
        public static IObservable<Unit> OnceApplicationQuit()
        {
            return MainThreadDispatcher.OnApplicationQuitAsObservable().Take(1);
        }

        public static IObservable<FrameInterval<T>> FrameInterval<T>(IObservable<T> source)
        {
            return new FrameIntervalObservable<T>(source);
        }

        public static IObservable<TimeInterval<T>> FrameTimeInterval<T>(IObservable<T> source, bool ignoreTimeScale = false)
        {
            return new FrameTimeIntervalObservable<T>(source, ignoreTimeScale);
        }

        /// <summary>
        /// Buffer elements in during target frame counts. Default raise same frame of end(frameCount = 0, frameCountType = EndOfFrame).
        /// </summary>
        public static IObservable<IList<T>> BatchFrame<T>(IObservable<T> source)
        {
            // if use default argument, comiler errors ambiguous(Unity's limitation)
            return BatchFrame<T>(source, 0, FrameCountType.EndOfFrame);
        }

        /// <summary>
        /// Buffer elements in during target frame counts.
        /// </summary>
        public static IObservable<IList<T>> BatchFrame<T>(IObservable<T> source, int frameCount, FrameCountType frameCountType)
        {
            if (frameCount < 0) throw new ArgumentException("frameCount must be >= 0, frameCount:" + frameCount);
            return new BatchFrameObservable<T>(source, frameCount, frameCountType);
        }

        /// <summary>
        /// Wait command in during target frame counts. Default raise same frame of end(frameCount = 0, frameCountType = EndOfFrame).
        /// </summary>
        public static IObservable<Unit> BatchFrame(IObservable<Unit> source)
        {
            return BatchFrame(source, 0, FrameCountType.EndOfFrame);
        }

        /// <summary>
        /// Wait command in during target frame counts.
        /// </summary>
        public static IObservable<Unit> BatchFrame(IObservable<Unit> source, int frameCount, FrameCountType frameCountType)
        {
            if (frameCount < 0) throw new ArgumentException("frameCount must be >= 0, frameCount:" + frameCount);
            return new BatchFrameObservable(source, frameCount, frameCountType);
        }
    }
}