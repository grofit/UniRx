using System.Reactive.Disposables;
using System.Reactive.Extensions;
using System.Reactive.Unity.Linq;
using System.Reactive.Operators;

namespace System.Reactive.Unity.Operators
{
    internal class DelayFrameSubscriptionObservable<T> : OperatorObservableBase<T>
    {
        readonly IObservable<T> source;
        readonly int frameCount;
        readonly FrameCountType frameCountType;

        public DelayFrameSubscriptionObservable(IObservable<T> source, int frameCount, FrameCountType frameCountType)
            : base(source.IsRequiredSubscribeOnCurrentThread())
        {
            this.source = source;
            this.frameCount = frameCount;
            this.frameCountType = frameCountType;
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer, IDisposable cancel)
        {
            var d = new MultipleAssignmentDisposable();
            d.Disposable = Observable.TimerFrame(frameCount, frameCountType)
                .SubscribeWithState3(observer, d, source, (_, o, disp, s) =>
                {
                    disp.Disposable = s.Subscribe(o);
                });

            return d;
        }
    }
}