using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windScript : MonoBehaviour
{
    public float windStrength = 1f;
    public Vector2 windDirection = Vector2.right;
    public string characterTag = "Player"; // Assign the tag of the character object in the inspector
    private bool isCharacterInZone = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(characterTag))
        {
            Rigidbody2D characterRB = collision.attachedRigidbody;
            characterRB.AddForce(windStrength * windDirection.normalized);
            isCharacterInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(characterTag))
        {
            isCharacterInZone = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isCharacterInZone ? Color.red : Color.cyan;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
