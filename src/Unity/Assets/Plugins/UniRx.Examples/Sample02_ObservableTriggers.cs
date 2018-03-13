using UnityEngine;
// Triggers Namepsace
using System.Reactive;
using System.Reactive.Unity.Linq;
using System.Reactive.Unity.Triggers;

namespace UniRx.Examples
{
    public class Sample02_ObservableTriggers : MonoBehaviour
    {
        void Start()
        {
            // Get the plain object
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Add ObservableXxxTrigger for handle MonoBehaviour's event as Observable
            cube.AddComponent<ObservableUpdateTrigger>()
                .UpdateAsObservable()
                .SampleFrame(30)
                .Subscribe(x => Debug.Log("cube"), () => Debug.Log("destroy"));

            // destroy after 3 second:)
            GameObject.Destroy(cube, 3f);
        }
    }
}