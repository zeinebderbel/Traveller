using Dijkstra;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Grille : MonoBehaviour
{
    public Material mat;
    public Graph Graph;
    public Vector3[] vertices;
    Dictionary<int, Node> NodesVertices;

    void Start()
    {
        int nbColonne = 24;
        int nbLigne = 24;
        Graph = new Graph(new List<Node>());
        NodesVertices = new Dictionary<int, Node>();
        vertices = new Vector3[(nbColonne + 1) * (nbLigne + 1)];
        for (int i = 0, z = -12; z <= 12; z++)
        {
            for (int x = -12; x <= 12; x++, i++)
            {
                vertices[i] = new Vector3(x, 1, z);
                var node = new Node()
                {
                    Id = i,
                    Position = vertices[i],
                    IsWalkable = true
                };
                SetWalkableNodes(node);
                Graph.Nodes.Add(node);
                NodesVertices.Add(i, node);
            }
        }
        SetNeighbors();
       

        Mesh msh = new Mesh();

        msh.vertices = vertices;

    }
#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (vertices != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.01f);
                Handles.Label(vertices[i] - Vector3.up * .01f, i.ToString());
            }
        }
    }
#endif
    public void SetNeighbors()
    {
        foreach (Node node in Graph.Nodes)
        {
            #region les extremites
            if (node.Id == 0)
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[25], 1f },
                    { NodesVertices[26], 1.42f },
                    { NodesVertices[1], 1f },
                };
            }
            else if (node.Id == 600)
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[575], 1f },
                    { NodesVertices[601], 1f },
                    { NodesVertices[576], 1.42f },
                };
            }
            else if (node.Id == 624)
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[623], 1f },
                    { NodesVertices[599], 1f },
                    { NodesVertices[598], 1.42f },
                };
            }
            else if (node.Id == 24)
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[23], 1f },
                    { NodesVertices[49], 1f },
                    { NodesVertices[48], 1.42f },
                };
            }
            #endregion
            #region les arêtes
            else if (node.Id < 24 && node.Id > 0)
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[node.Id+25], 1f },
                    { NodesVertices[node.Id+24], 1.42f },
                    { NodesVertices[node.Id+26], 1.42f },
                    { NodesVertices[node.Id+1], 1f },
                    { NodesVertices[node.Id-1], 1f },
                };
            }
            else if (node.Id % 25 == 0)
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[node.Id+25], 1f },
                    { NodesVertices[node.Id+26], 1.42f },
                    { NodesVertices[node.Id+1], 1f },
                    { NodesVertices[node.Id-24], 1.42f },
                    { NodesVertices[node.Id-25], 1f },
                };
            }
            else if ((node.Id - 24) % 25 == 0)
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[node.Id+24], 1.42f },
                    { NodesVertices[node.Id+25], 1f },
                    { NodesVertices[node.Id-1], 1f },
                    { NodesVertices[node.Id-26], 1.42f },
                    { NodesVertices[node.Id-25], 1f },
                };
            }
            else if (node.Id < 624 && node.Id > 600)
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[node.Id-25], 1f },
                    { NodesVertices[node.Id-24], 1.42f },
                    { NodesVertices[node.Id-26], 1.42f },
                    { NodesVertices[node.Id+1], 1f },
                    { NodesVertices[node.Id-1], 1f },
                };
            }
            #endregion
            #region reste de la grille
            else
            {
                node.Neighbors = new Dictionary<Node, float>()
                {
                    { NodesVertices[node.Id-25], 1f },
                    { NodesVertices[node.Id-24], 1.42f },
                    { NodesVertices[node.Id-26], 1.42f },
                    { NodesVertices[node.Id+1], 1f },
                    { NodesVertices[node.Id-1], 1f },
                    { NodesVertices[node.Id+25], 1f },
                    { NodesVertices[node.Id+24], 1.42f },
                    { NodesVertices[node.Id+26], 1.42f },
                };
            }
            #endregion
        }

    }
    public void SetWalkableNodes(Node n)
    {
        var colliders = Physics.OverlapSphere(n.Position, 0.3f);
        if (colliders.Any(c => c.tag == "Wall"))
            n.IsWalkable = false;
    }
    public Node GetNearestNode(Vector3 targetPosition)
    {
        var min = float.MaxValue;
        var nearestNode = new Node();

        foreach (var node in Graph.Nodes)
        {
            var distance = Vector3.Distance(node.Position, targetPosition);
            if (min > distance)
            {
                min = distance;
                nearestNode = node;
            }
        }
        return nearestNode;
    }
}
