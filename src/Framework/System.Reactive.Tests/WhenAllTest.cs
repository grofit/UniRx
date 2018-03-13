﻿using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Extensions;
using System.Reactive.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Reactive.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class WhenAllTest
    {
        [TestMethod]
        public void WhenAllEmpty()
        {
            var xs = Observable.WhenAll(new IObservable<int>[0]).Wait();
            xs.Length.Is(0);

            var xs2 = Observable.WhenAll(Enumerable.Empty<IObservable<int>>().Select(x => x)).Wait();
            xs2.Length.Is(0);
        }

        [TestMethod]
        public void WhenAll()
        {
            var xs = Observable.WhenAll(
                    Observable.Return(100),
                    Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => 5),
                    Observable.Range(1, 4))
                .Wait();

            xs.IsCollection(100, 5, 4);
        }

        [TestMethod]
        public void WhenAllEnumerable()
        {
            var xs = new[] {
                    Observable.Return(100),
                    Observable.Timer(TimeSpan.FromSeconds(1)).Select(_ => 5),
                    Observable.Range(1, 4)
            }.Select(x => x).WhenAll().Wait();

            xs.IsCollection(100, 5, 4);
        }

        [TestMethod]
        public void WhenAllUnitEmpty()
        {
            var xs = Observable.WhenAll(new IObservable<Unit>[0]).Wait();
            xs.Is(Unit.Default);

            var xs2 = Observable.WhenAll(Enumerable.Empty<IObservable<Unit>>().Select(x => x)).Wait();
            xs2.Is(Unit.Default);
        }

        [TestMethod]
        public void WhenAllUnit()
        {
            var xs = Observable.WhenAll(
                    Observable.Return(100).AsUnitObservable(),
                    Observable.Timer(TimeSpan.FromSeconds(1)).AsUnitObservable(),
                    Observable.Range(1, 4).AsUnitObservable())
                .Wait();

            xs.Is(Unit.Default);
        }

        [TestMethod]
        public void WhenAllUnitEnumerable()
        {
            var xs = new[] {
                    Observable.Return(100).AsUnitObservable(),
                    Observable.Timer(TimeSpan.FromSeconds(1)).AsUnitObservable(),
                    Observable.Range(1, 4).AsUnitObservable()
            }.Select(x => x).WhenAll().Wait();

            xs.Is(Unit.Default);
        }
    }
}
