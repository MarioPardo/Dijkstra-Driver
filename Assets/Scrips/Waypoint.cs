using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] List<Waypoint> neighbours;
    [SerializeField] public Dictionary<Waypoint, float> waypointDict = new Dictionary<Waypoint, float>();

    SpriteRenderer spriteRenderer;
    WaypointSystem waypointSystem;
    GameManager gameManager;

    bool drawLinesBool = false;
    bool isSelected = false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        waypointSystem = FindObjectOfType<WaypointSystem>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        changeColor(Color.green);
        showSprite(true);

    }

    void showSprite(bool b)
    {
        spriteRenderer.enabled = b;
    }

    void changeColor(Color c)
    {
        spriteRenderer.color = c;
    }


    void calculateDistances()
    {
        Waypoint currWaypoint = this;
        for (int i = 0; i < neighbours.Count; i++)
        {
            Waypoint nextWaypoint = neighbours[i];
            float distance = Mathf.Sqrt(Mathf.Pow((currWaypoint.transform.position.x - nextWaypoint.transform.position.x), 2) + Mathf.Pow((currWaypoint.transform.position.y - nextWaypoint.transform.position.y), 2));

            waypointDict[nextWaypoint] = distance;
        }

       
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
        if(drawLinesBool)
            drawLines();
    }

    private void OnMouseEnter()
    {
        //Debug.Log("ENTER");
        drawLinesBool = true;
    }

    private void OnMouseExit()
    {
        //Debug.Log("EXIT");
        drawLinesBool = false;
    }

    private void OnMouseDown()
    {
        if (gameManager.doneSetup)
            return;

        if(gameManager.isSelectingStart)
        {
            waypointSystem.startWaypoint = this;
            gameManager.hasSelectedStart = true;

            changeColor(Color.red);
            isSelected = true;
            return;
        }

        if (gameManager.isSelectingPickup)
        {
            waypointSystem.packageWaypoint = this;
            gameManager.hasSelectedPickup = true;

            changeColor(Color.red);
            isSelected = true;
            return;
        }

        if (gameManager.isSelectingDelivery)
        {
            waypointSystem.deliveryWaypoint = this;
            gameManager.hasSelectedDelivery = true;

            changeColor(Color.red);
            isSelected = true;
            return;
        }



    }
}
