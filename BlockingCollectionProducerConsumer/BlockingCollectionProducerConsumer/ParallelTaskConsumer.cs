using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlockingCollectionProducerConsumer
{
    public class ParallelTaskConsumer : IDisposable
    {
        private ParallelTaskConsumer(BlockingCollection<Task> queue, Task[] tasks, CancellationTokenSource cts, Action<string> logger)
        {
            _queue = queue;
            _tasks = tasks;
            _cancellationTokenSource = cts;
            _logger = logger;
        }

        private readonly BlockingCollection<Task> _queue;

        private bool _isDisposed;

        private readonly Action<string> _logger;

        private readonly CancellationTokenSource _cancellationTokenSource;

        private readonly Task[] _tasks;

        public static ParallelTaskConsumer StartUp(int width, Action<string> logger)
        {
            if (width < 1)
            {
                logger("Surely you want at least one taskrunner?");
                return null; ;
            }

            var cts = new CancellationTokenSource();

            var queue = new BlockingCollection<Task>(new ConcurrentQueue<Task>());

            var tasks = Enumerable.Range(0, parallelism)
                .Select(
                x =>
                Task.Factory.StartNew(() => Consume(cts.Token, queue, logger), TaskCreationOptions.LongRunning))
                .ToArray();

            return new ParallelTaskConsumer(tasks, queue, cts, logger);
        }

        public void AddTask(Task task)
        {
            _taskQueue.Add(task);
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
