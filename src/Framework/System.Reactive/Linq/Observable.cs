namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        public static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1); // from .NET 4.5
    }
}