using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class TaskRunner : IDisposable
    {
        private readonly BlockingCollection<Task> _queue;

        private bool _isDisposed;

        private readonly Action<string> _logger;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly Task[] _tasks;

        private TaskRunner(BlockingCollection<Task> queue, Task[] tasks, CancellationTokenSource cancellationTokenSource, Action<string> logger)
        {
            _queue = queue;
            _tasks = tasks;
            _cancellationTokenSource = cancellationTokenSource;
            _logger = logger;
        }

        public static TaskRunner StartUp(int width, Action<string> logger)
        {
            if (width < 1)
            {
                logger("Surely you want at least one taskrunner?");
                return null; ;
            }

            var queue = new BlockingCollection<Task>(new ConcurrentQueue<Task>());

            var cancellationTokenSource = new CancellationTokenSource();

            var tasks = Enumerable.Range(0, width)
                .Select(
                x =>
                Task.Factory.StartNew(() => Consume(cancellationTokenSource.Token, queue, logger), TaskCreationOptions.LongRunning))
                .ToArray();

            return new TaskRunner(queue, tasks, cancellationTokenSource, logger);
        }

        private static void Consume(CancellationToken token, BlockingCollection<Task> queue, Action<string> logger)
        {
            while (true)
            {
                try
                {
                    Task item;
                    try
                    {
                        item = queue.Take(token);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    item.Start();
                }
                catch (Exception ex)
                {
                    logger(string.Format(ex.Message));
                }
            }
        }

        public void AddTask(Task task)
        {
            _queue.Add(task);
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }
            _queue.CompleteAdding();
            _cancellationTokenSource.Cancel();

            Task.WaitAll(_tasks);

            foreach (var task in _tasks)
            {
                try
                {
                    task.Dispose();
                }
                catch (Exception ex)
                {
                    _logger(string.Format(ex.Message));
                }
            }
            _queue.Dispose();
            _isDisposed = true;
        }
    }
}
