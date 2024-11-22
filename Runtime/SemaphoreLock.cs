using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class SemaphoreLock(int maxConcurrentRequests)
    {
        private readonly SemaphoreSlim _semaphore = new(maxConcurrentRequests, maxConcurrentRequests);

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                return await operation();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);

            try
            {
                await operation();
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
