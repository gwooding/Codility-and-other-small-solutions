The question was to write an algorithm that counted the usage of each word in a string into a data structure, and then use that data structure
to find counts for various words.

I immediately decided not to use any data structure that required a fixed amount of memory at the outset, as the number of different words would be undetectable without reading the entire string.

The data structure also didn't need to be efficient for deletion as this operation would not be used, but did need to be very time (and space) efficient for insertion and reading.

My impulse was to use a HashSet, as this would have O(n) look-up time (worst-case), if I used the words as keys and the count as values.

However instead I used a tree of SortedSets.  I believed that the look-up time and insertion time would be constant (O(1)) in the worst-case.  Also if I used
each node to represent a letter, and words read down the tree starting from just below the root, then words that started with other words would share memory space. 

Each node in the tree represented a letter, and had a count.  If a node was the last letter of a word then it's count reflected how often it was used in the book.

The average height of the tree would be the average length of a word in the book.  The time to find the next node for a given node is also going to be, at worst, log(26) (as SortedSets themselves use a binary tree structure), therefore time to look up and insert items was constant.

The tricky part was considering all the edge cases for what defines a word.  In the end I decided that a word is any set of characters that had a space, start/end-of-file, "/", "\" or "_"
at each end.  I also assumed no typos or symbol slang, e.g."Sa$ha" , which meant I ignored these symbols: "',:;@.!/£$&+=.

It was great experience playing about with the Linq syntax and using SortedSets for the first time.  I will be sure to do more C# code in the future.




