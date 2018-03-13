using System.Reactive.Linq;
using System.Reactive.Extensions;
using System.Reactive.Tests.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssertEx = System.Reactive.Tests.Tools.AssertEx;

namespace System.Reactive.Tests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class TakeTest
    {
        [TestMethod]
        public void TakeCount()
        {
            var range = Observable.Range(1, 10);

            AssertEx.Throws<ArgumentOutOfRangeException>(() => range.Take(-1));

            range.Take(0).ToArray().Wait().Length.Is(0);

            range.Take(3).ToArrayWait().IsCollection(1, 2, 3);
            range.Take(15).ToArrayWait().IsCollection(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
        }
    }
}
