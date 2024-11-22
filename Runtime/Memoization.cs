using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public static class Memoization
    {
        public static Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> func)
        {
            var cache = new ConcurrentDictionary<T, TResult>();
            return arg =>
            {
                if (cache.TryGetValue(arg, out var result))
                {
                    return result;
                }
                result = func(arg);
                cache[arg] = result;
                return result;
            };
        }

        public static Func<T, Task<TResult>> MemoizeAsync<T, TResult>(Func<T, Task<TResult>> func)
        {
            var cache = new ConcurrentDictionary<T, Task<TResult>>();
            return async arg =>
            {
                if (cache.TryGetValue(arg, out var result))
                {
                    return await result;
                }
                result = func(arg);
                cache[arg] = result;
                return await result;
            };
        }
    }
}
