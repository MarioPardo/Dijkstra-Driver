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

    void Awake()
    {
        
    }



    private void Start()
    {
        car.transform.position = startWaypoint.transform.position;
        carDriver = car.GetComponent<Driver>();

        CalculatePath();
        StartCoroutine(StartDriving());
    }


    void CalculatePath()
    {
        pathStack.Push(deliveryWaypoint);
        pathStack.Push(packageWaypoint);
    }

    IEnumerator StartDriving()
    {
        while(pathStack.Count > 0)
        {
            Debug.Log("Called here");
            car.GetComponent<Driver>().DriveTo(pathStack.Pop());
            yield return new WaitUntil(() => carDriver.isDriving == false);
        }

        finished = true;
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

    void Update()
    {
        
    }
}
