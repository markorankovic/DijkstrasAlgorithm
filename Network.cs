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
        public Network()
        {
            nodes = new List<Node>();
        }
        public void connectNodes(uint iN1, uint iN2, uint weight)
        {
            this.nodes.ElementAt((int) iN1).linkToNode(this.nodes.ElementAt((int) iN2), weight);
        }
        public void addNode(Node node)
        {
            this.nodes.Add(node);
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
            foreach (Link link in node.getLinks())
            {
                if (link.getNode().getDistance() == node.getDistance() - link.getWeight())
                {
                    return link.getNode();
                }
            }
            return node; // Will cause infinite loop if it reaches here
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


    }

    class Class1
    {
        public static void Main(string[] args)
        {

            Network network = new Network();

            for (int i = 0; i < 10; i++)
            {
                network.addNode(new Node());
            }


            List<Node> nodes = network.getNodes();
            network.connectNodes(0, 1, 5);
            network.connectNodes(2, 0, 3);
            network.connectNodes(4, 2, 7);
            network.connectNodes(3, 2, 6);
            network.connectNodes(1, 4, 4);
            network.connectNodes(9, 4, 3);
            network.connectNodes(7, 9, 6);
            network.connectNodes(8, 3, 5);
            network.connectNodes(6, 7, 3);
            network.connectNodes(5, 4, 7);
            network.connectNodes(5, 9, 3);



            List<Node> path = network.getPath(9, 0);

            for (int i = 0; i < path.Count; i++)
            {
                Console.Write(path.ElementAt(i) + (i < path.Count - 1 ?  " -> " : ""));
            }
            Console.WriteLine();



        }
    }
}
