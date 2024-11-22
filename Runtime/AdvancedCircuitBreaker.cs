using System;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class AdvancedCircuitBreaker
    {
        private readonly int _maxFailures;
        private readonly TimeSpan _resetTimeout;
        private int _failureCount;
        private DateTime _lastFailureTime;
        private CircuitBreakerState _state;
        private readonly object _lock = new();

        public AdvancedCircuitBreaker(int maxFailures, TimeSpan resetTimeout)
        {
            _maxFailures = maxFailures;
            _resetTimeout = resetTimeout;
            _failureCount = 0;
            _lastFailureTime = DateTime.MinValue;
            _state = CircuitBreakerState.Closed;
        }

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
