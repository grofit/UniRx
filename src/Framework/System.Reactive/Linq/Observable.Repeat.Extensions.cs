﻿using System.Reactive.Operators;

namespace System.Reactive.Linq
{
    public static class ObservableRepeatExtensions
    {
        /// <summary>
        /// Same as Repeat() but if arriving contiguous "OnComplete" Repeat stops.
        /// </summary>
        public static IObservable<T> RepeatSafe<T>(this IObservable<T> source)
        {
            return new RepeatSafeObservable<T>(Observable.RepeatInfinite(source), source.IsRequiredSubscribeOnCurrentThread());
        }

        public static IObservable<T> Repeat<T>(this IObservable<T> source)
        {
            return Observable.RepeatInfinite(source).Concat();
        }
    }
}