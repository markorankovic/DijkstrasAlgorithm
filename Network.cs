using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithm
{

    class Network
    {
        List<Node> nodes;
        //List<uint[]> indexesOfLinkedNodes;
        public Network()
        {
            nodes = new List<Node>();
        }
        public void connectNodes(uint iN1, uint iN2, uint weight)
        {
            this.nodes.ElementAt((int) iN1).linkToNode(this.nodes.ElementAt((int) iN2), weight);
            //indexesOfLinkedNodes.Add(new uint[2] { iN1, iN2 });
        }
        public void createNodes(int count)
        {
            for (int i = 0; i < count; i++)
            {
                this.nodes.Add(new Node());
            }
        }
        public void removeNodeAt(uint index)
        {
            this.nodes.RemoveAt((int) index);
        }
        public List<Node> getNodes()
        {
            return this.nodes;
        }


        Node getNodeWithSmallestDistance()
        {
            int index = 0;
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                if (nodes.ElementAt(i).getDistance() > nodes.ElementAt(i + 1).getDistance())
                {
                    index = i + 1;
                }
            }
            return nodes.ElementAt(index);
        }


        List<Node> cloneListOfNodes()
        {
            List<Node> newListOfNodes = new List<Node>();
            foreach (Node node in nodes)
            {
                newListOfNodes.Add(node);
            }
            return newListOfNodes;
        }


        void updateDistance(Node adjacentNode, uint currentAdjacentNodeDistance, uint adjacentNodeDistanceThroughNode)
        {
            if (adjacentNodeDistanceThroughNode < currentAdjacentNodeDistance)
            {
                adjacentNode.setDistance(adjacentNodeDistanceThroughNode);
            }
        }

        void setDistancesToAdjacentNodes(Node node)
        {
            List<Link> nodeLinks = node.getLinks();

            for (int c = 0; c < nodeLinks.Count; c++)
            {
                Node adjacentNode = nodeLinks.ElementAt(c).getNode();
                uint currentAdjacentNodeDistance = adjacentNode.getDistance();
                uint adjacentNodeDistanceThroughNode = node.getDistance() + nodeLinks.ElementAt(c).getWeight();
                updateDistance(adjacentNode, currentAdjacentNodeDistance, adjacentNodeDistanceThroughNode);
            }
        }


        void updateDistanceForEachNode(List<Node> unVisitedNodes)
        {
            for (int i = 0; i < unVisitedNodes.Count; i++)
            {
                Node nextNode = getNodeWithSmallestDistance();
                setDistancesToAdjacentNodes(nextNode);
                nodes = nodes.Where((node) => !node.Equals(nextNode)).ToList();
            }
        }


        List<Node> getNodesWithCalculatedDistances(uint sourceIndex)
        {
            nodes.ElementAt((int)sourceIndex).setDistance(0);
            List<Node> unVisitedNodes = cloneListOfNodes();
            updateDistanceForEachNode(unVisitedNodes);
            return unVisitedNodes;
        }

        uint getShortestDistanceTo(uint destinationIndex, uint sourceIndex)
        {
            nodes = getNodesWithCalculatedDistances(sourceIndex);
            Node destination = nodes.ElementAt((int)destinationIndex);
            return destination.getDistance();
        }

        List<Node> getAdjacentNodes(Node node)
        {
            List<Node> adjacentNodes = new List<Node>();
            foreach (Link link in node.getLinks())
            {
                adjacentNodes.Add(link.getNode());
            }
            return adjacentNodes;
        }


        Node getNextNodeInPath(Node node)
        {
            Node nextNode = null;
            foreach (Link link in node.getLinks())
            {
                if (link.getNode().getDistance() == node.getDistance() - link.getWeight())
                {
                    nextNode = link.getNode();
                }
            }
            return nextNode;
        }

        List<Node> getPath(Node destination, List<Node> path)
        {
            if (destination.getDistance() == 0) { return path; }
            Node nextNode = getNextNodeInPath(destination);
            path.Insert(0, nextNode);
            return getPath(nextNode, path);
        }

        public List<Node> getPath(uint destinationIndex, uint sourceIndex)
        {
            Node destination = this.nodes.ElementAt((int)destinationIndex);
            this.nodes = getNodesWithCalculatedDistances(sourceIndex);
            return getPath(destination, new List<Node>() { destination });
        }

        public void displayShortestPath(List<Node> path)
        {
            for (int i = 0; i < path.Count; i++)
            {
                Console.Write(path.ElementAt(i) + (i < path.Count - 1 ? " -> " : ""));
            }
            Console.WriteLine();
        }

        List<int> getPossibleNumbers(string str)
        {
            string[] subStrings = str.Split();
            List<int> possibleNumbers = new List<int>();
            int num;
            foreach (string s in subStrings)
            {
                if (int.TryParse(s, out num))
                {
                    possibleNumbers.Add(num);
                }
            }
            return possibleNumbers;
        }

        bool possibleNumbersInvalid(List<int> possibleNumbers)
        {
            for (int i = 0; i < 2; i++)
            {
                if (possibleNumbers.ElementAt(i) < 0 || possibleNumbers.ElementAt(i) > nodes.Count-1)
                {
                    return true;
                }
            }
            return false;
        }

        public void openCommandLine()
        {
            string input = Console.ReadLine();
            string[] str = input.Split();
            List<int> possibleNumbers = this.getPossibleNumbers(input);

            if (possibleNumbers.Count == 0)
            {
                switch (input)
                {
                    case "exit": return;
                    default: Console.WriteLine("Error: Invalid command \n"); openCommandLine(); return;
                }
            }

            if (input != string.Format("create new nodes {0}", possibleNumbers.ElementAt(0)) && possibleNumbersInvalid(possibleNumbers))
            {
                Console.WriteLine("Error: Invalid input \n");
                openCommandLine();
                return;
            }

            switch (possibleNumbers.Count)
            {
                case 1:
                    bool createNewNodes = input == string.Format("create new nodes {0}", possibleNumbers.ElementAt(0));
                    if (createNewNodes) { this.createNodes(possibleNumbers.ElementAt(0)); openCommandLine(); return; };
                    break;
                case 2:
                    bool displayShortestPath = input == string.Format("display shortest path {0} {1}", possibleNumbers.ElementAt(0), possibleNumbers.ElementAt(1));
                    if (displayShortestPath) { this.displayShortestPath(this.getPath((uint)possibleNumbers.ElementAt(0), (uint)possibleNumbers.ElementAt(1))); openCommandLine(); return; };
                    break;
                case 3:
                    bool connectNodes = input == string.Format("connect nodes {0} {1} {2}", possibleNumbers.ElementAt(0), possibleNumbers.ElementAt(1), possibleNumbers.ElementAt(2));
                    if (connectNodes) { this.nodes.ElementAt(possibleNumbers.ElementAt(0)).linkToNode(this.nodes.ElementAt(possibleNumbers.ElementAt(1)), (uint)possibleNumbers.ElementAt(2)); openCommandLine(); return; };
                    break;
            }

        }

        override public string ToString()
        {
            return "";
        }


    }

}
