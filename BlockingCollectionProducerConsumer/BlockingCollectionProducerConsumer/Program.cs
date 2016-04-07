
using System;
using System.Threading.Tasks;

namespace BlockingCollectionProducerConsumer
{
    public class ParallelTaskRunner
    {
        public static void Main()
        {
            var consumerGrid = ParallelTaskConsumer.StartUp(10, Console.WriteLine);

            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 14; i++)
                {
                    int j = i;
                    consumerGrid.Add(new Task(() => Console.WriteLine(j)));
                }

                consumerGrid.Dispose();
            });

            Console.ReadLine();
        }
    }
}
