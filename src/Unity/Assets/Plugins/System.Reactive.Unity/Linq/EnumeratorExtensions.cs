using System.Collections;
using System.Reactive;
using System.Reactive.Unity;
using System.Reactive.Unity.Linq;
using System.Threading;

namespace Assets.Plugins.System.Reactive.Unity.Linq
{
    public static class EnumeratorExtensions
    {
        public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine)
        {
            return Observable.ToObservable(coroutine, false).ToYieldInstruction();
        }

        public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine, bool throwOnError)
        {
            return Observable.ToObservable(coroutine, false).ToYieldInstruction(throwOnError);
        }

        public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine, CancellationToken cancellationToken)
        {
            return Observable.ToObservable(coroutine, false).ToYieldInstruction(cancellationToken);
        }

        public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine, bool throwOnError, CancellationToken cancellationToken)
        {
            return Observable.ToObservable(coroutine, false).ToYieldInstruction(throwOnError, cancellationToken);
        }
    }
}
