﻿using System.Reactive.Operators;

namespace System.Reactive.Linq
{
    public static partial class Observable
    {
        public static T Wait<T>(this IObservable<T> source)
        {
            return new Wait<T>(source, InfiniteTimeSpan).Run();
        }

        public static T Wait<T>(this IObservable<T> source, TimeSpan timeout)
        {
            return new Wait<T>(source, timeout).Run();
        }
    }
}
