using System;
using System.Collections;
using System.Reactive.Extensions;
using System.Reactive.Unity;
using System.Reactive.Unity.Linq;
using System.Reactive.Unity.Schedulers;
using UnityEngine;

namespace UniRx.Examples
{
    public class Sample06_ConvertToCoroutine : MonoBehaviour
    {
        // convert IObservable to Coroutine
        void Start()
        {
            StartCoroutine(ComplexCoroutineTest());
        }

        IEnumerator ComplexCoroutineTest()
        {
            yield return new WaitForSeconds(1);

            var v = default(int);
            yield return Observable.Range(1, 10).StartAsCoroutine(x => v = x);

            Debug.Log(v); // 10(callback is last value)
            yield return new WaitForSeconds(3);

            yield return Observable.Return(100).StartAsCoroutine(x => v = x);

            Debug.Log(v); // 100
        }

        // Note:ToAwaitableEnumerator/StartAsCoroutine/LazyTask are obsolete way on Unity 5.3
        // You can use ToYieldInstruction.


        IEnumerator TestNewCustomYieldInstruction()
        {
            // wait Rx Observable.
            yield return Observable.Timer(TimeSpan.FromSeconds(1)).ToYieldInstruction();

            // you can change the scheduler(this is ignore Time.scale)
            yield return Observable.Timer(TimeSpan.FromSeconds(1), Scheduler.MainThreadIgnoreTimeScale).ToYieldInstruction();

            // get return value from ObservableYieldInstruction
            var o = ObservableWWW.Get("http://unity3d.com/").ToYieldInstruction(throwOnError: false);
            yield return o;

            if (o.HasError) { Debug.Log(o.Error.ToString()); }
            if (o.HasResult) { Debug.Log(o.Result); }

            // other sample(wait until transform.position.y >= 100) 
            yield return this.ObserveEveryValueChanged(x => x.transform).FirstOrDefault(x => x.position.y >= 100).ToYieldInstruction();
        }
    }
}