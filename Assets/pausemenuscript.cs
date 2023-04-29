using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenuscript : MonoBehaviour {
    public GameObject PauseMenu;
    public GameObject gameCharacter;
    public static bool isPaused;
    public Vector3 savedPosition;

    void Start() {
        PauseMenu.SetActive(false);
        LoadPosition();
    }
    
    
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(isPaused) {
                ResumeGame();
            }
            else {
                PauseGame();
            } //
        }
    }
    
    public void PauseGame() {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame() {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ResumeGameOnClick() {
        ResumeGame();
    }

        public void SavePosition() {
        PlayerPrefs.SetFloat("xPos", gameCharacter.transform.position.x);
        PlayerPrefs.SetFloat("yPos", gameCharacter.transform.position.y);
        PlayerPrefs.SetFloat("zPos", gameCharacter.transform.position.z);
        PlayerPrefs.Save();
    }

    public void OnSaveButtonClick() {
        SavePosition();
    }   

    public void LoadPosition() {
        if (PlayerPrefs.HasKey("xPos") && PlayerPrefs.HasKey("yPos") && PlayerPrefs.HasKey("zPos")) {
            float xPos = PlayerPrefs.GetFloat("xPos");
            float yPos = PlayerPrefs.GetFloat("yPos");
            float zPos = PlayerPrefs.GetFloat("zPos");
            gameCharacter.transform.position = new Vector3(xPos, yPos, zPos);
        }
    }
}