using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the boundaries of the camera
public class boundaries : MonoBehaviour
{
    private Vector2 bounds; // The bounds of the game object based on the camera's position and the screen size
    public float topMargin = 0.1f; // Adjust this value to change how close the object needs to be to the top
    public float sideMargin = 0.1f; // Adjust this value to change how close the object needs to be to the sides

    // Start is called before the first frame update
    void Start()
    {
        // Set the bounds of the game object based on the camera's position and the screen size
        bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update() {
        // Check if the game object has moved out of the screen bounds and teleport it to the other side if necessary

        // Get the screen position of the game object
        Vector3 viewPos = Camera.main.WorldToScreenPoint(transform.position); 
        // If the game object has moved to the left of the screen, teleport it to the right side
        if(viewPos.x < sideMargin) Teleport(viewPos,1); 
        // If the game object has moved to the right of the screen, teleport it to the left side
        if(viewPos.x > Screen.width - sideMargin) Teleport(viewPos,-1); 
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
        viewPos.y = Mathf.Clamp(viewPos.y, bounds.y * -1, bounds.y); 
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

        // Check if the object is close to the top of the screen
        if (screenPos.y > Screen.height - topMargin)
        {
            // If the object is close to the top, don't teleport it
            return;
        }

        // Convert the opposite position to world coordinates and teleport the game object to the opposite side of the screen
        transform.position = Camera.main.ScreenToWorldPoint(oppositePos);
    }
}
