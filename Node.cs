using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithm
{

    class Node
    {
        uint distanceToSource = uint.MaxValue;
        List<Link> links;
        public Node()
        {
            this.links = new List<Link>();
        }
        public List<Link> getLinks()
        {
            return this.links;
        }
        int getIndexOfLinkTo(Node node)
        {
            for (int i = 0; i < this.links.Count; i++)
            {
                if (this.links.ElementAt(i).getNode().Equals(node))
                {
                    return i;
                }
            }
            return -1;
        }
        public void linkToNode(Node node, uint weight)
        {
            int indexOfLinkToNode = getIndexOfLinkTo(node);
            if (indexOfLinkToNode != -1)
            {
                this.links.ElementAt(indexOfLinkToNode).setWeight(weight);
                node.links.ElementAt(node.getIndexOfLinkTo(this)).setWeight(weight);
                
            } else
            {
                this.links.Add(new Link(node, weight));
                node.links.Add(new Link(this, weight));
            }
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

}
