using System;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    public class ParallelTaskRunner
    {
        public static void Main()
        {
            var consumer = TaskRunner.StartUp(10, Console.WriteLine);

            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 14; i++)
                {
                    int j = i;
                    consumer.AddTask(new Task(() => Console.WriteLine(j)));
                }          
                consumer.Dispose();
            });
        }
    }
}
