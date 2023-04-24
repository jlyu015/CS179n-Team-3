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
        if(Input.GetKeyDown(KeyCode.Space)) {
            myRigidBody.velocity = Vector2.up * 10;
        }
        else if(Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }
        else if(Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }
        
        
        
        
    }
}
