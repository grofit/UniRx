#if !(UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2)

using RuntimeUnitTestToolkit;
using System.Reactive;
using System.Reactive.Extensions;

namespace UniRx.Tests
{
    public class _ManualyTest
    {
        public void ToReactiveProperty()
        {
            {
                var rxProp = new ReactiveProperty<int>();
                var calledCount = 0;

                var readRxProp = rxProp.ToReactiveProperty();
                readRxProp.Subscribe(x => calledCount++);

                calledCount.Is(1);
                rxProp.Value = 10;
                calledCount.Is(2);
                rxProp.Value = 10;
                calledCount.Is(2);

                rxProp.SetValueAndForceNotify(10);
                calledCount.Is(2);
            }
            {
                var rxProp = new ReactiveProperty<int>();
                var calledCount = 0;

                var readRxProp = rxProp.ToSequentialReadOnlyReactiveProperty();
                readRxProp.Subscribe(x => calledCount++);

                calledCount.Is(1);
                rxProp.Value = 10;
                calledCount.Is(2);
                rxProp.Value = 10;
                calledCount.Is(2);

                rxProp.SetValueAndForceNotify(10);
                calledCount.Is(3);
            }
        }
    }
}

#endif
