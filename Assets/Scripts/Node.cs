using System.Collections.Generic;
using UnityEngine;

namespace Dijkstra
{
    public class Node
    {
        public bool IsWalkable { get; set; }
        public int Id { get; set; }
        public Dictionary<Node, float> Neighbors { get; set; }
        public Vector3 Position { get; set; }

        public Node(bool isWalkable, Dictionary<Node, float> neighbors)
        {
            IsWalkable = isWalkable;
            Neighbors = neighbors;
        }

        public Node()
        {
        }
        public float GetDistanceTo(Node target, Graph graph)
        {
            return (this.Position - target.Position).magnitude;
            //return graph.GetShortestPath(this, target).TotalWeight;
        }
    }
}
