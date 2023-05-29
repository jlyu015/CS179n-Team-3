using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lavaScript : MonoBehaviour
{
    public GameObject startPoint; // Reference to the startPoint object

    // OnCollisionEnter is called when a collision occurs
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Touched lava!");
            collision.transform.position = startPoint.transform.position;
        }
    }
}
