﻿#if !(UNITY_IPHONE || UNITY_ANDROID || UNITY_METRO)

using System.Reactive;
using System.Reactive.Extensions;
using System.Reactive.Unity.Linq;
using System.Reactive.Unity.Triggers;
using UnityEngine;

// for enable gameObject.EventAsObservbale()

namespace UniRx.Examples
{
    public class Sample03_GameObjectAsObservable : MonoBehaviour
    {
        void Start()
        {
            // All events can subscribe by ***AsObservable if enables UniRx.Triggers
            this.OnMouseDownAsObservable()
                .SelectMany(_ => this.gameObject.UpdateAsObservable())
                .TakeUntil(this.gameObject.OnMouseUpAsObservable())
                .Select(_ => Input.mousePosition)
                .RepeatUntilDestroy(this)
                .Subscribe(x => Debug.Log(x), ()=> Debug.Log("!!!" + "complete"));
        }
    }
}

#endif