using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class Debouncer(TimeSpan delay)
    {
        private readonly TimeSpan _delay = delay;
        private CancellationTokenSource _cancellationTokenSource = new();

        public async Task DebounceAsync(Func<Task> action)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Delay(_delay, _cancellationTokenSource.Token);
                await action();
            }
            catch (TaskCanceledException)
            {
                // Task was canceled, do nothing
            }
        }
    }
}
