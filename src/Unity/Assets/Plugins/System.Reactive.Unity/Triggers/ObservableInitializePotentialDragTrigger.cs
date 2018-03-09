// for uGUI(from 4.6)
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)

using System.Reactive.Subjects;
using UnityEngine;
using UnityEngine.EventSystems;
// require keep for Windows Universal App

namespace System.Reactive.Unity.Triggers
{
    [DisallowMultipleComponent]
    public class ObservableInitializePotentialDragTrigger : ObservableTriggerBase, IEventSystemHandler, IInitializePotentialDragHandler
    {
        Subject<PointerEventData> onInitializePotentialDrag;

        void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (onInitializePotentialDrag != null) onInitializePotentialDrag.OnNext(eventData);
        }

        public IObservable<PointerEventData> OnInitializePotentialDragAsObservable()
        {
            return onInitializePotentialDrag ?? (onInitializePotentialDrag = new Subject<PointerEventData>());
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            if (onInitializePotentialDrag != null)
            {
                onInitializePotentialDrag.OnCompleted();
            }
        }
    }
}


#endif