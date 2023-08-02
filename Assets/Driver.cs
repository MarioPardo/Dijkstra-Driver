using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
   [SerializeField] float steerSpeed = 300f;
   [SerializeField] float moveSpeed = 20f;
   [SerializeField] float slowSpeed = 5f;
   [SerializeField] float boostSpeed = 60f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boost")
        {
            moveSpeed = boostSpeed;
            Debug.Log("BOOST");
        }
            
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("SLOW");

        moveSpeed = slowSpeed;
    }   



    void Update()
    {
        float steerAmount = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Rotate(0,0,-steerAmount);
        transform.Translate(0,moveAmount,0);
    }
}
