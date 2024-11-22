using System;
using System.Collections.Generic;

namespace Riverside.Runtime
{
    public class PriorityQueue<T>
    {
        private readonly List<(T item, int priority)> _elements = new();

        public int Count => _elements.Count;

        public void Enqueue(T item, int priority)
        {
            _elements.Add((item, priority));
            _elements.Sort((x, y) => y.priority.CompareTo(x.priority)); // Sort by priority in descending order
        }

        public T Dequeue()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("The priority queue is empty.");

            var item = _elements[0].item;
            _elements.RemoveAt(0);
            return item;
        }

        public T Peek()
        {
            if (_elements.Count == 0)
                throw new InvalidOperationException("The priority queue is empty.");

            return _elements[0].item;
        }
    }
}
