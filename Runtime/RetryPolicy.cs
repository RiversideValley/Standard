using System;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public static class RetryPolicy
    {
        public static async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, int maxRetries = 3, TimeSpan? delay = null)
        {
            if (maxRetries < 1)
                throw new ArgumentOutOfRangeException(nameof(maxRetries), "Max retries must be greater than or equal to 1.");

            delay ??= TimeSpan.FromSeconds(2);

            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    return await operation();
                }
                catch (Exception ex) when (attempt < maxRetries)
                {
                    Console.WriteLine($"Attempt {attempt} failed: {ex.Message}. Retrying in {delay.Value.TotalSeconds} seconds...");
                    await Task.Delay(delay.Value);
                }
            }

            // If the operation still fails after the maximum number of retries, rethrow the last exception
            return await operation();
        }
    }
}
