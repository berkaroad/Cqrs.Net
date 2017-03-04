using System;
using System.Threading;
using System.Threading.Tasks;

namespace CqrsFramework.Tasks
{
    public static class TimerTaskFactory
    {
        private static readonly TimeSpan DoNotRepeat = TimeSpan.FromMilliseconds(-1);

        public static Task<T> StartNew<T>(Func<T> getResult, Func<T, bool> isResultValid, TimeSpan pollInterval, TimeSpan timeout)
        {
            Timer timer = null;
            TaskCompletionSource<T> taskCompletionSource = null;
            DateTime expirationTime = DateTime.UtcNow.Add(timeout);
            timer =
                new Timer(_ =>
                {
                    try
                    {
                        if (DateTime.UtcNow > expirationTime)
                        {
                            timer.Dispose();
                            taskCompletionSource.SetResult(default(T));
                            return;
                        }

                        var result = getResult();

                        if (isResultValid(result))
                        {
                            timer.Dispose();
                            taskCompletionSource.SetResult(result);
                        }
                        else
                        {
                            // try again
                            timer.Change(pollInterval, DoNotRepeat);
                        }
                    }
                    catch (Exception e)
                    {
                        timer.Dispose();
                        taskCompletionSource.SetException(e);
                    }
                }, null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            taskCompletionSource = new TaskCompletionSource<T>(timer);

            timer.Change(pollInterval, DoNotRepeat);

            return taskCompletionSource.Task;
        }
    }
}
