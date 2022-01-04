using Dijkstra;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Grille Grille;
    public GeneticAlgo GeneticAlgo;
    [SerializeField]
    public float Speed = 0.4f;
    protected Node Current;
    public Path Path = new Path();
    private Node Target;
    public Node end;
    private int i = 0;
    void Start()
    {
        var start = Grille.GetNearestNode(this.transform.position);
        Path = GeneticAlgo.RunGeneticAlgorithm();
        Target = Path.Nodes.LastOrDefault();
    }
    // Update is called once per frame
    void Update()
    {
        if(Target != null && Input.GetMouseButtonDown(0) && i < Path.Nodes.Count)
        {
            Current = Path.Nodes[i];
            transform.position = Vector3.MoveTowards(transform.position, Current.Position, Speed);
            i++;
        }
    }
   
}
