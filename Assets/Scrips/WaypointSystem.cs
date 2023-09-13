using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointSystem : MonoBehaviour
{
    [SerializeField] GameObject car;
    Driver carDriver;

    [SerializeField] Waypoint startWaypoint;
    [SerializeField] Waypoint packageWaypoint;
    [SerializeField] Waypoint deliveryWaypoint;

    Stack<Waypoint> pathStack = new Stack<Waypoint>();

    bool finished = false;

    private void Start()
    {
        car.transform.position = startWaypoint.transform.position;
        carDriver = car.GetComponent<Driver>();

        CalculatePath();
        //PrintPath();
        StartCoroutine(StartDriving());
    }


    void CalculatePath()
    {


        //path to delivery
        List<Waypoint> tempList = FindShortestPath(packageWaypoint, deliveryWaypoint);

        for (int i = tempList.Count - 1; i >= 0; i--)
        {
            pathStack.Push(tempList[i]);
        }

        tempList.Clear();
        //path to pickup
        tempList = FindShortestPath(startWaypoint, packageWaypoint);

        for(int i = tempList.Count -1; i >=0; i--)
        {
            pathStack.Push(tempList[i]);
        }

    }

    void PrintPath()
    {
        Debug.Log("PATH:");
        Waypoint[] arr = pathStack.ToArray();
        for(int i = 0; i < arr.Length; i++)
        {
            Debug.Log(" - " + arr[i]);
        }
    }
   

    IEnumerator StartDriving()
    {
        while(pathStack.Count > 0)
        {
            car.GetComponent<Driver>().DriveTo(pathStack.Pop());
            yield return new WaitUntil(() => carDriver.isDriving == false);
            
        }
        Debug.Log("Finished Path!");
    }

    private void OnDrawGizmos()
    {
        foreach(Transform t in transform) //every transform
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(t.position, 1.5f);
        }
    }

    public List<Waypoint> FindShortestPath(Waypoint startingWaypoint, Waypoint endWaypoint)
    {
        // Initialize dictionaries to store distance and previous waypoints
        Dictionary<Waypoint, float> distance = new Dictionary<Waypoint, float>();
        Dictionary<Waypoint, Waypoint> previous = new Dictionary<Waypoint, Waypoint>();

        // Initialize a priority queue for unvisited waypoints
        SortedSet<Tuple<float, Waypoint>> unvisited = new SortedSet<Tuple<float, Waypoint>>();

        // Set the distance of the starting waypoint to 0
        distance[startingWaypoint] = 0;

        // Add the starting waypoint to the unvisited set with a distance of 0
        unvisited.Add(new Tuple<float, Waypoint>(0, startingWaypoint));

        while (unvisited.Count > 0)
        {
            // Get the waypoint with the smallest distance from the unvisited set
            Waypoint currentWaypoint = unvisited.Min.Item2;
            float currentDistance = unvisited.Min.Item1;

            // Remove the current waypoint from the unvisited set
            unvisited.Remove(unvisited.Min);

            // If we reach the end waypoint, reconstruct and return the path
            if (currentWaypoint == endWaypoint)
            {
                return ReconstructPath(previous, endWaypoint);
            }

            // Iterate through neighbors of the current waypoint
            foreach (var neighbor in currentWaypoint.waypointDict.Keys)
            {
                float tentativeDistance = currentDistance + currentWaypoint.waypointDict[neighbor];

                // If this path is shorter than the previously recorded distance, update it
                if (!distance.ContainsKey(neighbor) || tentativeDistance < distance[neighbor])
                {
                    distance[neighbor] = tentativeDistance;
                    previous[neighbor] = currentWaypoint;

                    // Add the neighbor to the unvisited set with its new distance
                    unvisited.Add(new Tuple<float, Waypoint>(tentativeDistance, neighbor));
                }
            }
        }

        // If no path is found, return an empty list
        return new List<Waypoint>();
    }

    List<Waypoint> ReconstructPath(Dictionary<Waypoint, Waypoint> previous, Waypoint endWaypoint)
    {
        List<Waypoint> path = new List<Waypoint>();
        Waypoint current = endWaypoint;

        while (previous.ContainsKey(current))
        {
            path.Add(current);
            current = previous[current];
        }

        path.Reverse(); // Reverse the list to get the correct order
        return path;
    }
}
