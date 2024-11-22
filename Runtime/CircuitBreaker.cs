using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class CircuitBreaker
    {
        private readonly int _maxFailures;
        private readonly TimeSpan _resetTimeout;
        private int _failureCount;
        private DateTime _lastFailureTime;
        private bool _isOpen;

        public CircuitBreaker(int maxFailures, TimeSpan resetTimeout)
        {
            _maxFailures = maxFailures;
            _resetTimeout = resetTimeout;
            _failureCount = 0;
            _lastFailureTime = DateTime.MinValue;
            _isOpen = false;
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
        {
            if (_isOpen)
            {
                if (DateTime.UtcNow - _lastFailureTime > _resetTimeout)
                {
                    // Reset the circuit breaker
                    _isOpen = false;
                    _failureCount = 0;
                }
                else
                {
                    throw new InvalidOperationException("Circuit breaker is open.");
                }
            }

            try
            {
                T result = await operation();
                _failureCount = 0; // Reset failure count on success
                return result;
            }
            catch
            {
                _failureCount++;
                _lastFailureTime = DateTime.UtcNow;

                if (_failureCount >= _maxFailures)
                {
                    _isOpen = true;
                }

                throw;
            }
        }
    }
}
