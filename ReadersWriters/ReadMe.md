This is my solution for the famous Readers-Writers concurrency problem.  

The problem, in a nutshell, is to have a resource, which in my case is a stringbuilder called word, that gets shared by two types of threads, readers and writers.

Writers cannot change the resource while it is being read, and other writers cannot change the resource while it is being written to.  It is acceptable for multiple readers to share the resource at the same time, however.

The question is, how to write an algorithm that will not starve readers or writers, and will solve the requirements above?

In my program, the reader action prints the word to the screen, whereas the writer action adds the letter "a" to the shared word and prints "WRITE" to the console, so it is obvious that a write had occurred.

The reader and writer actions were each stored in a BlockingCollection ConcurrentQueue.  These were added to their respective queues as it was being consumed.  

My algorithm uses two-way signalling between two methods (ReadersConsume and WritersConsume) running infinite while-loops.  The first signal is called readStopped.  If this is false then a fixed number of tasks are fired up in ReadersConsume, each of which takes an action from the reader queue and reads the resource, if there are any actions in the readers queue.

These tasks are then added, as an array, whilst still running, to a list of Task arrays, called taskCollectionList. 

The code checks if the writeReady signal is true.  If it is, then the code waits for  all tasks in taskCollectionList to stop.  Then the readStopped flag is set to true.

But what has caused the writeReady signal to change?  While all this has been happening in the ReadersConsume method, the WritersConsume method, assuming there are any writing actions in the writers queue, sets WriteReady to true.  It loops until ReadStopped is set to false, which is an inevitable result of setting WriteReady to true. 

At this point, one action is popped off the writer’s queue and is completed.  Then both signals are set to false.  At this point, control is passed back to the readersConsume method which fires off another x tasks to read the new resource.

I was quite pleased with my algorithm.  One of the key parts of my solution is that it uses a producer/consumer solution for orchestrating the reader and writer requests as they arrive.   Obviously my solution doesn’t end as there are two infinite while loops, but what is important is the algorithm for sharing the resource when requests to read it and write to it come in, which is also a problem that can go on forever.  I also haven't really made use of OO design principles for this demonstration of my algorithm, as it is one class with all static members. 
