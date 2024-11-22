using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class TokenBucketRateLimiter(int bucketCapacity, int tokensPerInterval, TimeSpan interval)
    {
        private readonly int _bucketCapacity = bucketCapacity;
        private readonly int _tokensPerInterval = tokensPerInterval;
        private readonly TimeSpan _interval = interval;
        private int _tokens = bucketCapacity;
        private DateTime _lastRefill = DateTime.UtcNow;
        private readonly object _lock = new();

        public bool TryConsume()
        {
            lock (_lock)
            {
                RefillTokens();

                if (_tokens > 0)
                {
                    _tokens--;
                    return true;
                }

                return false;
            }
        }

        private void RefillTokens()
        {
            var now = DateTime.UtcNow;
            var timeSinceLastRefill = now - _lastRefill;

            if (timeSinceLastRefill > _interval)
            {
                var tokensToAdd = (int)(timeSinceLastRefill.TotalMilliseconds / _interval.TotalMilliseconds) * _tokensPerInterval;
                _tokens = Math.Min(_bucketCapacity, _tokens + tokensToAdd);
                _lastRefill = now;
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            while (!TryConsume())
            {
                await Task.Delay(_interval, cancellationToken);
            }

            return await operation();
        }

        public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            while (!TryConsume())
            {
                await Task.Delay(_interval, cancellationToken);
            }

            await operation();
        }
    }
}
