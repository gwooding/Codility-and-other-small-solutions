Here is my solution to the famous Towers of Hanoi problem.

It’s a frustrating problem to solve from a complexity analysis perspective as it’s impossible to compute in under 2^n time (where n is the number of rings).  As I ran this on my 16Gb MacBook Pro with an i7 processor it took just under 2 minutes to compute 30 rings.  When you consider that this is over 1Bn ring shifts I was quite pleased with this time!

What was fun was implementing the recursion.  How I could have improved my program was to have a separate class for the three towers called, say, ThreeTowerCity.  I could then have passed around the city in the Move functions.  This would have made the Move function a pure function as it would have had no side effects on any fields.

I chose to use a Ring object rather than simply an int for the Rings generic stack out of habit, but I think it was also best practice.  Although in this case the Ring never had more to it than an int, in Agile development you might want the Ring to do more in the future.  It also made my code more readable and logical (e.g. line 48 of Program.cs reads like a sentence almost: “if we peek at the top of the rings we can see the width”).

Finally, I chose to use a static CreateRing method rather than a constructor for the Ring class.  This was to make my code testable, as it is much easier to mock a static method than a constructor.


