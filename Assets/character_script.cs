using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_script : MonoBehaviour
{
    // Private Variables
    public AudioClip jumpSoundClip; // Reference to the jump sound clip
    private AudioSource jumpSoundSource; // Reference to the AudioSource component
    private float _vertical;
    private float _horizontal;
    public float climbSpeed = 4;
    private bool _isClimbing;
    private HashSet<GameObject> ladders = new HashSet<GameObject>(); //check if multiple ladders are in the collision area
    //above variables are for ladder
    private bool isGrounded = false;
    private bool hitTop = false;
    private bool hitSide = false;
    private bool onIce = false;
    // Public Variables
    public Rigidbody2D myRigidBody;
    public LayerMask groundMask, slopeMask, iceMask;
    public SpriteRenderer characterRender;
    public Animator animator;
    public BoxCollider2D boxCollider;
    public PhysicsMaterial2D bounce, normalMat;
    public float wireCubeSizeBotX = 0.45f;
    public float wireCubeSizeBotY = 0.7f;
    public float wireCubeSizeTopX = 0.6f;
    public float wireCubeSizeTopY = 0.25f;
    public float wireCubesSizeSideX = .45f;
    public float wireCubeSizeSideY = .1f;
    public float groundDist = 1.1f;
    public static float moveSpeed = 7f;
    public static float horizontalInput = 0.0f;
    public float buttonTime = 0.5f;
    public float jumpHeight = 10;
    public float jumpLength = 0;
    public float cancelRate = 100;
    public float dir = 0;
    public float moveInput;
    public float leftright;
    public float slideSpeed = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject audioSourceObject = new GameObject("JumpSoundSource");
        audioSourceObject.transform.parent = transform;

        // Attach an AudioSource component to the new GameObject
        jumpSoundSource = audioSourceObject.AddComponent<AudioSource>();

        // Assign the jump sound clip to the AudioSource
        jumpSoundSource.clip = jumpSoundClip;

        InitializeComponents();
        gameObject.name = "Hop Queen";  // Our Queen
        myRigidBody.gravityScale = 3;   // Gravity
        moveSpeed = 7f;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        CheckLayerMask();

        horizontalInput = Input.GetAxisRaw("Horizontal"); 
        animator.SetFloat("isInAir", myRigidBody.velocity.y);
        // Check if the character is on a slope
        Collider2D slopeHit = SlopeCheck(horizontalInput);
        if (isGrounded){ //player can only use inputs if not jumping
            animator.SetFloat("isMoving", Mathf.Abs(myRigidBody.velocity.x));
            animator.SetBool("isGrounded", isGrounded);
            myRigidBody.sharedMaterial = normalMat;
            // Player presses/holds spacebar and starts charging the jump
            if (Input.GetKey(KeyCode.Space)) {
                // Animation - Charging
                animator.SetBool("isCharging", true);
                myRigidBody.velocity = new Vector2(0, myRigidBody.velocity.y);
                if (jumpHeight <= 15.5){
                    jumpHeight += .30f;
                }
                //characterRender.color = Color.blue;
            } 
            // Player releases spacebar and jumps
            else if(Input.GetKeyUp(KeyCode.Space)) { 
                // Animation - Jump Up
                animator.SetBool("isCharging", true);
                animator.SetBool("isGrounded", isGrounded);
                if (jumpHeight <= 5){
                    jumpHeight = 5;
                }
                float jumpVelocityMagnitude = jumpHeight * 0.3f;
                Vector2 jumpVelocity = new Vector2(jumpVelocityMagnitude * horizontalInput, jumpVelocityMagnitude);
                myRigidBody.velocity = jumpVelocity;
                Vector2 jumpDirection = new Vector2(jumpLength, jumpHeight);
                //Debug.Log("jumpDirection:" + jumpDirection);
                myRigidBody.AddForce(jumpDirection, ForceMode2D.Impulse);
                jumpHeight = 0;
                //jump sound below
                // Adjust the volume of the jump sound
                jumpSoundSource.volume = 0.5f; // Set the volume to 50% (adjust as needed)

                // Play the jump sound
                jumpSoundSource.Play();

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
        // Player is sliding on ice
        else if (onIce) {
            // Player presses/holds spacebar and starts charging the jump
            if (Input.GetKey(KeyCode.Space)) {
                // Animation - Charging
                animator.SetBool("isCharging", true);
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, myRigidBody.velocity.y);
                if (jumpHeight <= 11.5) {
                    jumpHeight += .30f;
                }
                //characterRender.color = Color.blue;
            }
            // Player releases spacebar and jumps
            else if (Input.GetKeyUp(KeyCode.Space)) {
                // Animation - Jump Up
                animator.SetBool("isCharging", true);
                animator.SetBool("isGrounded", isGrounded);
                if (jumpHeight <= 5) {
                    jumpHeight = 5;
                }
                float jumpVelocityMagnitude = jumpHeight * 0.3f;
                Vector2 jumpVelocity = new Vector2(jumpVelocityMagnitude * horizontalInput, jumpVelocityMagnitude);
                myRigidBody.velocity = jumpVelocity;
                Vector2 jumpDirection = new Vector2(jumpLength, jumpHeight);
                myRigidBody.AddForce(jumpDirection, ForceMode2D.Impulse);
                jumpHeight = 0;
            }
        }
        // In the air
        else {
            if(myRigidBody.velocity.y < -20f ) {
                myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, -20f);
                // Debug.Log("Max fall speed: " + myRigidBody.velocity.y);
            }
            if(!hitTop)
            {
                if (slopeHit != null)
                {
                    myRigidBody.sharedMaterial = normalMat;
                    Debug.Log("normal");
                }
                else
                {
                    if(hitSide) {
                        animator.SetBool("hitSide", hitSide);
                        Debug.Log("bounce");
                        myRigidBody.sharedMaterial = bounce;
                    }
                    animator.SetBool("hitSide", hitSide);
                    
                }
            }
            // Animation - Rising
            animator.SetBool("isCharging", false);
            animator.SetBool("isGrounded", isGrounded);
            //characterRender.color = Color.red;
        }
        _horizontal = Input.GetAxisRaw("Horizontal"); //ladder movement
        _vertical = Input.GetAxisRaw("Vertical"); //ladder
        if (ladders.Count > 0 && Mathf.Abs(_vertical) > 0f) {
            _isClimbing = true;
        }
        else if(ladders.Count <= 0) {
            _isClimbing = false;
        }

    }

    private void OnDrawGizmos() {
        Gizmos.color = isGrounded ? Color.blue : Color.red;
        Gizmos.color = !isGrounded && !hitTop && hitSide ? Color.yellow : Color.blue;
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.7f),
        new Vector2(wireCubeSizeBotX, wireCubeSizeBotY));
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x,gameObject.transform.position.y + 0.7f),
        new Vector2(wireCubeSizeTopX,wireCubeSizeTopY));
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x - .35f, gameObject.transform.position.y),
        new Vector2(wireCubesSizeSideX, wireCubeSizeBotY));
        Gizmos.DrawWireCube(new Vector2(gameObject.transform.position.x + .35f, gameObject.transform.position.y),
        new Vector2(wireCubesSizeSideX, wireCubeSizeBotY));
    }

    private void InitializeComponents()
    {
    characterRender = GetComponentInChildren<SpriteRenderer>();
    animator = GetComponentInChildren<Animator>();
    myRigidBody = GetComponent<Rigidbody2D>();
    boxCollider = GetComponent<BoxCollider2D>();
    groundMask = LayerMask.GetMask("terrain");
    slopeMask = LayerMask.GetMask("slopes");
    iceMask = LayerMask.GetMask("ice");
    bounce.bounciness = .5f;
    wireCubeSizeBotX = 0.35f;
    wireCubeSizeBotY = 0.7f;
    wireCubeSizeTopX = 0.6f;
    wireCubeSizeTopY = 0.25f;
    }

    private void CheckLayerMask()
    {
    Vector2 wireCubeSizeBot = new Vector2(wireCubeSizeBotX, wireCubeSizeBotY);
    Vector2 wireCubeSizeTop = new Vector2(wireCubeSizeTopX, wireCubeSizeTopY);
    Vector2 wireCubeSizeSide = new Vector2(wireCubesSizeSideX, wireCubeSizeSideY);
    Vector2 wireCubePos = new Vector2(transform.position.x, transform.position.y - 0.7f);
    Vector2 wireCubePosTop = new Vector2(transform.position.x, transform.position.y + 0.7f);
    Vector2 wireCubePosLeft = new Vector2(transform.position.x - .35f, transform.position.y);
    Vector2 wireCubePosRight = new Vector2(gameObject.transform.position.x + .35f, gameObject.transform.position.y);
    isGrounded = Physics2D.OverlapBox(wireCubePos, wireCubeSizeBot, 0, groundMask) ||
                 Physics2D.OverlapBox(wireCubePos, wireCubeSizeBot, 0, slopeMask);
    onIce = Physics2D.OverlapBox(wireCubePos, wireCubeSizeBot, 0, iceMask);
    hitTop = Physics2D.OverlapBox(wireCubePosTop, wireCubeSizeTop, 0, groundMask);

    hitSide = Physics2D.OverlapBox(wireCubePosLeft, wireCubeSizeSide, 0, groundMask) || 
                Physics2D.OverlapBox(wireCubePosRight, wireCubeSizeSide, 0, groundMask);

    //Debug.Log("isGrounded: " + isGrounded);
    //Debug.Log("hitHead: " + hitTop);
    }

    private Collider2D SlopeCheck(float horizontalInput)
    {
        float slopeDist = 1.1f;     // Dist. for ray to be cast
        Quaternion targetRotation;  // Rotations
        // Cast a ray down from the center of the character's collider to detect slope
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, slopeDist, slopeMask);
        if (hit.collider != null)
        {
            // Get the angle of the surface the character is standing on
            float angle = Vector2.Angle(hit.normal, Vector2.up);

            // Check if the angle is greater than a certain threshold to determine if the character is on a slope
            if (angle > 44.0f && angle < 46.0f)
            {
                // Set the character's movement to follow the slope
                animator.SetBool("isSliding", true);
                Vector2 slopeDirection = Vector2.Reflect(myRigidBody.velocity.normalized, hit.normal).normalized;
                
                // Rotation of sprite depends on the angle of the slope
                if (slopeDirection.x >= 0)
                {
                    targetRotation = Quaternion.Euler(0f, 0f, -45f);
                }
                else
                {
                    targetRotation = Quaternion.Euler(0f, 0f, 45f);
                }

                // Rotate the sprite
                animator.transform.rotation = targetRotation;
                Debug.DrawRay(transform.position, Vector2.down * slopeDist, Color.blue);
            }
            else
            {
                // Reset the character's rotation if they are not on a slope
                animator.SetBool("isSliding", false);
                animator.transform.rotation = Quaternion.identity;
                Debug.DrawRay(transform.position, Vector2.down * slopeDist, Color.red);
            }
        }
        else
        {
            // Reset the character's rotation if they are not on a slope
            animator.SetBool("isSliding", false);
            animator.transform.rotation = Quaternion.identity;
            Debug.DrawRay(transform.position, Vector2.down * slopeDist, Color.red);
        }
        return hit.collider;
    }

    private void FixedUpdate() { // for ladders
        if(_isClimbing) {
            myRigidBody.gravityScale = 0f;
            myRigidBody.velocity = new Vector2(_horizontal * moveSpeed, _vertical * climbSpeed);
        }
        else {
            myRigidBody.gravityScale = 3f;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) { //this and ontriggerexit are for ladder
        if(col.CompareTag("ladder")) {
            ladders.Add(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col) {
        if(col.CompareTag("ladder")) {
            ladders.Remove(col.gameObject);

        }
    }
}
