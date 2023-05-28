using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waterScript : MonoBehaviour
{
    private bool inWater = false;
    private float hi = 0.0f; // Horizontal Input
    private float ms = 0.0f; // Move Speed
    public LayerMask waterMask;
    public Rigidbody2D myRigidBody;
    public BoxCollider2D boxCollider;
    public float waterSpeedX = 0.50f; // Lower X-direction speed 
    public float waterSpeedY = 0.90f; // Lower Y-direction speed
    // Start is called before the first frame update
    void Start()
    {
        waterMask = LayerMask.GetMask("water");
        myRigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inWater){
            hi = character_script.horizontalInput;
            ms = character_script.moveSpeed;
            myRigidBody.velocity = new Vector2((hi * ms) * waterSpeedX, myRigidBody.velocity.y * waterSpeedY);
            Debug.Log("in water, speed: " + myRigidBody.velocity.x);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("water")) {
            Debug.Log("now in water");
            inWater = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col){
        if(col.CompareTag("water")) {
            inWater = false;
        }
    }
}
