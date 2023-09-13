using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{
   [SerializeField] float steerSpeed =0.1f;
   [SerializeField] float moveSpeed = 20f;
   [SerializeField] float slowSpeed = 5f;
   [SerializeField] float boostSpeed = 60f;

    private Coroutine rotatingCoroutine;
    private Coroutine movingCoroutine;

    Waypoint currentWaypoint;
    Transform targetTransform;

    public bool isDriving = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boost")
        {
            moveSpeed = boostSpeed;
        }
            
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        moveSpeed = slowSpeed;
    }   

    public void DriveTo(Waypoint waypoint)
    {
        currentWaypoint = waypoint;
        targetTransform = waypoint.transform;

        rotatingCoroutine = StartCoroutine(RotateTowards());
        movingCoroutine = StartCoroutine(MoveTowards());

        isDriving = true;
    }


    IEnumerator RotateTowards()
    {
        while (true)
        {
            Vector2 direction = targetTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

            // Calculate the desired rotation and interpolate it smoothly.
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, steerSpeed * Time.deltaTime);

            yield return null;
        }
    }

    IEnumerator MoveTowards()
    {
        while (Vector3.Distance(transform.position, targetTransform.position) > 0.01f)
        {
            // Calculate the direction to the target.
            Vector3 direction = targetTransform.position - transform.position;

            // Normalize the direction vector and move by a fixed distance each frame.
            Vector3 moveAmount = direction.normalized * moveSpeed * Time.deltaTime;

            // Move the object.
            transform.position += moveAmount;

            yield return null; // Yielding null means this coroutine will run once per frame.
        }
    }



    void Update()
    {
        /*
        float steerAmount = Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime;
        float moveAmount = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Rotate(0,0,-steerAmount);
        transform.Translate(0,moveAmount,0);
        */

        float distToTarget = Vector3.Distance(transform.position, targetTransform.position);
        //Debug.Log("Dist To Target:" + distToTarget);

        if (distToTarget < 0.5f)
        {
            Debug.Log("Reached");
            StopAllCoroutines();
            isDriving = false;
        }

    }
}
