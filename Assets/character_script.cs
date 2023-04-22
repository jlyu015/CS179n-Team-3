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
        gameObject.name = "Hop Queen";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            myRigidBody.velocity = Vector2.up * 5;
        }
        else if(Input.GetKey(KeyCode.A)) {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(Input.GetKey(KeyCode.D)) {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        

        
        
        
    }
}
