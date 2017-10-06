using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DijkstrasAlgorithm
{

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
        public void setWeight(uint newWeight)
        {
            this.weight = newWeight;
        }
    }

}
