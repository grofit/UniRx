﻿using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;

namespace System.Reactive.Schedulers
{
    public static class SchedulerExtensions
    {
        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action action)
        {
            return scheduler.Schedule(dueTime - scheduler.Now, action);
        }

        public static IDisposable Schedule(this IScheduler scheduler, Action<Action> action)
        {
            // InvokeRec1
            var group = new CompositeDisposable(1);
            var gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(() =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(() =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }
                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(recursiveAction));

            return group;
        }

        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan dueTime, Action<Action<TimeSpan>> action)
        {
            // InvokeRec2

            var group = new CompositeDisposable(1);
            var gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(dt =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(dt, () =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }
                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(dueTime, recursiveAction));

            return group;
        }

        public static IDisposable Schedule(this IScheduler scheduler, DateTimeOffset dueTime, Action<Action<DateTimeOffset>> action)
        {
            // InvokeRec3

            var group = new CompositeDisposable(1);
            var gate = new object();

            Action recursiveAction = null;
            recursiveAction = () => action(dt =>
            {
                var isAdded = false;
                var isDone = false;
                var d = default(IDisposable);
                d = scheduler.Schedule(dt, () =>
                {
                    lock (gate)
                    {
                        if (isAdded)
                            group.Remove(d);
                        else
                            isDone = true;
                    }
                    recursiveAction();
                });

                lock (gate)
                {
                    if (!isDone)
                    {
                        group.Add(d);
                        isAdded = true;
                    }
                }
            });

            group.Add(scheduler.Schedule(dueTime, recursiveAction));

            return group;
        }
    }
}
