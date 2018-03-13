using System.Collections;
using System.Reactive.Operators;
using System.Reactive.Unity.Schedulers;
using System.Threading;
using UniRx;

namespace System.Reactive.Unity.Operators
{
    internal class FromCoroutineObservable<T> : OperatorObservableBase<T>
    {
        readonly Func<IObserver<T>, CancellationToken, IEnumerator> coroutine;

        public FromCoroutineObservable(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine)
            : base(false)
        {
            this.coroutine = coroutine;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            var fromCoroutineObserver = new FromCoroutine(observer, cancel);
            
            var moreCancel = new CancellationDisposable();
            var token = moreCancel.Token;
            
            MainThreadDispatcher.SendStartCoroutine(coroutine(fromCoroutineObserver, token));

            return moreCancel;
        }

        class FromCoroutine : OperatorObserverBase<T, T>
        {
            public FromCoroutine(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
            }

            public override void OnNext(T value)
            {
                try
                {
                    base.observer.OnNext(value);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public override void OnError(Exception error)
            {
                try { observer.OnError(error); }
                finally { Dispose(); }
            }

            public override void OnCompleted()
            {
                try { observer.OnCompleted(); }
                finally { Dispose(); }
            }
        }
    }

    internal class FromMicroCoroutineObservable<T> : OperatorObservableBase<T>
    {
        readonly Func<IObserver<T>, CancellationToken, IEnumerator> coroutine;
        readonly FrameCountType frameCountType;

        public FromMicroCoroutineObservable(Func<IObserver<T>, CancellationToken, IEnumerator> coroutine, FrameCountType frameCountType)
            : base(false)
        {
            this.coroutine = coroutine;
            this.frameCountType = frameCountType;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            var microCoroutineObserver = new FromMicroCoroutine(observer, cancel);
            var moreCancel = new CancellationDisposable();
            var token = moreCancel.Token;


            switch (frameCountType)
            {
                case FrameCountType.Update:
                    MainThreadDispatcher.StartUpdateMicroCoroutine(coroutine(microCoroutineObserver, token));
                    break;
                case FrameCountType.FixedUpdate:
                    MainThreadDispatcher.StartFixedUpdateMicroCoroutine(coroutine(microCoroutineObserver, token));
                    break;
                case FrameCountType.EndOfFrame:
                    MainThreadDispatcher.StartEndOfFrameMicroCoroutine(coroutine(microCoroutineObserver, token));
                    break;
                default:
                    throw new ArgumentException("Invalid FrameCountType:" + frameCountType);
            }

            return moreCancel;
        }

        class FromMicroCoroutine : OperatorObserverBase<T, T>
        {
            public FromMicroCoroutine(IObserver<T> observer, IDisposable cancel) : base(observer, cancel)
            {
            }

            public override void OnNext(T value)
            {
                try
                {
                    base.observer.OnNext(value);
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public override void OnError(Exception error)
            {
                try { observer.OnError(error); }
                finally { Dispose(); }
            }

            public override void OnCompleted()
            {
                try { observer.OnCompleted(); }
                finally { Dispose(); }
            }
        }
    }
}