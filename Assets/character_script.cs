using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_script : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public LayerMask groundMask;
    public SpriteRenderer characterRender;
    public PhysicsMaterial2D bounce, normalMat;
    public float groundDist = 1f;
    public float moveSpeed = 10;
    public float buttonTime = 0.5f;
    public float jumpHeight = 10;
    public float jumpLength = 0;
    public float cancelRate = 100;
    public float dir = 0;
    public float moveInput;
    public float leftright;
    
    private float jumpTime;
    private bool jumping;
    private bool jumpCancelled;
    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = "Hop Queen";
        myRigidBody.gravityScale = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

        Vector2 wireCubeSize = new Vector2(0.7f, 0.7f);
        Vector2 wireCubePos = new Vector2(transform.position.x, transform.position.y - 0.5f);
        isGrounded = Physics2D.OverlapBox(wireCubePos, wireCubeSize, 0, groundMask);
        Debug.Log("isGrounded: " + isGrounded);
        
        float horizontalInput = Input.GetAxisRaw("Horizontal"); 

        if (isGrounded){ //player can only use inputs if not jumping
            myRigidBody.sharedMaterial = normalMat;
            renderer.color = Color.blue;
            if (Input.GetKey(KeyCode.Space)) { // charging jump
                myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
                if (jumpHeight <= 12){
                    jumpHeight += .15f;
                }
                
            } else if(Input.GetKeyUp(KeyCode.Space)) { // jumping release
                if (jumpHeight <= 2){
                    jumpHeight = 2;
                }
                float jumpVelocityMagnitude = jumpHeight * 0.3f;
                Vector2 jumpVelocity = new Vector2(jumpVelocityMagnitude * horizontalInput, jumpVelocityMagnitude);
                myRigidBody.velocity = jumpVelocity;
                Vector2 jumpDirection = new Vector2(jumpLength, jumpHeight);
                myRigidBody.AddForce(jumpDirection, ForceMode2D.Impulse);

                jumpHeight = 0;
            } else if(myRigidBody.velocity.x > 0 + .001f) { //moving right
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                renderer.color = Color.cyan;
                leftright = 0;      
            } else if(myRigidBody.velocity.x < 0 - .001f) { // moving left
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                renderer.color = Color.magenta;
                leftright = 1;
            }
            if (!Input.GetKey(KeyCode.Space)){
                myRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, myRigidBody.velocity.y);
            }

        } else {
            renderer.color = Color.red;
            myRigidBody.sharedMaterial = bounce;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = isGrounded ? Color.blue : Color.red;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .5f),
        new Vector2(.9f, .2f));
    }
}


    