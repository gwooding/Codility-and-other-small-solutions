using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReadersWriters
{
    class Program
    {
        static BlockingCollection<Action> readerActionQueue = new BlockingCollection<Action>(new ConcurrentQueue<Action>());
        static BlockingCollection<Action> writerActionQueue = new BlockingCollection<Action>(new ConcurrentQueue<Action>());

        static StringBuilder  word = new StringBuilder("");

        static bool writeReady;

        static bool readStopped;

        static List<Task[]> taskCollectionList = new List<Task[]>();
        
        static void Main(string[] args)
        {
            Task.Run(() => enQueueReader());
            Task.Run(() => enQueueWriter());
            Task.Run(() => ReadersConsume(5));
            Task.Run(() => WritersConsume());
            Console.ReadLine();
        }

        static void ReadersConsume(int readerParallelismWidth)
        {
            while (true)
            {
                if (!readStopped)
                {
                    Task[] tasks = new Task[readerParallelismWidth];
                    for (int i = 0; i < tasks.Length; i++)
                    {
                        tasks[i] = Task.Run(() => {
                            Action action;
                            bool actionAvailable = readerActionQueue.TryTake(out action);
                            if (actionAvailable)
                            {
                                action();
                            }
                        });
                    }
                    taskCollectionList.Add(tasks);

                    if (writeReady)
                    {
                        foreach(var taskList in taskCollectionList)
                        {
                            Task.WaitAll(taskList);
                        }
                        readStopped = true;
                    }
                }
            }
        }

        static void WritersConsume()
        {
            while (true)
            {
                Action action;
                if (writerActionQueue.Count > 0)
                {
                    writeReady = true;
                    if (readStopped)
                    {
                        if(writerActionQueue.TryTake(out action))
                        {
                            action();
                        }
                        readStopped = false;
                        writeReady = false;
                    }
                }
            }
        }

        static void enQueueReader()
        {
            int j = 0;
            while (j < 1000)
            {
                readerActionQueue.Add(() => Console.WriteLine(word.ToString()));
                j++;
            }
        }

        static void enQueueWriter()
        {
            int j = 0;
            while (j < 1000)
            {
                writerActionQueue.Add(() => { word.Append('a'); Console.WriteLine("WRITE"); });
                j++;
            }
        }
    }
}
