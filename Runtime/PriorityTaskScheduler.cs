using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class PriorityTaskScheduler(int maxConcurrentTasks)
    {
        private readonly ConcurrentDictionary<int, Queue<Func<Task>>> _taskQueues = new();
        private readonly SemaphoreSlim _semaphore = new(maxConcurrentTasks, maxConcurrentTasks);
        private readonly object _lock = new();

        public void EnqueueTask(Func<Task> task, int priority)
        {
            lock (_lock)
            {
                if (!_taskQueues.ContainsKey(priority))
                {
                    _taskQueues[priority] = new Queue<Func<Task>>();
                }

                _taskQueues[priority].Enqueue(task);
            }

            Task.Run(() => ProcessTasks());
        }

        private async Task ProcessTasks()
        {
            await _semaphore.WaitAsync();

            try
            {
                Func<Task> taskToExecute = null;

                lock (_lock)
                {
                    foreach (var priority in _taskQueues.Keys.OrderByDescending(p => p))
                    {
                        if (_taskQueues[priority].Count > 0)
                        {
                            taskToExecute = _taskQueues[priority].Dequeue();
                            if (_taskQueues[priority].Count == 0)
                            {
                                _taskQueues.TryRemove(priority, out _);
                            }
                            break;
                        }
                    }
                }

                if (taskToExecute != null)
                {
                    await taskToExecute();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
