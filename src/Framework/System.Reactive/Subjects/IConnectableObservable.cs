namespace System.Reactive.Subjects
{
    public interface IConnectableObservable<T> : IObservable<T>
    {
        IDisposable Connect();
    }
}