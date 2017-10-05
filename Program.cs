using System;
using System.Collections.Generic;
using System.Linq;

namespace DijkstrasAlgorithm
{


    class Node
    {
        uint distanceToSource = uint.MaxValue;
        List<Link> links;
        public Node()
        {
            links = new List<Link>(1);
        }
        public List<Link> getLinks()
        {
            return this.links;
        }
        public void linkToNode(Node node, uint weight)
        {
            links.Add(new Link(node, weight));
            node.links.Add(new Link(this, weight));
        }
        public uint getDistance()
        {
            return this.distanceToSource;
        }
        public void setDistance(uint newDistance)
        {
            this.distanceToSource = newDistance;
        }
        override public string ToString()
        {
            return "" + this.distanceToSource;
        }
    }

    class Link
    {
        Node node;
        uint weight;
        public Link(Node node, uint weight)
        {
            this.node = node;
            this.weight = weight;
        }
        public Node getNode()
        {
            return this.node;
        }
        public uint getWeight()
        {
            return this.weight;
        }
    }

    class Driver
    {


        public static List<Node> getNodeNetwork()
        {
            List<Node> nodes = new List<Node>();

            Node node1 = new Node();
            Node node2 = new Node();
            Node node3 = new Node();
            Node node4 = new Node();
            Node node5 = new Node();


            node1.linkToNode(node2, 2);

            node2.linkToNode(node3, 8);
            node2.linkToNode(node5, 4);

            node5.linkToNode(node3, 2);

            node3.linkToNode(node4, 6);
            node4.linkToNode(node1, 6);


            nodes.Add(node1);
            nodes.Add(node2);
            nodes.Add(node3);
            nodes.Add(node4);
            nodes.Add(node5);

            return nodes;
        }


        public static Node getNodeWithSmallestDistance(List<Node> nodes)
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


        public static List<Node> cloneListOfNodes(List<Node> nodes)
        {
            List<Node> newListOfNodes = new List<Node>();
            foreach (Node node in nodes)
            {
                newListOfNodes.Add(node);
            }
            return newListOfNodes;
        }


        public static void updateDistance(Node adjacentNode, uint currentAdjacentNodeDistance, uint adjacentNodeDistanceThroughNode)
        {
            if (adjacentNodeDistanceThroughNode < currentAdjacentNodeDistance)
            {
                adjacentNode.setDistance(adjacentNodeDistanceThroughNode);
            }
        }

        public static void setDistancesToAdjacentNodes(Node node)
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


        public static void updateDistanceForEachNode(List<Node> unVisitedNodes, List<Node> nodes)
        {
            for (int i = 0; i < unVisitedNodes.Count; i++)
            {
                Node nextNode = getNodeWithSmallestDistance(nodes);
                setDistancesToAdjacentNodes(nextNode);
                nodes = nodes.Where((node) => !node.Equals(nextNode)).ToList();
            }
        }


        public static List<Node> getNodesWithCalculatedDistances(uint sourceIndex, List<Node> nodes)
        {
            nodes.ElementAt((int)sourceIndex).setDistance(0);
            List<Node> unVisitedNodes = cloneListOfNodes(nodes);
            updateDistanceForEachNode(unVisitedNodes, nodes);
            return unVisitedNodes;
        }




        public static uint getShortestDistanceTo(uint destinationIndex, uint sourceIndex, List<Node> nodes)
        {
            nodes = getNodesWithCalculatedDistances(sourceIndex, nodes);
            Node destination = nodes.ElementAt((int)destinationIndex);
            return destination.getDistance();
        }

        public static List<Node> getAdjacentNodes(Node node)
        {
            List<Node> adjacentNodes = new List<Node>();
            foreach (Link link in node.getLinks())
            {
                adjacentNodes.Add(link.getNode());
            }
            return adjacentNodes;
        }


        public static Node getNextNodeInPath(Node node)
        {
            foreach (Link link in node.getLinks())
            {
                if (link.getNode().getDistance() == node.getDistance() - link.getWeight())
                {
                    return link.getNode();
                }
            }
            return node;
        }


        public static List<Node> getPath(Node destination, List<Node> path)
        {
            if (destination.getDistance() == 0) { return path; }
            Node nextNode = getNextNodeInPath(destination);
            path.Add(nextNode);
            return getPath(nextNode, path);
        } 

        public static List<Node> getPath(uint destinationIndex, uint sourceIndex, List<Node> nodeNetwork)
        {
            Node destination = nodeNetwork.ElementAt((int)destinationIndex);
            nodeNetwork = getNodesWithCalculatedDistances(sourceIndex, nodeNetwork);
            return getPath(destination, new List<Node>() { destination });
        }



        public static void Main(string[] args)
        {

            foreach (Node node in getPath(2, 0, getNodeNetwork()))
            {
                Console.WriteLine(node.getDistance());
            }


        }




    }


}
