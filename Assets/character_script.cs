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
    public float cancelRate = 100;
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

        if(isGrounded && !Input.GetKey(KeyCode.Space)) { // cant walk if charging jump
            myRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, myRigidBody.velocity.y);
        }
        
        if(isGrounded) {
            renderer.color = Color.blue;
        }
        else {
            renderer.color = Color.red;
        }
        

        if (Input.GetKey(KeyCode.Space) && isGrounded && canJump && jumpHeight <= 15) { // charging jump
            jumpHeight += .1f;
        }

        if(Input.GetKeyUp(KeyCode.Space)) { // jumping
            canJump = true;
            myRigidBody.velocity = new Vector2(moveInput, jumpHeight);
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
        
        //Teleport to other side of screen
        Vector3 viewPos = Camera.main.WorldToScreenPoint(transform.position);
        //Left to right
        if(viewPos.x < 0) teleport(viewPos,1);
        //Right to left
        if(viewPos.x > Screen.width) teleport(viewPos,-1);
        
    }

    //Moves the player from one side of the screen to the other side
    // 1 = left to right
    // -1 = right to left
    void teleport(Vector3 viewPos, int side)
    {

    //Position of the opposite side
    Vector3 oppSidePos = new Vector3(Screen.width * side, viewPos.y, viewPos.z);

    //Raidus to check if oppsite side is already occupied by another object
    float radius = 1500;

    if(!Physics.CheckSphere(oppSidePos, radius))
        //Player free to teleport to opposite side
        {
        //Move player to opposite side after transforming screen position to world position
            transform.position = Camera.main.ScreenToWorldPoint(oppSidePos);;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = isGrounded ? Color.blue : Color.red;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .5f),
        new Vector2(.9f, .2f));
    }
}


    