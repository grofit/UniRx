#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)
#define SupportCustomYieldInstruction
#endif

using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Unity.Operators;
using System.Reactive.Unity.Triggers;
using System.Threading;
using UnityEngine;

namespace System.Reactive.Unity
{
    public enum FrameCountType
    {
        Update,
        FixedUpdate,
        EndOfFrame,
    }

    public enum MainThreadDispatchType
    {
        /// <summary>yield return null</summary>
        Update,
        FixedUpdate,
        EndOfFrame,
        GameObjectUpdate,
        LateUpdate,
#if SupportCustomYieldInstruction
        /// <summary>[Obsolete]Same as Update</summary>
        [Obsolete]
        AfterUpdate
#endif
    }

    public static class FrameCountTypeExtensions
    {
        public static YieldInstruction GetYieldInstruction(this FrameCountType frameCountType)
        {
            switch (frameCountType)
            {
                case FrameCountType.FixedUpdate:
                    return YieldInstructionCache.WaitForFixedUpdate;
                case FrameCountType.EndOfFrame:
                    return YieldInstructionCache.WaitForEndOfFrame;
                case FrameCountType.Update:
                default:
                    return null;
            }
        }
    }

    internal interface ICustomYieldInstructionErrorHandler
    {
        bool HasError { get; }
        Exception Error { get; }
        bool IsReThrowOnError { get; }
        void ForceDisableRethrowOnError();
        void ForceEnableRethrowOnError();
    }

    public class ObservableYieldInstruction<T> : IEnumerator<T>, ICustomYieldInstructionErrorHandler
    {
        readonly IDisposable subscription;
        readonly CancellationToken cancel;
        bool reThrowOnError;
        T current;
        T result;
        bool moveNext;
        bool hasResult;
        Exception error;

        public ObservableYieldInstruction(IObservable<T> source, bool reThrowOnError, CancellationToken cancel)
        {
            this.moveNext = true;
            this.reThrowOnError = reThrowOnError;
            this.cancel = cancel;
            try
            {
                this.subscription = source.Subscribe(new ToYieldInstruction(this));
            }
            catch
            {
                moveNext = false;
                throw;
            }
        }

        public bool HasError
        {
            get { return error != null; }
        }

        public bool HasResult
        {
            get { return hasResult; }
        }

        public bool IsCanceled
        {
            get
            {
                if (hasResult) return false;
                if (error != null) return false;
                return cancel.IsCancellationRequested;
            }
        }

        /// <summary>
        /// HasResult || IsCanceled || HasError
        /// </summary>
        public bool IsDone
        {
            get
            {
                return HasResult || HasError || (cancel.IsCancellationRequested);
            }
        }

        public T Result
        {
            get { return result; }
        }

        T IEnumerator<T>.Current
        {
            get
            {
                return current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return current;
            }
        }

        public Exception Error
        {
            get
            {
                return error;
            }
        }

        bool IEnumerator.MoveNext()
        {
            if (!moveNext)
            {
                if (reThrowOnError && HasError)
                {
                    throw Error;
                }

                return false;
            }

            if (cancel.IsCancellationRequested)
            {
                subscription.Dispose();
                return false;
            }

            return true;
        }

        bool ICustomYieldInstructionErrorHandler.IsReThrowOnError
        {
            get { return reThrowOnError; }
        }

        void ICustomYieldInstructionErrorHandler.ForceDisableRethrowOnError()
        {
            this.reThrowOnError = false;
        }

        void ICustomYieldInstructionErrorHandler.ForceEnableRethrowOnError()
        {
            this.reThrowOnError = true;
        }

        public void Dispose()
        {
            subscription.Dispose();
        }

        void IEnumerator.Reset()
        {
            throw new NotSupportedException();
        }

        class ToYieldInstruction : IObserver<T>
        {
            readonly ObservableYieldInstruction<T> parent;

            public ToYieldInstruction(ObservableYieldInstruction<T> parent)
            {
                this.parent = parent;
            }

            public void OnNext(T value)
            {
                parent.current = value;
            }

            public void OnError(Exception error)
            {
                parent.moveNext = false;
                parent.error = error;
            }

            public void OnCompleted()
            {
                parent.moveNext = false;
                parent.hasResult = true;
                parent.result = parent.current;
            }
        }
    }
}
