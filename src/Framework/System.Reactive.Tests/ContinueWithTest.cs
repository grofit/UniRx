﻿using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Extensions;
using System.Reactive.Subjects;
using System.Reactive.Tests.Tools;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Reactive.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ContinueWithTest
    {
        [TestMethod]
        public void ContinueWith()
        {
            var subject = new Subject<int>();

            var record = subject.ContinueWith(Observable.Return).Record();

            subject.OnNext(10);
            record.Values.Count.Is(0);

            subject.OnNext(100);
            record.Values.Count.Is(0);

            subject.OnCompleted();
            record.Values[0].Is(100);
            record.Notifications.Last().Kind.Is(NotificationKind.OnCompleted);
        }

        [TestMethod]
        public void ContinueWith2()
        {
            var subject = new Subject<int>();

            var record = subject.ContinueWith(x => Observable.Return(x).Delay(TimeSpan.FromMilliseconds(100))).Record();

            subject.OnNext(10);
            record.Values.Count.Is(0);

            subject.OnNext(100);
            record.Values.Count.Is(0);

            subject.OnCompleted();
            Thread.Sleep(TimeSpan.FromMilliseconds(200));
            record.Values[0].Is(100);
            record.Notifications.Last().Kind.Is(NotificationKind.OnCompleted);
        }
    }
}
