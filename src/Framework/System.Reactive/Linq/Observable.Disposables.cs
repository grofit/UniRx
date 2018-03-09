using System.Reactive.Disposables;

namespace System.Reactive.Linq
{
    public partial class Observable
    {
        static IObservable<T> AddRef<T>(IObservable<T> xs, RefCountDisposable r)
        {
            return Create<T>((observer) => new CompositeDisposable(new []
            {
                r.GetDisposable(),
                xs.Subscribe(observer)
            }));
        }
    }
}
