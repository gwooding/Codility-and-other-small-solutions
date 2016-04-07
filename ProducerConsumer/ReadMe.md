Here is my implementation of a ProducerConsumer queue, which I learned how to do for some new multithreading problems being presented at my work.

This is the product of a lot of reading and practice around threading in C# that I've done.  There were many new concepts that I learned, such as the importance of having a cancellation token to make all the tasks running stop with one call to the token's cancel method.

There were many new design concepts I applied to this project that I hadn't thought of before, such as passing in the logger as an action so that the user of the queue can decide whether they'd like to output log messages using Trace or Log4Net libraries for an example, or indeed the console.

This was also my first open source project that used the IDisposable interface, which is very important for clearing objects out of memory in a large system that cannot afford to wait for garbage collection.

There will be much more multithreading code to come!
