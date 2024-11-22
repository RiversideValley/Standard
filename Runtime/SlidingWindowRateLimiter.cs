using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class SlidingWindowRateLimiter(int maxRequests, TimeSpan timeWindow)
    {
        private readonly int _maxRequests = maxRequests;
        private readonly TimeSpan _timeWindow = timeWindow;
        private readonly ConcurrentQueue<DateTime> _requestTimestamps = new();

        public bool IsRequestAllowed()
        {
            DateTime now = DateTime.UtcNow;

            // Remove timestamps outside the time window
            while (_requestTimestamps.TryPeek(out DateTime timestamp) && (now - timestamp) > _timeWindow)
            {
                _requestTimestamps.TryDequeue(out _);
            }

            if (_requestTimestamps.Count < _maxRequests)
            {
                _requestTimestamps.Enqueue(now);
                return true;
            }

            return false;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            while (!IsRequestAllowed())
            {
                await Task.Delay((int)(_timeWindow.TotalMilliseconds / _maxRequests), cancellationToken);
            }

            return await operation();
        }

        public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            while (!IsRequestAllowed())
            {
                await Task.Delay((int)(_timeWindow.TotalMilliseconds / _maxRequests), cancellationToken);
            }

            await operation();
        }
    }
}
