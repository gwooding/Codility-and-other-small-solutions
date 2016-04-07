using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlockingCollectionProducerConsumer
{
    public class ParallelTaskConsumer : IDisposable
    {
        private ParallelTaskConsumer(Task[] tasks, BlockingCollection<Task> taskQueue, CancellationTokenSource cts, Action<string> logger)
        {
            _taskQueue = taskQueue;
            _tasks = tasks;
            _cts = cts;
            _logger = logger;
        }

        private readonly BlockingCollection<Task> _taskQueue;

        private bool _isDisposed;

        private readonly Action<string> _logger;

        private readonly CancellationTokenSource _cts;

        private readonly Task[] _tasks;

        public static ParallelTaskConsumer StartNew(int parallelism, Action<string> logger)
        {
            if (parallelism < 1)
            {
                logger("Surely you want at least one taskrunner?");
                return null; ;
            }

            var cts = new CancellationTokenSource();

            var taskQueue = new BlockingCollection<Task>(new ConcurrentQueue<Task>());

            var tasks = Enumerable.Range(0, parallelism)
                .Select(
                x =>
                Task.Factory.StartNew(() => Consume(cts.Token, taskQueue, logger), TaskCreationOptions.LongRunning))
                .ToArray();

            return new ParallelTaskConsumer(tasks, taskQueue, cts, logger);
        }

        public void EnqueueTask(Task task)
        {
            _taskQueue.Add(task);
        }

        private static void Consume(CancellationToken token, BlockingCollection<Task> taskQueue, Action<string> logger)
        {
            while (true)
            {
                try
                {
                    Task item;
                    try
                    {
                        item = taskQueue.Take(token);
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
            _taskQueue.CompleteAdding();
            _cts.Cancel();

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
            _taskQueue.Dispose();
            _isDisposed = true;
        }
    }
}
