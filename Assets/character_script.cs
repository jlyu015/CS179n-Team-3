using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character_script : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public float moveSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        if(Input.GetKeyDown(KeyCode.Space)) {
            myRigidBody.velocity = Vector2.up * 10;
        }
        else if(Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
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
}

    