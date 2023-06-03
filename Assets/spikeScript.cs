using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeScript : MonoBehaviour
{
    public float bounceForce = 10f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 bounceDirection = collision.gameObject.transform.position - transform.position;
                playerRb.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);
            }
        }
    }
}