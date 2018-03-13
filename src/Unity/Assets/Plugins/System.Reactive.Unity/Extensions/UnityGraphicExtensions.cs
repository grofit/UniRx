using System.Reactive.Disposables;
using System.Reactive.Unity.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

namespace System.Reactive.Unity.Extensions
{
    public static class UnityGraphicExtensions
    {
        public static IObservable<Unit> DirtyLayoutCallbackAsObservable(this Graphic graphic)
        {
            return Observable.Create<Unit>(observer =>
            {
                UnityAction registerHandler = () => observer.OnNext(Unit.Default);
                graphic.RegisterDirtyLayoutCallback(registerHandler);
                return Disposable.Create(() => graphic.UnregisterDirtyLayoutCallback(registerHandler));
            });
        }

        public static IObservable<Unit> DirtyMaterialCallbackAsObservable(this Graphic graphic)
        {
            return Observable.Create<Unit>(observer =>
            {
                UnityAction registerHandler = () => observer.OnNext(Unit.Default);
                graphic.RegisterDirtyMaterialCallback(registerHandler);
                return Disposable.Create(() => graphic.UnregisterDirtyMaterialCallback(registerHandler));
            });
        }

        public static IObservable<Unit> DirtyVerticesCallbackAsObservable(this Graphic graphic)
        {
            return Observable.Create<Unit>(observer =>
            {
                UnityAction registerHandler = () => observer.OnNext(Unit.Default);
                graphic.RegisterDirtyVerticesCallback(registerHandler);
                return Disposable.Create(() => graphic.UnregisterDirtyVerticesCallback(registerHandler));
            });
        }
    }
}