using Dijkstra;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgo : MonoBehaviour
{
    //List of all destinations
    private List<Node> stopsList;
    public Grille grille;
    [SerializeField]
    public List<List<Node>> Populations;
    public int PopSize = 7;
    public int eliteSize = 10;
    public int nbGenesInFirstPop = 3;
    //private Node start;
    public int nbGeneration = 10;
    public int individualsToKill = 50;

    private static System.Random rng = new System.Random();

    public static void Shuffle(ref List<Node> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Node value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    private void Start()
    {
        var stops = GameObject.FindGameObjectsWithTag("Stop");
        stopsList = new List<Node>();
        foreach (var stop in stops)
        {
            stopsList.Add(grille.GetNearestNode(stop.transform.position));
        }
    }
    //Create 1 population
    private List<Node> CreateTrip()
    {
        var trip = stopsList.OrderBy(arg => Guid.NewGuid()).Take(stopsList.Count).ToList();
        return trip;
    }

    public void SetPopulation()
    {
        Populations = new List<List<Node>>();
        for (int i = 0; i < PopSize; i++)
            Populations.Add(CreateTrip());
    }

    public Dictionary<List<Node>, float> RankFitness()
    {
        Dictionary<List<Node>, float> fitnessResults = new Dictionary<List<Node>, float>();
        for (int i = 0; i < Populations.Count; i++)
            fitnessResults.Add(Populations[i], new Fitness(Populations[i]).GetFitnessScore(grille));
        return fitnessResults.OrderBy(n => n.Value).ToDictionary(x => x.Key, x => x.Value);
    }

    private List<List<Node>> Breed(List<List<Node>> selectedElitePop)
    {
        List<List<Node>> newGenes = new List<List<Node>>();
        for (int i = 0; i < selectedElitePop.Count - 1; i++)
        {
            newGenes.Add(BreedParents(selectedElitePop[i], selectedElitePop[i + 1]));
        }
        return newGenes;
    }

    public List<Node> BreedParents(List<Node> firstPop, List<Node> secondPop)
    {
        List<Node> newPop = firstPop.Take(nbGenesInFirstPop).ToList<Node>();
        int rest = secondPop.Count - nbGenesInFirstPop;
        int index = 0;
        for (int i = 0; i < secondPop.Count; i++)
        {
            if (!newPop.Contains(secondPop[i]))
            {
                newPop.Add(secondPop[i]);
                index++;
            }
            if (index == rest)
                break;
        }
        MutateGenes(ref newPop);
        return newPop;
    }
    void MutateGenes(ref List<Node> child)
    {
        Shuffle(ref child);
    }
    public List<List<Node>> GetNextGeneration()
    {
        var rankedPopWithFitness = RankFitness();
        //keep only the k best and kill the rest 
        rankedPopWithFitness = rankedPopWithFitness.Take(PopSize - individualsToKill).ToDictionary(x => x.Key, x => x.Value);
        Debug.Log(rankedPopWithFitness.FirstOrDefault().Value);
        string path = string.Empty;
        for (int i = 0; i < stopsList.Count; i++)
        {
            path = path + " " + rankedPopWithFitness.FirstOrDefault().Key[i].Id;
        }
        Debug.Log(path);

        var population = rankedPopWithFitness.Keys.ToList();
        var selectedElitePop = population.Take(eliteSize).ToList();

        var newGeneration = Breed(selectedElitePop);
        List<List<Node>> newPop = population;
        newPop.AddRange(newGeneration);
        return newPop;
    }
    public Path RunGeneticAlgorithm()
    {
        SetPopulation();

        for (int i = 0; i < nbGeneration; i++)
        {
            Populations = GetNextGeneration();
        }
        var result = RankFitness().FirstOrDefault();
        Path path = new Path(result.Key, result.Value);
        return path;
    }
}
