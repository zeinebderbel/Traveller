using System.Collections.Generic;

namespace Dijkstra
{
    public class Path
    {
        public List<Node> Nodes { get; set; }
        public float TotalWeight { get; set; }

        public Path(List<Node> nodes, float totalWeight)
        {
            Nodes = nodes;
            TotalWeight = totalWeight;
        }

        public Path()
        {
        }
    }
}
