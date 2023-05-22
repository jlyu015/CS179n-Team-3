using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class endgamescript : MonoBehaviour
{
    public GameObject playerCharacter;
    public GameObject squareObject;
    public TextMeshProUGUI winText;

    private bool gameEnded = false;

    void Start(){
        playerCharacter = GameObject.Find("Circle");
        squareObject = GameObject.Find("Square");
    }
    void Update()
    {
        if (!gameEnded && IsPlayerInBounds())
        {
            gameEnded = true;
            StartCoroutine(EndGameCoroutine());
         }
    }

    private bool IsPlayerInBounds()
    {
        Bounds bounds = squareObject.GetComponent<Renderer>().bounds;
        return bounds.Contains(playerCharacter.transform.position);
    }

    IEnumerator EndGameCoroutine()
    {
        // Player has left the square area, show the "YOU WIN" text
        winText.gameObject.SetActive(true);
        winText.text = "YOU WIN!";

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("mainmenu");
    }
}
