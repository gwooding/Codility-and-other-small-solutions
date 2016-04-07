
using System;
using System.Threading.Tasks;

namespace BlockingCollectionProducerConsumer
{
    public class ParallelTaskRunner
    {
        public static void Main()
        {
            var consumer = ParallelTaskConsumer.StartNew(10, Console.WriteLine);

            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 14; i++)
                {
                    int j = i;
                    consumer.EnqueueTask(new Task(() => Console.WriteLine(j)));
                }

                consumer.Dispose();
            });

            Console.ReadLine();
        }
    }
}
