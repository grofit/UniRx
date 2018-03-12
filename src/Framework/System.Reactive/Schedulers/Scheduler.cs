namespace System.Reactive.Schedulers
{
    // Scheduler Extension
    public partial class Scheduler
    {
        private Scheduler() { }

        // configurable defaults
        public static class DefaultSchedulers
        {
            static IScheduler constantTime;
            public static IScheduler ConstantTimeOperations
            {
                get => constantTime ?? (constantTime = Scheduler.Immediate);
                set => constantTime = value;
            }

            static IScheduler tailRecursion;
            public static IScheduler TailRecursion
            {
                get
                {
                    return tailRecursion ?? (tailRecursion = Scheduler.Immediate);
                }
                set
                {
                    tailRecursion = value;
                }
            }

            static IScheduler iteration;
            public static IScheduler Iteration
            {
                get => iteration ?? (iteration = Scheduler.CurrentThread);
                set => iteration = value;
            }

            static IScheduler timeBasedOperations;
            public static IScheduler TimeBasedOperations
            {
                get => timeBasedOperations ?? (timeBasedOperations = Scheduler.ThreadPool);
                set => timeBasedOperations = value;
            }

            static IScheduler asyncConversions;
            public static IScheduler AsyncConversions
            {
                get
                {
#if WEB_GL
                    // WebGL does not support threadpool
                    return asyncConversions ?? (asyncConversions = Scheduler.MainThread);
#else
                    return asyncConversions ?? (asyncConversions = Scheduler.ThreadPool);
#endif
                }
                set
                {
                    asyncConversions = value;
                }
            }

            public static void SetDotNetCompatible()
            {
                ConstantTimeOperations = Scheduler.Immediate;
                TailRecursion = Scheduler.Immediate;
                Iteration = Scheduler.CurrentThread;
                TimeBasedOperations = Scheduler.ThreadPool;
                AsyncConversions = Scheduler.ThreadPool;
            }
        }

        // utils

        public static DateTimeOffset Now => DateTimeOffset.UtcNow;

        public static TimeSpan Normalize(TimeSpan timeSpan)
        {
            return timeSpan >= TimeSpan.Zero ? timeSpan : TimeSpan.Zero;
        }
    }
}