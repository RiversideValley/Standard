using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class FixedWindowRateLimiter(int maxRequests, TimeSpan timeWindow)
    {
        private readonly int _maxRequests = maxRequests;
        private readonly TimeSpan _timeWindow = timeWindow;
        private readonly ConcurrentDictionary<DateTime, int> _requestCounts = new();
        private readonly object _lock = new();

        public bool TryConsume()
        {
            lock (_lock)
            {
                DateTime now = DateTime.UtcNow;
                DateTime windowStart = now - _timeWindow;

                // Remove old entries
                foreach (var key in _requestCounts.Keys)
                {
                    if (key < windowStart)
                    {
                        _requestCounts.TryRemove(key, out _);
                    }
                }

                // Calculate the total number of requests in the current window
                int requestCount = 0;
                foreach (var count in _requestCounts.Values)
                {
                    requestCount += count;
                }

                if (requestCount < _maxRequests)
                {
                    _requestCounts.AddOrUpdate(now, 1, (key, value) => value + 1);
                    return true;
                }

                return false;
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            while (!TryConsume())
            {
                await Task.Delay(_timeWindow, cancellationToken);
            }

            return await operation();
        }

        public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            while (!TryConsume())
            {
                await Task.Delay(_timeWindow, cancellationToken);
            }

            await operation();
        }
    }
}
