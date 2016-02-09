import java.util.ArrayList;
import java.util.Arrays;

public class EpsilonSolutionForCodility {

	public static void main(String[] args) {
		
		//int[] A = {-1, 1};
		//int[] B = {3, 1};
		
		//int[] A = {2, 0, -1, -1, 0, 2, 2};
		//int[] B = {3, -1, 4, -2, 1, 5, 4};
		
		//int[] A = {2, 0, 4};
		//int[] B = {3, 1, 2};
		
		int[] A = {-1, 1, 0};
		int[] B = {3, 0, 2};
		
		double minimumDifference = solve(A, B);
		
		System.out.println(minimumDifference);
	}
	
	private static double solve(int[] A, int[] B) {
	        int[][] straightLines = new int[A.length][2];
	        
	        for(int i =0;i<A.length;i++){
	        	straightLines[i][0] = A[i];
	        	straightLines[i][1] = B[i];
	        }
	        
	        /*Straight lines are sorted by increasing gradient 
	        or decreasing y-intercept if they are parallel*/
	        
	        java.util.Arrays.sort(straightLines, new java.util.Comparator<int[]>(){
	        	public int compare(int[] a, int[] b){
	        		
	        		if(Integer.compare(a[0], b[0])==0){
	        			return -Integer.compare(a[1], b[1]);
	        		}
	        		
	        		return Integer.compare(a[0], b[0]);
	        	}
	        });
	        
	        ArrayList<LineChainElement> maxRoute= GetRoute(straightLines); 
	        
	        int[][] straightLinesReversed = new int[straightLines.length][2];
			
			for(int i = 0; i<straightLines.length; i++){
				straightLinesReversed[straightLines.length-1-i] = straightLines[i];
			}
	        
	        ArrayList<LineChainElement> minRoute = GetRoute(straightLinesReversed);
	        
	        return getMinRecursively(0, 0, Double.MIN_VALUE, Double.MAX_VALUE, maxRoute, minRoute);
	 }
	
	private static ArrayList<LineChainElement> GetRoute(int straightLines[][]){
		
		ArrayList<LineChainElement> route = new ArrayList<LineChainElement>();
		
        boolean notAtEnd = true;
            
        int nextLineOnRoute = 0;
        
        int counter = 0;
        
        int remainingSteeperOrParallelLines = straightLines.length-1;
        
        //Iterates on counter variable through LineChainElements on maximal route 
        //The line with lowest gradient and highest y-intercept (if gradient not unique)
        //is going to be the first on the route.
        
        outer: while(notAtEnd){
        	
        	double firstIntersection= Double.MIN_VALUE;
        	
        	boolean allParallel =true; 
        	
        	//Iterates through other lines with the same or steeper gradient
        	inner: for(int i = 1; i<=remainingSteeperOrParallelLines; i++){
        		
        		double nextIntersection;
        		
        		if(straightLines[counter][0]==straightLines[counter+i][0]){
        			
        			//Ignores if parallel
        			continue inner;
        			
        		} else{
        			
        			allParallel = false;
        			nextIntersection = findIntersection(straightLines[counter], straightLines[counter+i]);	
        			
        		}
        		
        		if(firstIntersection == Double.MIN_VALUE){
        			
        			firstIntersection =  nextIntersection;
        			nextLineOnRoute = counter+i;
        			
        		}
        		
        		else if(nextIntersection<=firstIntersection){
        			
        			firstIntersection = nextIntersection;
        			nextLineOnRoute = counter+i;
        			
        		}
        	}
        	
        	if(allParallel){
        		break outer;
        		
        	}
        	//The line that intersects route first has been found, and will form the basis 
        	//for finding the next part of the route
        	
        	route.add(new LineChainElement(straightLines[counter], firstIntersection));
        	counter = nextLineOnRoute;
        	remainingSteeperOrParallelLines = straightLines.length - counter - 1;
        	
        	boolean areAllOtherstraightLinesParallel = areRemainingLinesParallel(
        			remainingSteeperOrParallelLines, straightLines, counter);
        	
        	if(counter>=straightLines.length-1||areAllOtherstraightLinesParallel){
        		route.add(new LineChainElement(straightLines[nextLineOnRoute]));
        		notAtEnd = false;
        	}
        }
        
        return route;	
	}
	
