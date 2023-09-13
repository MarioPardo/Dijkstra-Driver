using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textArea;
    WaypointSystem waypointSystem;

    public bool isSelectingStart = false;
    public bool hasSelectedStart = false;

    public bool isSelectingPickup = false;
    public bool hasSelectedPickup = false;

    public bool isSelectingDelivery = false;
    public bool hasSelectedDelivery = false;

    public bool doneSetup = false;

    private void Start()
    {
        waypointSystem = FindObjectOfType<WaypointSystem>();
    }

    public void PlayGame()
    {
        textArea.text = "";
        waypointSystem.StartGame();
    }

    public void StartSetup()
    {
        StartCoroutine(Setup());
    }


    public IEnumerator Setup()
    {
        isSelectingStart = true;
        textArea.text = "Please select Start Location";
        yield return new WaitUntil(() => hasSelectedStart == true);
        isSelectingStart = false;
    

        isSelectingPickup = true;
        textArea.text = "Please select Pickup Location";
        yield return new WaitUntil(() => hasSelectedPickup == true);
        isSelectingPickup = false;
        

        isSelectingDelivery = true;
        textArea.text = "Please select Delivery Location";
        yield return new WaitUntil(() => hasSelectedDelivery == true);
        isSelectingDelivery = false;

        doneSetup = true;
        textArea.text = "Ready To Play!";

        Debug.Log("Ready to Play!");
        StopAllCoroutines();
        


    }
}
