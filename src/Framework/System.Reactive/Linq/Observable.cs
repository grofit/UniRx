namespace System.Reactive.Linq
{
    public partial class Observable
    {
        public static readonly TimeSpan InfiniteTimeSpan = new TimeSpan(0, 0, 0, 0, -1); // from .NET 4.5

        // Stops instantiation, but allows for extension
        protected Observable(){ }
    }
}