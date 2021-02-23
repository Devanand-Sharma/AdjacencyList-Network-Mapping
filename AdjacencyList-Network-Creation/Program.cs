//Dave Sharma
//2021
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdjacencyList_Network_Creation
{

    public enum Colour { RED, YELLOW, GREEN, PURPLE }

    class Node
    {
        public Station Connection { get; set; }
        public Colour Line { get; set; }
        public Node Next { get; set; }


        public Node(Station connection, Colour c, Node next)
        {
            Connection = connection;
            Line = c;
            Next = next;
        }

    }

    class Station
    {
        public string Name { get; set; }
        public bool Visited { get; set; }
        public Node E { get; set; }
        public Station(string name)
        {
            Name = name;
        }
    }

    class SubwayMap
    {
        private List<Station> S;
        public SubwayMap()
        {
            // create an empty list
            S = new List<Station>();
            Console.WriteLine("Subway Map creation Started.\n");
        }

        //Given a station name return its index in the list of stations S. If the name is not found then return -1.
        public int FindStation(string name)
        {
            // iterate over the list of stations in S
            for (int i = 0; i < S.Count; i++)
            {
                if (S[i].Name.Equals(name))
                {
                    return i;
                }
            }
            return -1;
        }

        //Given a station name, check if it is already in the list. If yes, do nothing. Otherwise create a Station object and add it to the list
        public void InsertStation(string name)
        {
            if (FindStation(name) == -1)
            {
                Station station = new Station(name);
                S.Add(station);
                Console.WriteLine("Station {0} was inserted successfully into the map.", name);
            }
            else
            {
                Console.WriteLine("Station {0} already exists.", name);
            }
        }

        // Given a station name, check if it is in the list. If yes remove station. Otherwise, do nothing
        public void RemoveStation(string name)
        {
            int z;
            if ((z = FindStation(name)) == -1)
            {
                Console.WriteLine("Station {0} does not exist.", name);
            }
            int i, j, k;
            //if name exists in list
            if ((i = FindStation(name)) > -1)
            {
                //Remove all incident edges to the given station name
                //For every station in the the list
                for (j = 0; j < S.Count; j++)
                {
                    Node previousNode = null;
                    Node currentNode = S[j].E;
                    //Loop as long as the currentNode is not null
                    while (currentNode != null)
                    {
                        //check if current node's connection name is same as the name given to the function
                        if (currentNode.Connection.Name.Equals(name))
                        {
                            if (previousNode == null)
                            {
                                S[j].E = currentNode.Next;
                            }
                            else
                            {
                                previousNode.Next = currentNode.Next;
                            }
                        }
                        else
                        {
                            //update previous node to current node
                            previousNode = currentNode;
                        }
                        //update currentNode to next node
                        currentNode = currentNode.Next;
                    }
                }
                Console.WriteLine("Station {0} removed successfully.", name);
                S.RemoveAt(i);

            }
        }

        public void InsertConnection(string name1, string name2, Colour c)
        {
            //If stations are the same
            if (name1.Equals(name2))
            {
                Console.WriteLine("From and To stations are the same. Can not insert connection.");
                return;
            }
            //if stations are found, add connection.
            int i, j;
            if ((i = FindStation(name1)) > -1 && (j = FindStation(name2)) > -1)
            {
                int sourceStationIndex = j;
                Node newConnection = new Node(S[i], c, null);
                AppendConnection(sourceStationIndex, newConnection);

                sourceStationIndex = i;
                newConnection = new Node(S[j], c, null);
                AppendConnection(sourceStationIndex, newConnection);
                Console.WriteLine("Connection {0} to {1} with colour {2} added successfully.", name1, name2, c);
            }
            else
            {
                Console.WriteLine("Station {0} or Station {1} do not exist.", name1, name2);
            }
        }

        //private helper function 
        private void AppendConnection(int sourceStationIndex, Node newConnection)
        {
            Node previousNode = null;
            Node currentNode = S[sourceStationIndex].E;
            //new edge to be added to the linked list
            //Traverse the linked list until the end 
            while (currentNode != null)
            {
                //Same edge so the new edge is not unique
                if (currentNode.Connection.Name.Equals(newConnection.Connection.Name) && currentNode.Line.Equals(newConnection.Line))
                {
                    return;
                }
                previousNode = currentNode;
                currentNode = currentNode.Next;

            }
            // add the new node to the end
            if (previousNode == null)
            {
                S[sourceStationIndex].E = newConnection;
            }
            else
            {
                previousNode.Next = newConnection;
            }
        }


        public void RemoveConnection(string name1, string name2, Colour c)
        {
            if (name1.Equals(name2))
            {
                Console.WriteLine("From and To stations are the same.");
                return;
            }

            int i, j;
            if ((i = FindStation(name1)) > -1 && (j = FindStation(name2)) > -1)
            {
                //select one of the indices as the sourceStationIndex
                int sourceStationIndex = i;
                string destinationStationName = name2;
                RemoveConnectionFromStation(sourceStationIndex, destinationStationName, c);

                //select one of the indices as the sourceStationIndex
                sourceStationIndex = j;
                destinationStationName = name1;
                RemoveConnectionFromStation(sourceStationIndex, destinationStationName, c);
                Console.WriteLine("Connection from {0} to {1} with colour {2} removed successfully.", name1, name2, c);

            }
            else
            {
                Console.WriteLine("Station {0} or Station {1} does not exist. Connection not added.", name1, name2);
            }
        }


        private void RemoveConnectionFromStation(int sourceStationIndex, string destinationStationName, Colour c)
        {
            //create currentNode and previousNode
            Node previousNode = null;
            Node currentNode = S[sourceStationIndex].E;
            //start while loop
            while (currentNode != null)
            {
                if (currentNode.Connection.Name.Equals(destinationStationName) && currentNode.Line.Equals(c))
                {
                    break;
                }
                //update  previousNode and currentNode
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }
            //check if currentNode is not null 
            if (currentNode != null)
            {
                //check if previousNode is null
                if (previousNode == null)
                {
                    S[sourceStationIndex].E = currentNode.Next;
                }
                else
                {
                    //else
                    previousNode.Next = currentNode.Next;
                }

            }
        }
        public void FastestRoute(string from, string to)
        {
            int y, z;
            if ((y = FindStation(from)) == -1 || (z = FindStation(to)) == -1)
            {
                Console.WriteLine("From or To station doesn't exist.");
            }
            //for each station set visited to false
            for (int k = 0; k < S.Count; k++)
                S[k].Visited = false;              // Set all vertices as unvisited

            //check if from and to station names exist
            int i, j;
            if ((i = FindStation(from)) > -1 && (j = FindStation(to)) > -1)
            {
                //check if from and to are Equal
                if (i == j)
                {
                    Console.WriteLine("From and To stations are the same");
                    return;
                }

                //performs BFS
                //create a variable and assign the station for from name
                Station fromStation = S[i];
                //create a variable and assign the station for to name
                Station toStation = S[j];

                Dictionary<string, string> stationPathLookup = BreadthFirstSearch(fromStation, toStation);

                //check if stationPathLookup is null. if yes, print no path
                if (stationPathLookup == null)
                {
                    Console.WriteLine("There is no path from station {0} to station {1}.", from, to);
                }
                else
                {
                    //build path from stationPathLookup

                    //create an empty list called stationPath. the type of the elements in the list is string
                    List<string> stationPath = new List<string>();

                    //add the "to"  name to the list
                    stationPath.Add(to);
                    string currentStationName = to;
                    //loop until the currentStationName is not equal to the "from"
                    while (currentStationName != from)
                    {
                        //lookup the value associated with the currentStationName
                        currentStationName = stationPathLookup[currentStationName];
                        //add the currentStationName to the stationPath list
                        stationPath.Add(currentStationName);
                    }

                    //we are done building the stationPath. Reverse it
                    stationPath.Reverse();
                    Colour currentColour;
                    //print the list contents 
                    Console.WriteLine("Fastest Route:");
                    for (int k = 0; k < stationPath.Count; k++)
                    {
                        if (k == 0)
                        {
                            Console.Write(stationPath[k]);
                        }
                        else
                        {

                            Console.Write(" -> {0}", stationPath[k]);
                        }
                    }
                    Console.WriteLine();

                }


            }
        }


        //implement to include transfers
        private Dictionary<string, string> BreadthFirstSearch(Station fromStation, Station toStation)
        {
            int j;
            Station w;
            Queue<Station> Q = new Queue<Station>();
            Dictionary<string, string> previousStationLookUp = new Dictionary<string, string>();

            fromStation.Visited = true;        // Mark vertex as visited when placed in the queue.
            Q.Enqueue(fromStation);            // Add station to the queue. 
            Station currentStation;
            while (Q.Count != 0)
            {
                currentStation = Q.Dequeue();     // Output vertex when removed from the queue

                //create currentNode and assign it to the currentStation's E 
                Node currentNode = currentStation.E;
                Station adjacentStation;
                //loop till currentNode is null
                while (currentNode != null)
                {

                    //set adjacentStation to the station stored in the currentNode
                    adjacentStation = currentNode.Connection;

                    //check if adjacentStation is same as toStation
                    if (adjacentStation.Name == toStation.Name)
                    {
                        //found the end station so we are done with BFS. return
                        previousStationLookUp.Add(adjacentStation.Name, currentStation.Name);
                        return previousStationLookUp;
                    }

                    //check if adjacentStation is not visited
                    if (!(adjacentStation.Visited))
                    {
                        //create an entry with key as adjacentStation's name and value as the currentStation's name
                        previousStationLookUp.Add(adjacentStation.Name, currentStation.Name);
                        //set the adjacentStation's vistied to true
                        adjacentStation.Visited = true;
                        //put adjacentStation to the queue
                        Q.Enqueue(adjacentStation);
                    }
                    //update currentNode
                    currentNode = currentNode.Next;
                }
            }
            //if control reaches here then no path to toStation is found
            return null;
        }

        public void CriticalConnections()
        {

            // Step 1 : One by one remove an edge from the graph.
            // Step 2 : When an edge is removed from the graph, perform traversing[DFS] on the graph and 
            // see if all the vertexes are visited. If any vertex remains unvisited, then the edge causing the problem is 
            // nothing but a critical connection.

            HashSet<string> processedStationNames = new HashSet<string>();
            bool hasCriticalConnections = false;

            //loop through all stations in the list
            Station currentStation;
            for (int k = 0; k < S.Count; k++)
            {
                //set currentStation to the station at index k
                currentStation = S[k];
                //add currentStation's name to the processedStationNames set
                processedStationNames.Add(currentStation.Name);

                //make a copy of edges
                List<Node> edges = new List<Node>();
                Node currentNode = currentStation.E;
                while (currentNode != null)
                {
                    edges.Add(currentNode);
                    currentNode = currentNode.Next;
                }

                foreach (Node stationEdge in edges)
                {
                    //before we remove edge, check if stationEdge.Connection is already processed
                    if (!processedStationNames.Contains(stationEdge.Connection.Name))
                    {
                        //remove current edge
                        RemoveConnection(currentStation.Name, stationEdge.Connection.Name, stationEdge.Line);

                        //run dfs
                        int i;
                        //for each station in the list, set its visited to false
                        for (i = 0; i < S.Count; i++)     // Set all vertices as unvisited
                            S[i].Visited = false;

                        //run dfs from the currentStation
                        DepthFirstSearch(currentStation);

                        //after DFS if any station is unvisited then the stationEdge is a criticalConnection
                        for (i = 0; i < S.Count; i++)
                        {
                            if (!S[i].Visited)
                            {
                                if (!hasCriticalConnections)
                                {
                                    hasCriticalConnections = true;
                                    Console.WriteLine("Critical Connections are:");
                                }

                                Console.WriteLine("\t{0} -> {1} Colour: {2}", currentStation.Name, stationEdge.Connection.Name, stationEdge.Line);
                                break;
                            }
                        }

                        //insert current edge
                        InsertConnection(currentStation.Name, stationEdge.Connection.Name, stationEdge.Line);

                    }

                    //Console.WriteLine("{0} -> {1} Colour: {2}", currentStation.Name, stationEdge.Connection.Name, stationEdge.Line);
                }

            }
            if (!hasCriticalConnections)
            {
                Console.WriteLine("No critical connections");
            }





        }


        private void DepthFirstSearch(Station currentStation)
        {
            int j, k;
            Station w;

            currentStation.Visited = true;    // Output vertex when marked as visited
            //Console.WriteLine(v.Name);

            //loop through edges of currentStation
            Node stationConnection = currentStation.E;
            while (stationConnection != null)
            {
                //get the coonecting station and assign it to w
                w = stationConnection.Connection;
                if (!w.Visited)
                    DepthFirstSearch(w);
                //update stationConnection
                stationConnection = stationConnection.Next;
            }
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            //create an object of type SubwayMap
            SubwayMap subway = new SubwayMap();
            //add stations w, x, y, z
            string[] stationNames = { "a", "w", "x", "y", "z" };

            int stationIndex;
            foreach (string name in stationNames)
            {
                subway.InsertStation(name);
                stationIndex = subway.FindStation(name);
                //if (stationIndex == -1)
                //{
                //    Console.WriteLine("{0} was not inserted to the subway map", name);
                //}
                //else
                //{
                //    Console.WriteLine("{0} was successfully inserted to the subway map", name);
                //}
            }

            //remove station "a"
            subway.RemoveStation("a");
            //stationIndex = subway.FindStation("a");
            //if (stationIndex == -1)
            //{
            //    Console.WriteLine("Station a was successfully removed from the subway map");
            //}
            //else
            //{
            //    Console.WriteLine("Station a was not removed from the subway map");
            //}

            //add connections
            subway.InsertConnection("w", "x", Colour.RED);
            subway.InsertConnection("x", "y", Colour.GREEN);
            subway.InsertConnection("w", "y", Colour.RED);
            subway.InsertConnection("y", "z", Colour.PURPLE);
            //subway.InsertConnection("x", "z", Colour.PURPLE);

            //remove connection w to z
            subway.RemoveConnection("w", "b", Colour.PURPLE);

            //fastest path between "w" to "z"
            //subway.FastestRoute("w", "z");

            //Critical Connections
            subway.CriticalConnections();

            Console.ReadLine();
        }
    }
}
