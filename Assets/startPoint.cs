using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startPoint : MonoBehaviour
{
    public string characterTag = "Player";
    public GameObject startPt;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("xPos") && PlayerPrefs.HasKey("yPos") &&PlayerPrefs.HasKey("zPos")) {
            float xPos = PlayerPrefs.GetFloat("xPos");
            float yPos = PlayerPrefs.GetFloat("yPos");
            float zPos = PlayerPrefs.GetFloat("zPos");
            startPt.transform.position = new Vector3(xPos, yPos, zPos);
        }
        else {
            startPt.transform.position = transform.position;
        }
    }

    public void SetDefaultSpawnPosition() {
        startPt.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
