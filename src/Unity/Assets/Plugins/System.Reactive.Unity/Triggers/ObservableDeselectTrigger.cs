// for uGUI(from 4.6)
#if !(UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5)

using System.Reactive.Subjects;
using UnityEngine;
using UnityEngine.EventSystems;
// require keep for Windows Universal App

namespace System.Reactive.Unity.Triggers
{
    [DisallowMultipleComponent]
    public class ObservableDeselectTrigger : ObservableTriggerBase, IEventSystemHandler, IDeselectHandler
    {
        Subject<BaseEventData> onDeselect;

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            if (onDeselect != null) onDeselect.OnNext(eventData);
        }

        public IObservable<BaseEventData> OnDeselectAsObservable()
        {
            return onDeselect ?? (onDeselect = new Subject<BaseEventData>());
        }

        protected override void RaiseOnCompletedOnDestroy()
        {
            if (onDeselect != null)
            {
                onDeselect.OnCompleted();
            }
        }
    }
}


#endif