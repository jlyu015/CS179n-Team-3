using UnityEngine;
using UnityEngine.SceneManagement;

public class mainmenuscript : MonoBehaviour
{
    private string sceneName;
    private void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void NewGame()
    {
        // Set the scene name
        sceneName = "TestTransition";

        // Load the game scene
        SceneManager.LoadScene(sceneName);
        PlayerPrefs.DeleteKey("xPos");
        PlayerPrefs.DeleteKey("yPos");
        PlayerPrefs.DeleteKey("zPos");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the loaded scene is the popup scene
        if (scene.name == sceneName)
        {
            // Find the spawnObject game object in the popup scene
            GameObject spawnObject = GameObject.Find("spawnObject");

            // Set the player's position to the default spawn position
        }
    }

public void LoadGame()
{
    // Check if the saved position exists
    if (PlayerPrefs.HasKey("xPos") && PlayerPrefs.HasKey("yPos") && PlayerPrefs.HasKey("zPos"))
    {
        // Set the player's position to the saved position
        float xPos = PlayerPrefs.GetFloat("xPos");
        float yPos = PlayerPrefs.GetFloat("yPos");
        float zPos = PlayerPrefs.GetFloat("zPos");

        // Load the game scene
        sceneName = "TestTransition";
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        // Find the spawnObject game object in the popup scene
        GameObject spawnObject = GameObject.Find("spawnObject");

        // Set the player's position to the saved position
    }
    else
    {
        // If the saved position doesn't exist, start a new game
        NewGame();
    }
}


    public void ExitGame()
    {
        // Exit the game
        Application.Quit();
    }
}