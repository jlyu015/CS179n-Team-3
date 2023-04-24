using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_script : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public LayerMask groundMask;
    public SpriteRenderer characterRender;
    public float groundDist = 1f;
    public float moveSpeed = 10;
    public float buttonTime = 0.5f;
    public float jumpHeight = 10;
    public float jumpLength = 0;
    public float cancelRate = 100;
    public float dir = 0;
    public float moveInput;
    
    private float jumpTime;
    private bool jumping;
    private bool jumpCancelled;
    private bool canJump;
    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {

        gameObject.name = "Hop Queen";

    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        
        
        Vector2 wireCubeSize = new Vector2(0.9f, 0.2f);
        Vector2 wireCubePos = new Vector2(transform.position.x, transform.position.y - 0.5f);
        isGrounded = Physics2D.OverlapBox(wireCubePos, wireCubeSize, 0, groundMask);
        Debug.Log("isGrounded: " + isGrounded);
        
        float horizontalInput = Input.GetAxisRaw("Horizontal"); 

        if (!isGrounded){
        } else {
            if(isGrounded && !Input.GetKey(KeyCode.Space)) { // cant walk if charging jump
                myRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, myRigidBody.velocity.y);
            }
            
            if(!isGrounded) {
                renderer.color = Color.red;
            }
            

            if (Input.GetKey(KeyCode.Space) && isGrounded && canJump && jumpHeight <= 12) { // charging jump
                myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
                jumpHeight += .2f;
                renderer.color = Color.blue;
            }

            if(Input.GetKeyUp(KeyCode.Space)) { // jumping
                canJump = true;
                jumpLength += horizontalInput;
                myRigidBody.velocity = new Vector2(jumpLength, jumpHeight);
                jumpHeight = 0;
            }


            if (myRigidBody.velocity.x > 0 && isGrounded) {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                renderer.color = Color.cyan;
            } 
            else if (myRigidBody.velocity.x < 0 && isGrounded) {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                renderer.color = Color.magenta;
            }
            
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = isGrounded ? Color.blue : Color.red;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .5f),
        new Vector2(.9f, .2f));
    }
}


    