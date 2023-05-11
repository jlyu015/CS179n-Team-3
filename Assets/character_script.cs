using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_script : MonoBehaviour
{
    // Private Variables
    private bool isGrounded = false;
    private bool hitTop = false;
    // Public Variables
    public Rigidbody2D myRigidBody;
    public LayerMask groundMask;
    public SpriteRenderer characterRender;
    public Animator animator;
    public BoxCollider2D boxCollider;
    public PhysicsMaterial2D bounce, normalMat, slope;
    public float wireCubeSizeBotX = 0.6f;
    public float wireCubeSizeBotY = 0.25f;
    public float wireCubeSizeTopX = 0.6f;
    public float wireCubeSizeTopY = 0.25f;
    public float groundDist = 1f;
    public float moveSpeed = 7f;
    public float buttonTime = 0.5f;
    public float jumpHeight = 10;
    public float jumpLength = 0;
    public float cancelRate = 100;
    public float dir = 0;
    public float moveInput;
    public float leftright;
    public float slideSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Game Object's Components, Character is parent, Sprite is child
        // Sprite is a child so it can be resized without modifying the rigidbody of the charcter
        characterRender = GetComponentInChildren<SpriteRenderer>(); // Returns child's SpriteRenderer component
        animator =  GetComponentInChildren<Animator>();             // Returns child's Animator component
        myRigidBody = GetComponent<Rigidbody2D>();                  // Returns the Rigidbody 2D component
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();  // Returns the BoxCollider 2D component
        groundMask = LayerMask.GetMask("terrain");                  // Returns the groundMask as the "terrain"
        gameObject.name = "Hop Queen";  // Our Queen
        myRigidBody.gravityScale = 3;   // Gravity
        moveSpeed = 7f;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 wireCubeSizeBot = new Vector2(wireCubeSizeBotX, wireCubeSizeBotY);
        Vector2 wireCubeSizeTop = new Vector2(wireCubeSizeTopX,wireCubeSizeTopY);
        Vector2 wireCubePos = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Vector2 wireCubePosTop = new Vector2(transform.position.x,transform.position.y + 0.5f);
        isGrounded = Physics2D.OverlapBox(wireCubePos, wireCubeSizeBot, 0, groundMask);
        hitTop = Physics2D.OverlapBox(wireCubePosTop, wireCubeSizeTop, 0, groundMask);
        //Debug.Log("isGrounded: " + isGrounded);
        //Debug.Log("hitHead: " + hitTop);

        float horizontalInput = Input.GetAxisRaw("Horizontal"); 
        animator.SetFloat("isInAir", myRigidBody.velocity.y);
        
        if (isGrounded){ //player can only use inputs if not jumping
            animator.SetFloat("isMoving", Mathf.Abs(myRigidBody.velocity.x));
            animator.SetBool("isGrounded", isGrounded);
            myRigidBody.sharedMaterial = normalMat;
            // Cast a ray down from the center of the character's collider to detect ground
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundMask);
            if (hit.collider != null) {
                // Get the angle of the surface the character is standing on
                float angle = Vector2.Angle(hit.normal, Vector2.up);
                
                // Check if the angle is greater than a certain threshold to determine if the character is on a slope
                if (angle > 44.0f && angle < 46.0f) {
                    // Set the character's movement to follow the slope
                    animator.SetBool("isSliding", true);
                    myRigidBody.sharedMaterial = slope;
                    Vector2 slopeDirection = Vector2.Reflect(myRigidBody.velocity.normalized, hit.normal).normalized;
                    myRigidBody.velocity = slopeDirection * slideSpeed;
                    float slopeAngle = Mathf.Atan2(slopeDirection.y, slopeDirection.x) * Mathf.Rad2Deg;
                    animator.transform.rotation = Quaternion.Euler(0f, 0f, slopeAngle);


                    horizontalInput = 0;
                } else {
                    // Reset the character's rotation if they are not on a slope
                    animator.SetBool("isSliding", false);
                    animator.transform.rotation = Quaternion.identity;
                }
            }
            // Player presses/holds spacebar and starts charging the jump
            if (Input.GetKey(KeyCode.Space)) {
                // Animation - Charging
                animator.SetBool("isCharging", true);
                myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
                if (jumpHeight <= 11.5){
                    jumpHeight += .30f;
                }
                //characterRender.color = Color.blue;
            } 
            // Player releases spacebar and jumps
            else if(Input.GetKeyUp(KeyCode.Space)) { 
                // Animation - Jump Up
                animator.SetBool("isCharging", true);
                animator.SetBool("isGrounded", isGrounded);
                if (jumpHeight <= 2){
                    jumpHeight = 2;
                }
                float jumpVelocityMagnitude = jumpHeight * 0.3f;
                Vector2 jumpVelocity = new Vector2(jumpVelocityMagnitude * horizontalInput, jumpVelocityMagnitude);
                myRigidBody.velocity = jumpVelocity;
                Vector2 jumpDirection = new Vector2(jumpLength, jumpHeight);
                //Debug.Log("jumpDirection:" + jumpDirection);
                myRigidBody.AddForce(jumpDirection, ForceMode2D.Impulse);
                jumpHeight = 0;
            } 
            // Player movement - right
            else if(myRigidBody.velocity.x > 0 + .001f || horizontalInput == 1) { 
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                //characterRender.color = Color.cyan;
                leftright = 0;      
            } 
            // Player movement - left
            else if(myRigidBody.velocity.x < 0 - .001f || horizontalInput == -1) {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                //characterRender.color = Color.magenta;
                leftright = 1;
            }
            if (!Input.GetKey(KeyCode.Space)){
                myRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, myRigidBody.velocity.y);
            }
       
        } 
         // In the air
        else {
            if(!hitTop) {
                myRigidBody.sharedMaterial = bounce;
            }
            // Animation - Rising
            animator.SetBool("isCharging", false);
            animator.SetBool("isGrounded", isGrounded);
            //characterRender.color = Color.red;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = isGrounded ? Color.blue : Color.red;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.5f),
        new Vector2(wireCubeSizeBotX, wireCubeSizeBotY));
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x,gameObject.transform.position.y + 0.5f),
        new Vector2(wireCubeSizeTopX,wireCubeSizeTopY));
    }
}


    