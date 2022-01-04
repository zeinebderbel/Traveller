using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dijkstra;

public class Fitness
{
    private float distance;
    public List<Node> trip;

    public Fitness(List<Node> trip)
    {
        this.trip = trip;
    }

    private float TripDistance(Grille grille)
    {
        distance = 0;
        for (int i = 0; i < trip.Count - 1; i++)
        {
            distance += Vector3.Distance(trip[i].Position, trip[i + 1].Position);
        }
        return distance;
    }

    public float GetFitnessScore(Grille grille)
    {
        if (trip.Count == 0)
            return float.MaxValue;
        return TripDistance(grille);
    }
}
