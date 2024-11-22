using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class SlidingLogRateLimiter(int maxRequests, TimeSpan timeWindow)
    {
        private readonly int _maxRequests = maxRequests;
        private readonly TimeSpan _timeWindow = timeWindow;
        private readonly ConcurrentQueue<DateTime> _requestLog = new();
        private readonly object _lock = new();

        public bool TryConsume()
        {
            lock (_lock)
            {
                DateTime now = DateTime.UtcNow;
                DateTime windowStart = now - _timeWindow;

                // Remove timestamps outside the time window
                while (_requestLog.TryPeek(out DateTime timestamp) && timestamp < windowStart)
                {
                    _requestLog.TryDequeue(out _);
                }

                if (_requestLog.Count < _maxRequests)
                {
                    _requestLog.Enqueue(now);
                    return true;
                }

                return false;
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            while (!TryConsume())
            {
                await Task.Delay((int)(_timeWindow.TotalMilliseconds / _maxRequests), cancellationToken);
            }

            return await operation();
        }

        public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            while (!TryConsume())
            {
                await Task.Delay((int)(_timeWindow.TotalMilliseconds / _maxRequests), cancellationToken);
            }

            await operation();
        }
    }
}
