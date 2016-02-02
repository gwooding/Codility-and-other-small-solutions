import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

public class RouteFinder {
	
	private static ArrayList<ArrayList<Node>> routes = new ArrayList<ArrayList<Node>>();
	
	private static Node[] nodes;
	
	private static List<Integer> endNodeKeys;
	
	private static int[] paths;
	
	private static int[] results;
	
	public static void main(String[] args){
		
		int[] practice = new int[14];
		
		practice[0]=0;
		practice[1]=0;
		practice[2]=1;
		practice[3]=2;
		practice[6]=2;
		practice[9]=4;
		practice[4]=0;
		
		practice[5]=0;
		practice[7]=5;
		practice[8]=7;
		practice[10]=4;
		practice[11]=10;
		practice[12]=11;
		practice[13]=12;
		
		int starterTown = 4;
		
		int[] results = maxTownsPerDay(starterTown, practice);
		
		//Print results
		for(int i=0; i<results.length; i++){
			System.out.println(results[i]+", ");
		}
	}
	
	private static int[] maxTownsPerDay(int startingNodeNumber, int[] submittedPaths){
		
		paths = submittedPaths;
		
		nodes = createNodes(paths.length);
		
		findEndNodes(paths);
		
		buildRoutes(paths.length);
		
		//Goes to top of tree and ticks off the visited nodes on the way.
		goToHotel(startingNodeNumber);
		
		//Begins going from town to town, returning array of daily "target" town
		return visitedTowns();
	}
	
	private static Node[] createNodes(int numberOfNodes){
		
		Node[] newNodes = new Node[numberOfNodes];
		
		for(int i =0; i<numberOfNodes; i++){
			newNodes[i] = new Node(i, false);
		}
		return newNodes;
	}
	
	private static void findEndNodes(int[] paths){
		
		//Copy parameter to Integer array for endNodeKeys List
		Integer[] integerPaths = new Integer[paths.length];
		
		for(int i =0;i<paths.length; i++){
			integerPaths[i] = paths[i];
		}
		
		endNodeKeys = Arrays.asList(integerPaths);
		
		//endNodes cannot have a node pointing to them.
		//The value -1 signals non-endNode Keys
		for(int i =0; i<paths.length; i++){
			endNodeKeys.set(paths[i], -1);
		}
	}
	
	private static void buildRoutes(int numberOfNodes){
		
		int count = 0;
		
		//Add first node of each route (end node)
		for(int i =0; i<numberOfNodes; i++){
			if(endNodeKeys.get(i)!=-1){
				routes.add(count, new ArrayList<Node>());
				routes.get(count).add(0, nodes[i]);
				count++;
			}
		}
		
		//Build up rest of the routes (build up to root)
		for(int i=0; i<routes.size(); i++){
			ArrayList<Node> route = routes.get(i);
			boolean atRoot = false;
			int key = route.get(0).getKey();
			while(!atRoot){
				if(paths[key]!=key){
					route.add(nodes[paths[key]]);
					key=paths[key];
				} else {
					atRoot = true;
				}
			}	
		}
	}
	
	private static void goToHotel(int K){
		results = new int[routes.size()+1];	
		results[0] = K;
		
		if(paths[K]!=K){
			boolean atRoot=false;
			while(!atRoot){
				nodes[K].setVisited(true);
				atRoot = (K==paths[K]);
				K = paths[K];
			}
		}else{
			nodes[K].setVisited(true);
		}
	}
	
	private static int[] visitedTowns(){
		int days = 0;
		
		boolean gameOver = false;
		while(!gameOver){
			int maxRouteNumber = findMaxRoute();
			
			goDownMaxRoute(maxRouteNumber);
						
			days++;
			gameOver = true;
			
			//Gets end town of travelled route for results printing
			results[days] = routes.get(maxRouteNumber).get(0).getKey();
			
			//Ends game if all nodes are visited
			for(int i =0; i<nodes.length; i++){
				if(nodes[i].getVisited() == false) {
					gameOver = false; 
					break;
				}
			}			
		}
		return results;
	}
	
	private static int findMaxRoute(){
		
		int maxRoute = 0;
		int maxCityCounter = 0;
		
		//Goes through each route, and counts unvisited nodes from bottom
		for(int i=0; i<routes.size(); i++){
			ArrayList<Node> route = routes.get(i);
			int counter = 0;
			
			boolean arrivedAtVisitedNode = false;
			if(!route.get(0).getVisited()){
				int cityCounter = 0;
				while(arrivedAtVisitedNode == false){
					if(route.get(counter).getVisited()){
						arrivedAtVisitedNode = true;
					}else{
					counter++;
					cityCounter++;
					}
				}
				if(cityCounter>maxCityCounter){
					maxCityCounter = cityCounter;
					maxRoute = i;
				}
			}
		}
		return maxRoute;
	}
	
	private static void goDownMaxRoute(int maxRouteNumber){
		
		boolean arrivedAtVisitedNode = false;
		ArrayList<Node> routeToGoDown = routes.get(maxRouteNumber);
		int counter = routeToGoDown.size();
		
		while(counter>0&&arrivedAtVisitedNode == false){
			routeToGoDown.get(counter-1).setVisited(true);
			counter--;
		}		
	}
}

class Node{
	
	private int key;
	private boolean visited;
	
	Node(int key, boolean visited){
		this.key = key;
		this. visited = visited;
	}
	
	public int getKey(){
		return this.key;
	}
	
	public void setVisited(boolean visited){
		this.visited = visited;
	}
	
	public boolean getVisited(){
		return this.visited;
	}
}


