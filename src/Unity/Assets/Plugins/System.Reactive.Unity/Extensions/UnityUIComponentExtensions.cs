using System.Reactive.Unity.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace System.Reactive.Unity.Extensions
{
    public static class UnityUIComponentExtensions
    {
        public static IDisposable SubscribeToText(this IObservable<string> source, UnityEngine.UI.Text text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x);
        }

        public static IDisposable SubscribeToText<T>(this IObservable<T> source, UnityEngine.UI.Text text)
        {
            return source.SubscribeWithState(text, (x, t) => t.text = x.ToString());
        }

        public static IDisposable SubscribeToText<T>(this IObservable<T> source, UnityEngine.UI.Text text, Func<T, string> selector)
        {
            return source.SubscribeWithState2(text, selector, (x, t, s) => t.text = s(x));
        }

        public static IDisposable SubscribeToInteractable(this IObservable<bool> source, Selectable selectable)
        {
            return source.SubscribeWithState(selectable, (x, s) => s.interactable = x);
        }

        /// <summary>Observe onClick event.</summary>
        public static IObservable<Unit> OnClickAsObservable(this Button button)
        {
            return button.onClick.AsObservable();
        }

        /// <summary>Observe onValueChanged with current `isOn` value on subscribe.</summary>
        public static IObservable<bool> OnValueChangedAsObservable(this Toggle toggle)
        {
            // Optimized Defer + StartWith
            return Observable.CreateWithState<bool, Toggle>(toggle, (t, observer) =>
            {
                observer.OnNext(t.isOn);
                return t.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        /// <summary>Observe onValueChanged with current `value` on subscribe.</summary>
        public static IObservable<float> OnValueChangedAsObservable(this Scrollbar scrollbar)
        {
            return Observable.CreateWithState<float, Scrollbar>(scrollbar, (s, observer) =>
            {
                observer.OnNext(s.value);
                return s.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        /// <summary>Observe onValueChanged with current `normalizedPosition` value on subscribe.</summary>
        public static IObservable<Vector2> OnValueChangedAsObservable(this ScrollRect scrollRect)
        {
            return Observable.CreateWithState<Vector2, ScrollRect>(scrollRect, (s, observer) =>
            {
                observer.OnNext(s.normalizedPosition);
                return s.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        /// <summary>Observe onValueChanged with current `value` on subscribe.</summary>
        public static IObservable<float> OnValueChangedAsObservable(this Slider slider)
        {
            return Observable.CreateWithState<float, Slider>(slider, (s, observer) =>
            {
                observer.OnNext(s.value);
                return s.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        /// <summary>Observe onEndEdit(Submit) event.</summary>
        public static IObservable<string> OnEndEditAsObservable(this InputField inputField)
        {
            return inputField.onEndEdit.AsObservable();
        }

        /// <summary>Observe onValueChange with current `text` value on subscribe.</summary>

        [Obsolete("onValueChange has been renamed to onValueChanged")]
        public static IObservable<string> OnValueChangeAsObservable(this InputField inputField)
        {
            return Observable.CreateWithState<string, InputField>(inputField, (i, observer) =>
            {
                observer.OnNext(i.text);
                return i.onValueChanged.AsObservable().Subscribe(observer);
            });
        }

        /// <summary>Observe onValueChanged with current `text` value on subscribe.</summary>
        public static IObservable<string> OnValueChangedAsObservable(this InputField inputField)
        {
            return Observable.CreateWithState<string, InputField>(inputField, (i, observer) =>
            {
                observer.OnNext(i.text);
                return i.onValueChanged.AsObservable().Subscribe(observer);
            });
        }
    }
}