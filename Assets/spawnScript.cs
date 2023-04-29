using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnScript : MonoBehaviour
{
    public GameObject spawnObject;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("xPos") && PlayerPrefs.HasKey("yPos") &&PlayerPrefs.HasKey("zPos")) {
            float xPos = PlayerPrefs.GetFloat("xPos");
            float yPos = PlayerPrefs.GetFloat("yPos");
            float zPos = PlayerPrefs.GetFloat("zPos");
            spawnObject.transform.position = new Vector3(xPos, yPos, zPos);
        }
        else {
            spawnObject.transform.position = transform.position;
        }
    }

    public void SetDefaultSpawnPosition() {
        spawnObject.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
