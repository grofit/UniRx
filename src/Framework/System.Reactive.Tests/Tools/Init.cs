using System.Reactive.Schedulers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Reactive.Tests.Tools
{
    [TestClass]
    public class Init
    {
        [AssemblyInitialize]
        public static void Initialize(TestContext ctx)
        {
            Scheduler.DefaultSchedulers.SetDotNetCompatible();
        }
    }
}