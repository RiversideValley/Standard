using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class Throttler
    {
        private readonly SemaphoreSlim _semaphore;
        private readonly TimeSpan _timeWindow;
        private DateTime _lastExecutionTime;

        public Throttler(int maxRequests, TimeSpan timeWindow)
        {
            _semaphore = new SemaphoreSlim(maxRequests, maxRequests);
            _timeWindow = timeWindow;
            _lastExecutionTime = DateTime.MinValue;
        }

        public async Task ThrottleAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan timeSinceLastExecution = now - _lastExecutionTime;

                if (timeSinceLastExecution < _timeWindow)
                {
                    await Task.Delay(_timeWindow - timeSinceLastExecution, cancellationToken);
                }

                _lastExecutionTime = DateTime.UtcNow;
                await action();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}

