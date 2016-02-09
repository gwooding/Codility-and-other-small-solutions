Here is my solution to Epsilon, a challenge from Codility in 2011.  As I discovered Codility late, I did it in 2016.

The challenge:

Two non-empty zero-indexed arrays A and B, each consisting of N integers, are given. Four functions are defined based on these arrays:

F(X,K)	=	A[K]*X + B[K]
U(X)	=	max{ F(X,K) : 0 ≤ K < N }
D(X)	=	min{ F(X,K) : 0 ≤ K < N }
S(X)	=	U(X) − D(X)
Write a function:

double solution(int A[], int B[], int N);

that, given two arrays A and B consisting of N integers each, returns the minimum value of S(X) where X can be any real number.

I immediately started thinking about using objects representing straight lines in my solution, as well as their intersections.

What my final solution does is finds the maximum and minimum "routes" separately (by route I mean the order of lines you take from 
left to right, and the x-values of their intersections).  If you then cut this pair of lines into horizontal slices, where each slice is 
on an intersection for the maximal or minimal route, then the minimum "gap" at each slice will be at one of the two slice borders.
This is because both lines will be straight and unkinked within each slice.

I could only find the minimal and maximal routes in O(N^2) time, which means my solution is O(N^2).  Although all lines were ordered by 
gradient (which Java's array library can sort in O(nlogn) time) to find the next part of each route you had to check the intersection of 
every steeper curve.

One of the difficulties I had was with parallel lines.  I got around this by sorting the lines by gradient, and THEN by REVERSE y-intercept.
Then my code would always use the curve with the highest intercept (for finding the maximum route) and vice versa.

It's an old challenge so I am looking for solutions that have O(n^2), but I'm pleased with my solution. 

I've included some examples in my code that are hardcoded into the main method, but feel free to fire in more weird datasets.  
My code doesn't work if every line is parallel, or you only use one line, as I considered these cases too trivial.
