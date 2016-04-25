using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReadersWriters
{
    class Program
    {
        static BlockingCollection<Action> ReaderActionQueue = new BlockingCollection<Action>(new ConcurrentQueue<Action>());
        static BlockingCollection<Action> WriterActionQueue = new BlockingCollection<Action>(new ConcurrentQueue<Action>());

        static StringBuilder  Word = new StringBuilder("");

        static bool WriteReady;

        static bool ReadStopped;

        static List<Task[]> TaskCollectionList = new List<Task[]>();
        
        static void Main(string[] args)
        {
            Task.Run(() => EnqueueReader());
            Task.Run(() => EnqueueWriter());
            Task.Run(() => ReadersConsume(5));
            Task.Run(() => WritersConsume());
            Console.ReadLine();
        }

        static void ReadersConsume(int readerParallelismWidth)
        {
            while (true)
            {
                if (!ReadStopped)
                {
                    Task[] tasks = new Task[readerParallelismWidth];
                    for (int i = 0; i < tasks.Length; i++)
                    {
                        tasks[i] = Task.Run(() => {
                            Action action;
                            bool actionAvailable = ReaderActionQueue.TryTake(out action);
                            if (actionAvailable)
                            {
                                action();
                            }
                        });
                    }
                    TaskCollectionList.Add(tasks);

                    if (WriteReady)
                    {
                        foreach(var taskArray in TaskCollectionList)
                        {
                            Task.WaitAll(taskArray);
                        }
                        ReadStopped = true;
                    }
                }
            }
        }

        static void WritersConsume()
        {
            while (true)
            {
                Action action;
                if (WriterActionQueue.Count > 0)
                {
                    WriteReady = true;
                    if (ReadStopped)
                    {
                        if(WriterActionQueue.TryTake(out action))
                        {
                            action();
                        }
                        ReadStopped = false;
                        WriteReady = false;
                    }
                }
            }
        }

        static void EnqueueReader()
        {
            int j = 0;
            while (j < 1000)
            {
                ReaderActionQueue.Add(() => Console.WriteLine(Word.ToString()));
                j++;
            }
        }

        static void EnqueueWriter()
        {
            int j = 0;
            while (j < 1000)
            {
                WriterActionQueue.Add(() => { Word.Append('a'); Console.WriteLine("WRITE"); });
                j++;
            }
        }
    }
}
