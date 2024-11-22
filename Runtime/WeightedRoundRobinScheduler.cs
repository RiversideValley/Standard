using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class WeightedRoundRobinScheduler<T>(IEnumerable<(T item, int weight)> items)
    {
        private readonly List<(T item, int weight)> _items = items.ToList();
        private int _currentIndex = -1;
        private int _currentWeight = 0;
        private readonly object _lock = new();

        public T GetNext()
        {
            lock (_lock)
            {
                while (true)
                {
                    _currentIndex = (_currentIndex + 1) % _items.Count;
                    if (_currentIndex == 0)
                    {
                        _currentWeight = _currentWeight - Gcd();
                        if (_currentWeight <= 0)
                        {
                            _currentWeight = MaxWeight();
                            if (_currentWeight == 0)
                            {
                                return default;
                            }
                        }
                    }

                    if (_items[_currentIndex].weight >= _currentWeight)
                    {
                        return _items[_currentIndex].item;
                    }
                }
            }
        }

        private int Gcd()
        {
            int gcd = _items[0].weight;
            foreach (var item in _items)
            {
                gcd = Gcd(gcd, item.weight);
            }
            return gcd;
        }

        private int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        private int MaxWeight()
        {
            return _items.Max(i => i.weight);
        }
    }
}
