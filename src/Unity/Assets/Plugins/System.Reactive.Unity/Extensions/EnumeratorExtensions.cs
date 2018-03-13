using System.Collections;
using System.Reactive.Unity.Linq;
using System.Threading;

namespace System.Reactive.Unity.Extensions
{
    public static class EnumeratorExtensions
    {
        public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine) =>
            Observable.ToYieldInstruction(coroutine);

        public static ObservableYieldInstruction<Unit>
            ToYieldInstruction(this IEnumerator coroutine, bool throwOnError) =>
            Observable.ToYieldInstruction(coroutine, throwOnError);

        public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine,
            CancellationToken cancellationToken) => Observable.ToYieldInstruction(coroutine, cancellationToken);

        public static ObservableYieldInstruction<Unit> ToYieldInstruction(this IEnumerator coroutine, bool throwOnError,
            CancellationToken cancellationToken) =>
            Observable.ToYieldInstruction(coroutine, throwOnError, cancellationToken);
    }
}
