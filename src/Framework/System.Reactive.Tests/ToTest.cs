﻿using System.Reactive.Linq;
using System.Reactive.Extensions;
using System.Reactive.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Reactive.Tests
{
    [TestClass]
    public class ToTest
    {

        [TestMethod]
        public void ToArray()
        {
            Observable.Empty<int>().ToArray().Wait().IsCollection();
            Observable.Return(10).ToArray().Wait().IsCollection(10);
            Observable.Range(1, 10).ToArray().Wait().IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }

        [TestMethod]
        public void ToList()
        {
            Observable.Empty<int>().ToList().Wait().IsCollection();
            Observable.Return(10).ToList().Wait().IsCollection(10);
            Observable.Range(1, 10).ToList().Wait().IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }
    }
}
