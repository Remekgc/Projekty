﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [Tooltip("Start and End Points for the player on the grid")] public Waypoint startPoint, endPoint;
    [SerializeField] bool isRunning = true; // todo make private

    Dictionary<Vector3Int, Waypoint> grid = new Dictionary<Vector3Int, Waypoint>();
    Queue<Waypoint> waypointQueue = new Queue<Waypoint>();
    Waypoint searchCenter;
    Vector3Int[] directions =
    {
        new Vector3Int(0, 0, 1),
        Vector3Int.right,
        new Vector3Int(0, 0, -1),
        Vector3Int.left
    };

    void Start()
    {
        LoadBlocks();
        ColorStartAndEnd();
        FindPath();
        //ExploreNeighbours();
    }

    private void FindPath()
    {
        waypointQueue.Enqueue(startPoint);

        while (waypointQueue.Count > 0 && isRunning)
        {
            searchCenter = waypointQueue.Dequeue();
            print("Searching from " + searchCenter); // todo Remove log
            StopIfEndFound();
            ExploreNeighbours();
            searchCenter.isExplored = true;
        }
    }

    private void StopIfEndFound()
    {
        if (searchCenter == endPoint)
        {
            isRunning = false;
            print("End point found");
        }
    }

    void ExploreNeighbours()
    {
        if (!isRunning)
        {
            return;
        }
        foreach (Vector3Int direction in directions)
        {
            Vector3Int neighbourCoordinates = searchCenter.GetGridPos() + direction;
            try
            {
                QueueNewNeighbours(neighbourCoordinates);
            }
            catch
            {
                // check if function works properly
                //print("Error, block at " + explorationCoordinates + " not found.");
            }
        }
    }

    private void QueueNewNeighbours(Vector3Int neighbourCoordinates)
    {
        Waypoint neighbour = grid[neighbourCoordinates];
        if (!neighbour.isExplored || !waypointQueue.Contains(neighbour))
        {
            waypointQueue.Enqueue(neighbour);
            neighbour.exploredFrom = searchCenter;
            //print("Queueing" + neighbour);
        }
    }

    void ColorStartAndEnd()
    {
        startPoint.SetTopColor(Color.yellow);
        endPoint.SetTopColor(Color.red);
    }

    private void LoadBlocks()
    {
        Waypoint[] waypoints = FindObjectsOfType<Waypoint>();
        foreach (Waypoint waypoint in waypoints)
        {
            // overlaping blocks?
            bool isOverlaping = grid.ContainsKey(waypoint.GetGridPos());
            // add to dictionary
            if (isOverlaping)
            {
                print("Object Overlaping" + waypoint);
            }
            else
            {
                grid.Add(waypoint.GetGridPos(), waypoint);
                //waypoint.SetTopColor(Color.gray);
            }
        }
    }
}
