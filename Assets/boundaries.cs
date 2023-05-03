using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the boundaries of the camera
public class boundaries : MonoBehaviour
{
    // Private Variables
    private Vector2 bounds;         // The bounds of the game object based on the camera's position and the screen size
    private Camera mainCamera;      // Main Camera of game
    private float cameraMoveAmount; // How far the camera will move
    // Public Variables
    public float topMargin = 0.1f;  // Adjust this value to change how close the object needs to be to the top
    public float sideMargin = 0.1f; // Adjust this value to change how close the object needs to be to the sides

    // Start is called before the first frame update
    void Start()
    {
        // This gets the Main Camera from the Scene
        mainCamera = Camera.main;
        // This enables Main Camera
        mainCamera.enabled = true;
        // Set the bounds of the game object based on the camera's position and the screen size
        bounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
    }

    // Update is called once per frame
    void Update() {
        // Check if the game object has moved out of the screen bounds and teleport it to the other side if necessary

        // Get the screen position of the game object
        Vector3 viewPos = mainCamera.WorldToScreenPoint(transform.position);
        // The camera will move the by size of its height
        cameraMoveAmount = 2.0f * mainCamera.orthographicSize;
        // If the game object has moved to the left of the screen, teleport it to the right side
        if(viewPos.x < sideMargin) Teleport(viewPos,1); 
        // If the game object has moved to the right of the screen, teleport it to the left side
        if(viewPos.x > Screen.width - sideMargin) Teleport(viewPos,-1); 
        // If the game object has moved past the top of the screen, move the camera up
        if(viewPos.y > Screen.height - topMargin) MoveCamera(viewPos, 1);
        // If the game object has moved past the bottom of the screen, move the camera down
        if(viewPos.y < topMargin) MoveCamera(viewPos, -1);
    }
    
    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        // Clamp the position of the game object within the screen bounds

        // Get the world position of the game object
        Vector3 viewPos = transform.position; 
        // Clamp the x-position of the game object within the screen bounds
        viewPos.x = Mathf.Clamp(viewPos.x, bounds.x * -1, bounds.x); 
        // Clamp the y-position of the game object within the screen bounds
        //viewPos.y = Mathf.Clamp(viewPos.y, bounds.y * -1, bounds.y); 
        // Set the position of the game object to the clamped position
        transform.position = viewPos; 
    }
    
    // Move the player from one side of the screen to the other side
    // 1 = left to right
    // -1 = right to left
    void Teleport(Vector3 screenPos, int side)
    {
        // Get the position of the opposite side of the screen
        Vector3 oppositePos = new Vector3(Screen.width * side, screenPos.y, screenPos.z);

        // Convert the opposite position to world coordinates and teleport the game object to the opposite side of the screen
        transform.position = mainCamera.ScreenToWorldPoint(oppositePos);
    }

    //Determines which direction to move the camera
    void MoveCamera(Vector3 screenPos, int side)
    {
        // Move camera up
        if(side == 1)
        {
            mainCamera.transform.position += Vector3.up * cameraMoveAmount; // Move up by move amount
        }
        //Move camera down
        else
        {
            mainCamera.transform.position += Vector3.down * cameraMoveAmount; // Move down by move amount
        }
    }
}
