using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public static class TimeoutPolicy
    {
        public static async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, TimeSpan timeout)
        {
            using var cts = new CancellationTokenSource();
            var timeoutTask = Task.Delay(timeout, cts.Token);
            var operationTask = operation(cts.Token);

            var completedTask = await Task.WhenAny(operationTask, timeoutTask);

            if (completedTask == timeoutTask)
            {
                throw new TimeoutException("The operation has timed out.");
            }

            cts.Cancel(); // Cancel the timeout task
            return await operationTask; // Await the operation task to propagate any exceptions
        }
    }
}

