using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class ExponentialBackoffRateLimiter(int maxRetries, TimeSpan initialDelay, TimeSpan maxDelay)
    {
        private readonly int _maxRetries = maxRetries;
        private readonly TimeSpan _initialDelay = initialDelay;
        private readonly TimeSpan _maxDelay = maxDelay;
        private readonly object _lock = new();

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            int attempt = 0;
            TimeSpan delay = _initialDelay;

            while (attempt < _maxRetries)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (attempt < _maxRetries)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying in {delay.TotalSeconds} seconds...");
                    await Task.Delay(delay, cancellationToken);
                    delay = TimeSpan.FromTicks(Math.Min(delay.Ticks * 2, _maxDelay.Ticks)); // Exponential backoff
                }
            }

            // If the operation still fails after the maximum number of retries, rethrow the last exception
            return await operation();
        }

        public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            int attempt = 0;
            TimeSpan delay = _initialDelay;

            while (attempt < _maxRetries)
            {
                try
                {
                    await operation();
                    return;
                }
                catch (Exception ex) when (attempt < _maxRetries)
                {
                    attempt++;
                    Console.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying in {delay.TotalSeconds} seconds...");
                    await Task.Delay(delay, cancellationToken);
                    delay = TimeSpan.FromTicks(Math.Min(delay.Ticks * 2, _maxDelay.Ticks)); // Exponential backoff
                }
            }

            // If the operation still fails after the maximum number of retries, rethrow the last exception
            await operation();
        }
    }
}
