using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class LeakyBucketRateLimiter(int bucketCapacity, TimeSpan leakInterval)
    {
        private readonly int _bucketCapacity = bucketCapacity;
        private readonly TimeSpan _leakInterval = leakInterval;
        private int _currentBucketSize = 0;
        private DateTime _lastLeakTime = DateTime.UtcNow;
        private readonly object _lock = new();

        public bool TryConsume()
        {
            lock (_lock)
            {
                Leak();

                if (_currentBucketSize < _bucketCapacity)
                {
                    _currentBucketSize++;
                    return true;
                }

                return false;
            }
        }

        private void Leak()
        {
            var now = DateTime.UtcNow;
            var timeSinceLastLeak = now - _lastLeakTime;

            if (timeSinceLastLeak > _leakInterval)
            {
                var leaks = (int)(timeSinceLastLeak.TotalMilliseconds / _leakInterval.TotalMilliseconds);
                _currentBucketSize = Math.Max(0, _currentBucketSize - leaks);
                _lastLeakTime = now;
            }
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            while (!TryConsume())
            {
                await Task.Delay(_leakInterval, cancellationToken);
            }

            return await operation();
        }

        public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            while (!TryConsume())
            {
                await Task.Delay(_leakInterval, cancellationToken);
            }

            await operation();
        }
    }
}
