﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_Astar
{
    public class FollowPath : MonoBehaviour
    {
        Transform goal;
        float speed = 5.0f;
        float accuracy = 1.0f;
        float rotSpeed = 2.0f;
        public WaypointManager wpManager;
        GameObject[] wps;
        GameObject currentNode;
        int currentWP = 0;
        Graph g;

        void Start()
        {
            wps = wpManager.waypoints;
            g = wpManager.graph;
            currentNode = wps[0];
        }

        public void GoToHeli()
        {
            g.AStar(currentNode, wps[10]);
            currentWP = 0;
        }

        public void GoToRuin()
        {
            g.AStar(currentNode, wps[6]);
            currentWP = 0;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (g.getPathLength() == 0 || currentWP == g.getPathLength()) 
                return;
            
            //the node we are closest to at this moment
            currentNode = g.getPathPoint(currentWP);
            
            //if we are close enough to the current waypoioint move to next
            if (Vector3.Distance(g.getPathPoint(currentWP).transform.position, transform.position) < accuracy)
                currentWP++;

            // if we are not at hte end of the path
            if (currentWP < g.getPathLength())
            {
                goal = g.getPathPoint(currentWP).transform;
                Vector3 lookAtGoal = new Vector3(goal.position.x, transform.position.y, goal.position.z);
                Vector3 direction = lookAtGoal - transform.position;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);
            }
        }
    }
}
