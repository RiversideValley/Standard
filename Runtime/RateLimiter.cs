using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class RateLimiter(int maxRequests, TimeSpan timeWindow)
    {
        private readonly int _maxRequests = maxRequests;
        private readonly TimeSpan _timeWindow = timeWindow;
        private readonly ConcurrentQueue<DateTime> _requestTimestamps = new();

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            while (true)
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
                    return await operation();
                }

                // Wait for the next available slot
                if (_requestTimestamps.TryPeek(out DateTime nextAvailableTime))
                {
                    await Task.Delay(_timeWindow - (now - nextAvailableTime), cancellationToken);
                }
            }
        }
    }
}
