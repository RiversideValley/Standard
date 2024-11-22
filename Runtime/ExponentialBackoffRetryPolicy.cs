using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public static class ExponentialBackoffRetryPolicy
    {
        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, int maxRetries = 5, TimeSpan? initialDelay = null, CancellationToken cancellationToken = default)
        {
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries), "Max retries must be greater than or equal to 1.");

            initialDelay ??= TimeSpan.FromSeconds(1);
            var delay = initialDelay.Value;

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (attempt < maxRetries)
                {
                    Console.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying in {delay.TotalSeconds} seconds...");
                    await Task.Delay(delay, cancellationToken);
                    delay = TimeSpan.FromTicks(delay.Ticks * 2); // Exponential backoff
                }
            }

            // If the operation still fails after the maximum number of retries, rethrow the last exception
            return await operation();
        }
    }
}

