using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_script : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public LayerMask groundMask;
    public SpriteRenderer characterRender;
    public PhysicsMaterial2D bounce, normalMat;
    public Animator animator;

    public float groundDist = 1f;
    public float moveSpeed = 7f;
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
    private bool hitTop = false;

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
        characterRender = GetComponentInChildren<SpriteRenderer>();
        animator =  GetComponentInChildren<Animator>();

        Vector2 wireCubeSize = new Vector2(0.7f, 0.7f);
        Vector2 wireCubeSizeTop = new Vector2(0.5f,0.5f);
        Vector2 wireCubePos = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Vector2 wireCubePosTop = new Vector2(transform.position.x,transform.position.y + 0.5f);
        isGrounded = Physics2D.OverlapBox(wireCubePos, wireCubeSize, 0, groundMask);
        hitTop = Physics2D.OverlapBox(wireCubePosTop, wireCubeSizeTop, 0, groundMask);
        Debug.Log("isGrounded: " + isGrounded);
        Debug.Log("hitHead: " + hitTop);
        
        float horizontalInput = Input.GetAxisRaw("Horizontal"); 

        if (isGrounded){ //player can only use inputs if not jumping

            myRigidBody.sharedMaterial = normalMat;

            animator.SetBool("isGrounded", true);

            if (Input.GetKey(KeyCode.Space)) { // charging jump
                animator.SetBool("isCharging", true);
                myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
                if (jumpHeight <= 11.5){
                    jumpHeight += .15f;
                }

                //characterRender.color = Color.blue;

            } else if(Input.GetKeyUp(KeyCode.Space)) { // jumping release
                animator.SetBool("isCharging", false);
                animator.SetBool("isGrounded", false);
                if (jumpHeight <= 2){
                    jumpHeight = 2;
                }

                if (myRigidBody.velocity.x != 0){
                    animator.SetBool("isMoving", true);
                }
                float jumpVelocityMagnitude = jumpHeight * 0.3f;
                Vector2 jumpVelocity = new Vector2(jumpVelocityMagnitude * horizontalInput, jumpVelocityMagnitude);
                myRigidBody.velocity = jumpVelocity;
                Vector2 jumpDirection = new Vector2(jumpLength, jumpHeight);
                Debug.Log("jumpDirection:" + jumpDirection);
                myRigidBody.AddForce(jumpDirection, ForceMode2D.Impulse);

                jumpHeight = 0;
            } else if(myRigidBody.velocity.x > 0 + .001f || horizontalInput == 1) { //moving right
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("isMoving", true);
                //characterRender.color = Color.cyan;
                leftright = 0;      
            } else if(myRigidBody.velocity.x < 0 - .001f || horizontalInput == -1) { // moving left
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
                //characterRender.color = Color.magenta;
                animator.SetBool("isMoving", true);
                leftright = 1;
            }
            else {
                animator.SetBool("isMoving", false);
            }
            if (!Input.GetKey(KeyCode.Space)){
                myRigidBody.velocity = new Vector2(horizontalInput * moveSpeed, myRigidBody.velocity.y);
            }

        } else {
            if(!hitTop) {
                myRigidBody.sharedMaterial = bounce;
            }
            
            animator.SetBool("isGrounded", false);
            animator.SetBool("isCharging", false);
            //characterRender.color = Color.red;

        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = isGrounded ? Color.blue : Color.red;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .5f),
        new Vector2(.7f, .7f));
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x,gameObject.transform.position.y + 0.5f),
        new Vector2(.5f,.5f));
    }
}


    