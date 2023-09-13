using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] List<Waypoint> neighbours;
    [SerializeField] public Dictionary<Waypoint, float> waypointDict = new Dictionary<Waypoint, float>();

    void calculateDistances()
    {
        Waypoint currWaypoint = this;
        for (int i = 0; i < neighbours.Count; i++)
        {
            Waypoint nextWaypoint = neighbours[i];
            float distance = Mathf.Sqrt(Mathf.Pow((currWaypoint.transform.position.x - nextWaypoint.transform.position.x), 2) + Mathf.Pow((currWaypoint.transform.position.y - nextWaypoint.transform.position.y), 2));

            waypointDict[nextWaypoint] = distance;
        }

        /*
        Debug.Log(this.name + "'s thhings are" );
        foreach(KeyValuePair<Waypoint, float> kvp in waypointDict)
        {
           Debug.Log("  " + kvp.Key.name + " : " + kvp.Value.ToString());
        }
        */
    }

    void drawLines()
    {
        Waypoint currWaypoint = this;
        for (int i = 0; i < neighbours.Count; i++)
        {
            Waypoint nextWaypoint = neighbours[i];
            Debug.DrawLine((Vector2)transform.position, (Vector2)nextWaypoint.transform.position);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        calculateDistances();
        drawLines();
    }

    // Update is called once per frame
    void Update()
    {
        drawLines();
    }
}
