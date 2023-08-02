using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
    [SerializeField] Color32 hasPackageColor = new Color32(1, 1, 1, 1);
    [SerializeField] Color32 emptyColor = new Color32(1, 1, 1, 1);

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    int packageCount = 0;
    int capacity = 1;
    [SerializeField]float destroyPkgDelay = 0.8f;


    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Collided with: " + other.ToString());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Package")
        {
            if(packageCount + 1 <= capacity)
            {
                packageCount++;
                Debug.Log("Picked Up Package, Count:" + packageCount);
                Destroy(other.gameObject, destroyPkgDelay);
            }else
            {
                Debug.Log("Full of packages!");
            }


            spriteRenderer.color = hasPackageColor;
            
        }

        if (other.tag == "Customer")
        {
            if(packageCount > 0)
            {
                Debug.Log("Package Delivered!");
                Destroy(other.gameObject, destroyPkgDelay);
                packageCount--; if (packageCount < 0) packageCount = 0;

            }

            if (packageCount == 0)
                spriteRenderer.color = emptyColor;

        }





    }
}


