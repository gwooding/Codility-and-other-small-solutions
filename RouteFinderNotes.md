What this algorithm does is, given a starting node in a non-binary tree of "towns", it first goes to the root (the hotel), visiting towns along the way.  

From here, the aim is to visit the maximum number of towns each day without crossing your own path on the same day (you stay the night in the town you end up in).

In my example I've included a starting tree, but you can include any tree, and just pass the array and the starting number into the first private method, maxTownsPerDay.

This was originally an attempt to do Fluorum 2014 on Codility.  For some reason I assumed you would go through the root on each move, which isn't necessarily true.

If I could do this again I would recreate the tree so that the node you began on was the root (rather than the node that points to itself), but I want to do a new problem now. 

The time complexity of my algorithm is O(nlogn).
