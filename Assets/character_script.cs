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
    public float leftright;
    
    private float jumpTime;
    private bool jumping;
    private bool jumpCancelled;
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

        if (isGrounded){ //player can only use inputs if not jumping

            if (Input.GetKey(KeyCode.Space)) { // charging jump
                myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
                if (jumpHeight <= 15){
                    jumpHeight += .3f;
                }
                renderer.color = Color.blue;
            } else if(Input.GetKeyUp(KeyCode.Space)) { // jumping release
                if (jumpHeight <= 2){
                    jumpHeight = 2;
                }
                jumpLength += horizontalInput;
                myRigidBody.velocity = new Vector2(jumpLength+.1f, jumpHeight);
                jumpHeight = 0;
            } else if(myRigidBody.velocity.x > 0) { //moving right
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                renderer.color = Color.cyan;
                leftright = 0;      
            } else if(myRigidBody.velocity.x < 0) { // moving left
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                renderer.color = Color.magenta;
                leftright = 1;
            }
            if (!Input.GetKey(KeyCode.Space)){
                myRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, myRigidBody.velocity.y);
            }

        } else {
            renderer.color = Color.red;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = isGrounded ? Color.blue : Color.red;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .5f),
        new Vector2(.9f, .2f));
    }
}


    