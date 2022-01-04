using System;
using System.Collections.Generic;
using System.Linq;

namespace Dijkstra
{
    public class Graph
    {
        private Node target;
        public List<Node> Nodes { get; set; }
        public Graph(List<Node> nodes)
        {
            Nodes = nodes;
        }

        public Path GetShortestPath(Node start, Node end)
        {
            target = end;
            if (start == null || end == null)
            {
                throw new ArgumentNullException();
            }

            Path path = new Path();

            if (start == end)
            {
                path.Nodes.Add(start);
                return path;
            }

            var PriorityQueue = new List<Node>() { start };
            var currentNode = start;
            var visitedNodes = new List<Node>();
            Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

            Dictionary<Node, float> distances = new Dictionary<Node, float>();

            foreach (var node in Nodes)
            {
                distances[node] = float.MaxValue;
            }
            distances[start] = 0f;

            int i = 0;
            while (PriorityQueue.Any())
            {
                currentNode = PriorityQueue.OrderBy(n => distances[n]).FirstOrDefault();
                PriorityQueue.Remove(currentNode);
                if(currentNode == end)
                {
                    path.TotalWeight = distances[currentNode];
                    path.Nodes = new List<Node>();

                    while (previous.ContainsKey(end) && currentNode != start)
                    {
                        path.Nodes.Insert(0,currentNode);
                        currentNode = previous[currentNode];
                    }
                    path.Nodes.Insert(0, currentNode);
                    break;
                }
                if (currentNode.Neighbors != null)
                {
                    foreach (var neighbor in currentNode.Neighbors.OrderBy(n => n.Value))
                    {
                        if ( visitedNodes.Contains(neighbor.Key )|| !neighbor.Key.IsWalkable)
                            continue;
                        float cost = neighbor.Value + distances[currentNode];
                        if (cost < distances[neighbor.Key])
                        {
                            distances[neighbor.Key] = cost;
                            previous[neighbor.Key] = currentNode;
                            if (!PriorityQueue.Contains(neighbor.Key))
                                PriorityQueue.Add(neighbor.Key);
                        }
                    }
                }
                visitedNodes.Add(currentNode);
                i++;
            }

            return path;
        }
    }
}
