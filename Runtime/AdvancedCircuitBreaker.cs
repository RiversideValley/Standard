using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class AdvancedCircuitBreaker(int maxFailures, TimeSpan resetTimeout)
    {
        private readonly int _maxFailures = maxFailures;
        private readonly TimeSpan _resetTimeout = resetTimeout;
        private int _failureCount = 0;
        private DateTime _lastFailureTime = DateTime.MinValue;
        private CircuitBreakerState _state = CircuitBreakerState.Closed;
        private readonly object _lock = new();

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> operation)
        {
            lock (_lock)
            {
                if (_state == CircuitBreakerState.Open)
                {
                    if (DateTime.UtcNow - _lastFailureTime > _resetTimeout)
                    {
                        _state = CircuitBreakerState.HalfOpen;
                    }
                    else
                    {
                        throw new InvalidOperationException("Circuit breaker is open.");
                    }
                }
            }

            try
            {
                T result = await operation();
                lock (_lock)
                {
                    _failureCount = 0;
                    _state = CircuitBreakerState.Closed;
                }
                return result;
            }
            catch
            {
                lock (_lock)
                {
                    _failureCount++;
                    _lastFailureTime = DateTime.UtcNow;

                    if (_failureCount >= _maxFailures)
                    {
                        _state = CircuitBreakerState.Open;
                    }
                    else
                    {
                        _state = CircuitBreakerState.HalfOpen;
                    }
                }
                throw;
            }
        }

        private enum CircuitBreakerState
        {
            Closed,
            Open,
            HalfOpen
        }
    }
}