	private static boolean areRemainingLinesParallel(int remainingLines, int[][]straightLines, int counter){
		
		boolean allParallel = true;
		
		for (int i=1; i<=remainingLines; i++){
			if(straightLines[counter][0]!=straightLines[counter+i][0]){
				allParallel = false;
			}
		}
		
		return allParallel;
	}
	
	private static double getMinRecursively(
		int maxRouteLine, int minRouteLine, double Start, double minimumDistanceBetweenRoutes, 
		ArrayList<LineChainElement> maxRoute, ArrayList<LineChainElement> minRoute){
			
		double nextMaxRouteIntersection = maxRoute.get(maxRouteLine).
				getNextIntersection()==null?Double.MAX_VALUE:maxRoute.get(maxRouteLine).getNextIntersection();
		
		double nextMinRouteIntersection = minRoute.get(minRouteLine).
				getNextIntersection()==null?Double.MAX_VALUE:minRoute.get(minRouteLine).getNextIntersection();
		
		int netGradient = maxRoute.get(maxRouteLine).getLine()[0]-minRoute.get(minRouteLine).getLine()[0];
		
		int netCValue = maxRoute.get(maxRouteLine).getLine()[1]-minRoute.get(minRouteLine).getLine()[1];
		
		double End =  Double.MIN_VALUE;
		
		if(nextMaxRouteIntersection>nextMinRouteIntersection){
			End = nextMinRouteIntersection;
			minRouteLine +=1;
		}
		
		if(nextMaxRouteIntersection<nextMinRouteIntersection){
			End = nextMaxRouteIntersection;
			maxRouteLine += 1;
		}
		
		if(nextMaxRouteIntersection==nextMinRouteIntersection){
			End = nextMaxRouteIntersection;
			maxRouteLine += 1;
			minRouteLine += 1;
		}
		
		double thisMin = Double.MIN_VALUE;
		
		if(netGradient < 0 ){
			thisMin = netGradient*End + netCValue;	
		}
		
		if(netGradient >= 0 ){
			thisMin = netGradient*Start + netCValue;	
		}
		
		if(thisMin<minimumDistanceBetweenRoutes){
			minimumDistanceBetweenRoutes = thisMin;
		}
		
		Start = End;
		
		if(maxRouteLine<maxRoute.size()-1||minRouteLine<minRoute.size()-1){
			minimumDistanceBetweenRoutes = getMinRecursively(maxRouteLine, minRouteLine, Start, minimumDistanceBetweenRoutes, maxRoute, minRoute);
		} else {
			//Recursion stops:
			if(netGradient*End + netCValue<minimumDistanceBetweenRoutes){
		
			minimumDistanceBetweenRoutes = netGradient*End + netCValue;
			}
		}
		return minimumDistanceBetweenRoutes;
	}
		
	private static double findIntersection (int[] line1, int[] line2){
		
		double answer = 0;
		
		if(line2[0]- line1[0]!=0){
		
		answer  = (double)(line2[1]- line1[1])/(double)(line1[0]- line2[0]);
		
		}
		return answer;
	}
}

class LineChainElement{
	
	private int[] line;
	private Double nextIntersection;
	
	LineChainElement(int[] line, Double nextIntersection){
		this.line = line;
		this.nextIntersection = nextIntersection;
	}	
	
	LineChainElement(int[] line){
		this.line = line;
	}
	
	int[] getLine(){
		return this.line;
	}
	
	Double getNextIntersection(){
		return this.nextIntersection;
	}
}
